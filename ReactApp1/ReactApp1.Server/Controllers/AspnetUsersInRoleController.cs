using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetUsersInRoleController : ControllerBase {
        private readonly IConfiguration _configuration;

        public AspnetUsersInRoleController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("SupportDB"));

        // GET: api/AspnetUsersInRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetUsersInRole>>> GetAll() {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<AspnetUsersInRole>("SELECT * FROM aspnet_UsersInRoles");
            return Ok(result);
        }

        // GET: api/AspnetUsersInRole/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AspnetUsersInRole>>> GetByUserId(Guid userId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM aspnet_UsersInRoles WHERE UserId = @UserId";
            var roles = await connection.QueryAsync<AspnetUsersInRole>(sql, new { UserId = userId });
            return Ok(roles);
        }

        // GET: api/AspnetUsersInRole/role/{roleId}
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<AspnetUsersInRole>>> GetByRoleId(Guid roleId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM aspnet_UsersInRoles WHERE RoleId = @RoleId";
            var users = await connection.QueryAsync<AspnetUsersInRole>(sql, new { RoleId = roleId });
            return Ok(users);
        }

        // POST: api/AspnetUsersInRole
        [HttpPost]
        public async Task<ActionResult> AddUserToRole(AspnetUsersInRole userRole) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO aspnet_UsersInRoles (UserId, RoleId)
                VALUES (@UserId, @RoleId)";
            await connection.ExecuteAsync(sql, userRole);
            return Ok();
        }

        // DELETE: api/AspnetUsersInRole/user/{userId}/role/{roleId}
        [HttpDelete("user/{userId}/role/{roleId}")]
        public async Task<ActionResult> RemoveUserFromRole(Guid userId, Guid roleId) {
            using var connection = GetConnection();
            var sql = @"
                DELETE FROM aspnet_UsersInRoles 
                WHERE UserId = @UserId AND RoleId = @RoleId";
            var affected = await connection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
            if (affected == 0)
                return NotFound();

            return Ok();
        }
    }
}
