using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer;
using IdentityServer.Contexts;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
string persistConnectionString = builder.Configuration.GetConnectionString("Persist");
string configConnectionString = builder.Configuration.GetConnectionString("Configuration");
string ConnectionString = builder.Configuration.GetConnectionString("App");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
});


builder.Services.AddIDSIdentity();
builder.Services.AddIdentityServer()
            .AddConfigurationStore<IDSConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(configConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore<PersistedGrantStoreContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(persistConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

    if (!dbContext.Database.CanConnect())
    {
        dbContext.Database.Migrate();
    }

    if (!dbContext.Users.Any())
    {
        var user = new User
        {
            UserName = "admin",
            Email = "admin@api.com"
        };

        userManager.CreateAsync(user, "Admin@123").Wait();

    }
    var configurationDbContext = serviceProvider.GetRequiredService<IDSConfigurationDbContext>();
    if (!configurationDbContext.Database.CanConnect())
    {
        configurationDbContext.Database.Migrate();
    }

    if (!configurationDbContext.Clients.Any())
    {
        Config.Clients.ToList().ForEach(client =>
        {
            configurationDbContext.Clients.Add(client.ToEntity());
        });
        configurationDbContext.SaveChanges();
    }
    if (!configurationDbContext.IdentityResources.Any())
    {
        Config.IdentityResources.ToList().ForEach(identityResource =>
        {
            configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
        });
        configurationDbContext.SaveChanges();
    }
    if (!configurationDbContext.ApiScopes.Any())
    {
        Config.ApiScopes.ToList().ForEach(apiScope =>
        {
            configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
        });
        configurationDbContext.SaveChanges();

    }


    var persistDb= serviceProvider.GetRequiredService<PersistedGrantStoreContext>();
    if (!persistDb.Database.CanConnect())
    {
        persistDb.Database.Migrate();
    }
}
app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
