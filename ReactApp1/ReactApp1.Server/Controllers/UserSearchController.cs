using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserSearchController : ControllerBase {
        private readonly IConfiguration _config;

        public UserSearchController(IConfiguration config) {
            _config = config;
        }

        private SqlConnection GetConnection() {
            return new SqlConnection(_config.GetConnectionString("SupportDB"));
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] Guid? applicationId,
            [FromQuery] string? userName,
            [FromQuery] string? lastName,
            [FromQuery] string? firstName) {
            using var connection = GetConnection();

            var sql = @"
                SELECT DISTINCT 
                    ap.ApplicationName, 
                    ap.ApplicationId, 
                    us.UserId, 
                    us.UserName, 
                    pf.FirstName, 
                    pf.LastName, 
                    pf.Active
                FROM Support.dbo.aspnet_Applications AS ap
                LEFT JOIN Support.dbo.aspnet_Users AS us ON us.ApplicationId = ap.ApplicationId
                LEFT JOIN Support.dbo.Profile AS pf ON pf.UserName = us.UserName
                WHERE (@ApplicationId IS NULL OR ap.ApplicationId = @ApplicationId)
                  AND (@UserName IS NULL OR us.UserName = @UserName)
                  AND (@LastName IS NULL OR pf.LastName = @LastName)
                  AND (@FirstName IS NULL OR pf.FirstName = @FirstName)
                ORDER BY ap.ApplicationName, us.UserId";

            try {
                var results = await connection.QueryAsync(sql, new {
                    ApplicationId = applicationId,
                    UserName = userName,
                    LastName = lastName,
                    FirstName = firstName
                });

                return Ok(results);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Database query failed: {ex.Message}");
            }
        }
    }
}
