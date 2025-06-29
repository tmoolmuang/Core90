using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase {
        private readonly IConfiguration _configuration;

        public ProfilesController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("SupportDB"));

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetAll() {
            using var connection = GetConnection();
            var profiles = await connection.QueryAsync<Profile>("SELECT * FROM Profile");
            return Ok(profiles);
        }

        // GET: api/Profiles/{profileId}
        [HttpGet("{profileId}")]
        public async Task<ActionResult<Profile>> GetById(int profileId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM Profile WHERE ProfileId = @ProfileId";
            var profile = await connection.QueryFirstOrDefaultAsync<Profile>(sql, new { ProfileId = profileId });

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        // POST: api/Profiles
        [HttpPost]
        public async Task<ActionResult> Create(Profile profile) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO Profile (UserName, FirstName, MiddleName, LastName, Active, Email, OneHealthcareUuid)
                VALUES (@UserName, @FirstName, @MiddleName, @LastName, @Active, @Email, @OneHealthcareUuid)";

            await connection.ExecuteAsync(sql, profile);

            return Ok();
        }

        // PUT: api/Profiles/{profileId}
        [HttpPut("{profileId}")]
        public async Task<ActionResult> Update(int profileId, Profile profile) {
            if (profileId != profile.ProfileId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE Profile
                SET UserName = @UserName,
                    FirstName = @FirstName,
                    MiddleName = @MiddleName,
                    LastName = @LastName,
                    Active = @Active,
                    Email = @Email,
                    OneHealthcareUuid = @OneHealthcareUuid
                WHERE ProfileId = @ProfileId";

            var rows = await connection.ExecuteAsync(sql, profile);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/Profiles/{profileId}
        [HttpDelete("{profileId}")]
        public async Task<ActionResult> Delete(int profileId) {
            using var connection = GetConnection();
            var sql = "DELETE FROM Profile WHERE ProfileId = @ProfileId";
            var rows = await connection.ExecuteAsync(sql, new { ProfileId = profileId });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
