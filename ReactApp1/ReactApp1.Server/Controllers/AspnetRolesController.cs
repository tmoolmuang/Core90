using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetRolesController : ControllerBase {
        private readonly IConfiguration _configuration;

        public AspnetRolesController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection() {
            return new SqlConnection(_configuration.GetConnectionString("SupportDB"));
        }

        // GET: api/AspnetRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetRole>>> GetAll() {
            using var connection = GetConnection();
            var roles = await connection.QueryAsync<AspnetRole>("SELECT * FROM aspnet_Roles");
            return Ok(roles);
        }

        // GET: api/AspnetRoles/{roleId}
        [HttpGet("{roleId}")]
        public async Task<ActionResult<AspnetRole>> GetById(Guid roleId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM aspnet_Roles WHERE RoleId = @RoleId";
            var role = await connection.QueryFirstOrDefaultAsync<AspnetRole>(sql, new { RoleId = roleId });

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        // POST: api/AspnetRoles
        [HttpPost]
        public async Task<ActionResult> Create(AspnetRole role) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO aspnet_Roles (ApplicationId, RoleId, RoleName, LoweredRoleName, Description)
                VALUES (@ApplicationId, @RoleId, @RoleName, @LoweredRoleName, @Description)";

            await connection.ExecuteAsync(sql, role);

            return Ok();
        }

        // PUT: api/AspnetRoles/{roleId}
        [HttpPut("{roleId}")]
        public async Task<ActionResult> Update(Guid roleId, AspnetRole role) {
            if (roleId != role.RoleId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE aspnet_Roles
                SET ApplicationId = @ApplicationId,
                    RoleName = @RoleName,
                    LoweredRoleName = @LoweredRoleName,
                    Description = @Description
                WHERE RoleId = @RoleId";

            var rows = await connection.ExecuteAsync(sql, role);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/AspnetRoles/{roleId}
        [HttpDelete("{roleId}")]
        public async Task<ActionResult> Delete(Guid roleId) {
            using var connection = GetConnection();
            var sql = "DELETE FROM aspnet_Roles WHERE RoleId = @RoleId";
            var rows = await connection.ExecuteAsync(sql, new { RoleId = roleId });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
