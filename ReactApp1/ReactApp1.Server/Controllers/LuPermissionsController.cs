using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LuPermissionsController : ControllerBase {
        private readonly IConfiguration _configuration;

        public LuPermissionsController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("SupportDB"));

        // GET: api/LuPermissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LuPermission>>> GetAll() {
            using var connection = GetConnection();
            var permissions = await connection.QueryAsync<LuPermission>("SELECT * FROM lu_Permission");
            return Ok(permissions);
        }

        // GET: api/LuPermissions/{permissionId}
        [HttpGet("{permissionId}")]
        public async Task<ActionResult<LuPermission>> GetById(short permissionId) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM lu_Permission WHERE PermissionId = @PermissionId";
            var permission = await connection.QueryFirstOrDefaultAsync<LuPermission>(sql, new { PermissionId = permissionId });

            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        // POST: api/LuPermissions
        [HttpPost]
        public async Task<ActionResult> Create(LuPermission permission) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO lu_Permission (PermissionId, PermissionTitle, PermissionDescription, ApplicationId)
                VALUES (@PermissionId, @PermissionTitle, @PermissionDescription, @ApplicationId)";

            await connection.ExecuteAsync(sql, permission);

            return Ok();
        }

        // PUT: api/LuPermissions/{permissionId}
        [HttpPut("{permissionId}")]
        public async Task<ActionResult> Update(short permissionId, LuPermission permission) {
            if (permissionId != permission.PermissionId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE lu_Permission
                SET PermissionTitle = @PermissionTitle,
                    PermissionDescription = @PermissionDescription,
                    ApplicationId = @ApplicationId
                WHERE PermissionId = @PermissionId";

            var rows = await connection.ExecuteAsync(sql, permission);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/LuPermissions/{permissionId}
        [HttpDelete("{permissionId}")]
        public async Task<ActionResult> Delete(short permissionId) {
            using var connection = GetConnection();
            var sql = "DELETE FROM lu_Permission WHERE PermissionId = @PermissionId";
            var rows = await connection.ExecuteAsync(sql, new { PermissionId = permissionId });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
