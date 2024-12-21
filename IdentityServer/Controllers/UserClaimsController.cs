using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Contexts;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserClaimsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserClaims
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserClaim>>> GetUserClaims()
        {
            return await _context.UserClaims.ToListAsync();
        }

        // GET: api/UserClaims/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserClaim>> GetUserClaim(int id)
        {
            var userClaim = await _context.UserClaims.FindAsync(id);

            if (userClaim == null)
            {
                return NotFound();
            }

            return userClaim;
        }

        // PUT: api/UserClaims/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserClaim(int id, UserClaim userClaim)
        {
            if (id != userClaim.Id)
            {
                return BadRequest();
            }

            _context.Entry(userClaim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserClaimExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserClaims
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserClaim>> PostUserClaim(UserClaim userClaim)
        {
            _context.UserClaims.Add(userClaim);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserClaim", new { id = userClaim.Id }, userClaim);
        }

        // DELETE: api/UserClaims/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserClaim(int id)
        {
            var userClaim = await _context.UserClaims.FindAsync(id);
            if (userClaim == null)
            {
                return NotFound();
            }

            _context.UserClaims.Remove(userClaim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserClaimExists(int id)
        {
            return _context.UserClaims.Any(e => e.Id == id);
        }
    }
}
