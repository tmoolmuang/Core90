using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetUsersController : ControllerBase {
        private readonly IConfiguration _configuration;

        public AspnetUsersController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection() {
            return new SqlConnection(_configuration.GetConnectionString("SupportDB"));
        }

        // GET: api/AspnetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetUser>>> GetAll() {
            using var connection = GetConnection();
            var users = await connection.QueryAsync<AspnetUser>("SELECT * FROM aspnet_Users");
            return Ok(users);
        }

        // GET: api/AspnetUsers/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<AspnetUser>> GetById(Guid userId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM aspnet_Users WHERE UserId = @UserId";
            var user = await connection.QueryFirstOrDefaultAsync<AspnetUser>(sql, new { UserId = userId });

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/AspnetUsers
        [HttpPost]
        public async Task<ActionResult> Create(AspnetUser user) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, MobileAlias, IsAnonymous, LastActivityDate)
                VALUES (@ApplicationId, @UserId, @UserName, @LoweredUserName, @MobileAlias, @IsAnonymous, @LastActivityDate)";

            await connection.ExecuteAsync(sql, user);

            return Ok();
        }

        // PUT: api/AspnetUsers/{userId}
        [HttpPut("{userId}")]
        public async Task<ActionResult> Update(Guid userId, AspnetUser user) {
            if (userId != user.UserId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE aspnet_Users
                SET ApplicationId = @ApplicationId,
                    UserName = @UserName,
                    LoweredUserName = @LoweredUserName,
                    MobileAlias = @MobileAlias,
                    IsAnonymous = @IsAnonymous,
                    LastActivityDate = @LastActivityDate
                WHERE UserId = @UserId";

            var rows = await connection.ExecuteAsync(sql, user);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/AspnetUsers/{userId}
        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(Guid userId) {
            using var connection = GetConnection();
            var sql = "DELETE FROM aspnet_Users WHERE UserId = @UserId";
            var rows = await connection.ExecuteAsync(sql, new { UserId = userId });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
