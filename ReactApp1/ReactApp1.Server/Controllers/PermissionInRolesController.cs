using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionInRolesController : ControllerBase {
        private readonly IConfiguration _configuration;

        public PermissionInRolesController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("SupportDB"));

        // GET: api/PermissionInRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionInRole>>> GetAll() {
            using var connection = GetConnection();
            var permissionsInRoles = await connection.QueryAsync<PermissionInRole>("SELECT * FROM PermissionInRole");
            return Ok(permissionsInRoles);
        }

        // GET: api/PermissionInRoles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionInRole>> GetById(short id) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM PermissionInRole WHERE PermissionInRoleId = @Id";
            var permissionInRole = await connection.QueryFirstOrDefaultAsync<PermissionInRole>(sql, new { Id = id });

            if (permissionInRole == null)
                return NotFound();

            return Ok(permissionInRole);
        }

        // POST: api/PermissionInRoles
        [HttpPost]
        public async Task<ActionResult> Create(PermissionInRole permissionInRole) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO PermissionInRole (PermissionInRoleId, RoleId, PermissionId)
                VALUES (@PermissionInRoleId, @RoleId, @PermissionId)";
            await connection.ExecuteAsync(sql, permissionInRole);
            return Ok();
        }

        // PUT: api/PermissionInRoles/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(short id, PermissionInRole permissionInRole) {
            if (id != permissionInRole.PermissionInRoleId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE PermissionInRole
                SET RoleId = @RoleId,
                    PermissionId = @PermissionId
                WHERE PermissionInRoleId = @PermissionInRoleId";

            var rows = await connection.ExecuteAsync(sql, permissionInRole);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/PermissionInRoles/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(short id) {
            using var connection = GetConnection();
            var sql = "DELETE FROM PermissionInRole WHERE PermissionInRoleId = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
