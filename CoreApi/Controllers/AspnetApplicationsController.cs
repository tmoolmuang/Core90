using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreApi.Models;
using Microsoft.Data.SqlClient;
using Dapper;

//table aspnet_Applications does not have a primary key, we will use Dapper for direct SQL execution instead of EF.
//All object properties needs to be passed in when posting.

namespace CoreApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetApplicationsController : ControllerBase {
        private readonly string _connectionString;

        public AspnetApplicationsController(IConfiguration configuration) {
            IConfiguration config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = config.GetConnectionString("SupportTestDB")?? throw new InvalidOperationException("Connection string 'SupportTestDB' is not configured.");
        }

        // GET: api/AspnetApplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetApplication>>> GetAspnetApplications() {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT ApplicationId, ApplicationName, LoweredApplicationName, Description FROM aspnet_Applications";

            var applications = await connection.QueryAsync<AspnetApplication>(query);

            return Ok(applications);
        }

        // GET: api/AspnetApplications/00000000-0000-0000-0000-000000000000
        [HttpGet("{ApplicationId}")]
        public async Task<ActionResult<AspnetApplication>> GetAspnetApplication(string ApplicationId) {
            if (!Guid.TryParse(ApplicationId, out Guid appId))
                return BadRequest("Invalid GUID format.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM aspnet_Applications WHERE ApplicationId = @ApplicationId";
            var application = await connection.QuerySingleOrDefaultAsync<AspnetApplication>(query, new { ApplicationId = appId });

            return application is null ? NotFound() : Ok(application);
        }

        // PUT: api/AspnetApplications/00000000-0000-0000-0000-000000000000
        [HttpPut("{ApplicationId}")]
        public async Task<IActionResult> UpdateApplication(string ApplicationId, AspnetApplication updatedApp) {
            if (!Guid.TryParse(ApplicationId, out Guid appId) || appId != updatedApp.ApplicationId)
                return BadRequest("Invalid ApplicationId.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var exists = await connection.QuerySingleOrDefaultAsync<bool>(
                "SELECT COUNT(1) FROM aspnet_Applications WHERE ApplicationId = @ApplicationId",
                new { updatedApp.ApplicationId });

            if (!exists) return NotFound("ApplicationId not found.");

            var query = @"  UPDATE aspnet_Applications
                            SET LoweredApplicationName = @LoweredApplicationName,
                                Description = @Description,
                                ApplicationName = @ApplicationName
                            WHERE ApplicationId = @ApplicationId";

            await connection.ExecuteAsync(query, new {
                updatedApp.LoweredApplicationName,
                updatedApp.Description,
                updatedApp.ApplicationName,
                updatedApp.ApplicationId
            });

            return NoContent();
        }

        // POST: api/AspnetApplications
        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] AspnetApplication app) {
            if (app == null) return BadRequest("Invalid application data.");

            app.ApplicationId = app.ApplicationId == Guid.Empty ? Guid.NewGuid() : app.ApplicationId;

            var sql = @"INSERT INTO aspnet_Applications (ApplicationName, LoweredApplicationName, ApplicationId, Description)
                        VALUES (@ApplicationName, @LoweredApplicationName, @ApplicationId, @Description)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new {
                app.ApplicationName,
                app.LoweredApplicationName,
                app.ApplicationId,
                app.Description
            };

            try {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0
                    ? Ok(new { message = "Application created successfully.", applicationId = app.ApplicationId })
                    : StatusCode(500, "Failed to insert application.");
            }
            catch (SqlException ex) {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        // DELETE: api/AspnetApplications/00000000-0000-0000-0000-000000000000
        [HttpDelete("{ApplicationId}")]
        public async Task<IActionResult> DeleteApplication(string ApplicationId) {
            if (!Guid.TryParse(ApplicationId, out Guid appId))
                return BadRequest("Invalid GUID format.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM aspnet_Applications WHERE ApplicationId = @ApplicationId";

            var affectedRows = await connection.ExecuteAsync(query, new { ApplicationId = appId });

            return affectedRows > 0 ? NoContent() : NotFound("Application not found.");
        }
    }
}
