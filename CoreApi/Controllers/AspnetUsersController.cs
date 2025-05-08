using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreApi.Models;

namespace CoreApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetUsersController : ControllerBase {
        private readonly SupportTestContext _context;

        public AspnetUsersController(SupportTestContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //GET: api/AspnetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetUser>>> GetAspnetUsers() {
            return await _context.AspnetUsers.ToListAsync();
        }

        //// GET: api/AspnetUsers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Profile>> GetProfile(int id) {
        //    var profile = await _context.Profiles.FindAsync(id);

        //    if (profile == null) {
        //        return NotFound();
        //    }

        //    return profile;
        //}

        //// PUT: api/Profiles/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProfile(int id, Profile profile) {
        //    if (id != profile.ProfileId) {
        //        return BadRequest();
        //    }

        //    _context.Entry(profile).State = EntityState.Modified;

        //    try {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException) {
        //        if (!ProfileExists(id)) {
        //            return NotFound();
        //        }
        //        else {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Profiles
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Profile>> PostProfile(Profile profile) {
        //    _context.Profiles.Add(profile);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProfile", new { id = profile.ProfileId }, profile);
        //}

        //// DELETE: api/Profiles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProfile(int id) {
        //    var profile = await _context.Profiles.FindAsync(id);
        //    if (profile == null) {
        //        return NotFound();
        //    }

        //    _context.Profiles.Remove(profile);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProfileExists(int id) {
        //    return _context.Profiles.Any(e => e.ProfileId == id);
        //}
    }
}
