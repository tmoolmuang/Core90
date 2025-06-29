using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AspnetApplicationsController : ControllerBase {
        private readonly IConfiguration _configuration;

        public AspnetApplicationsController(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection GetConnection() {
            return new SqlConnection(_configuration.GetConnectionString("SupportDB"));
        }

        // GET: api/AspnetApplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspnetApplication>>> GetAll() {
            using var connection = GetConnection();
            var apps = await connection.QueryAsync<AspnetApplication>("SELECT * FROM aspnet_Applications");
            return Ok(apps);
        }

        // GET: api/AspnetApplications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AspnetApplication>> GetById(Guid id) {
            using var connection = GetConnection();
            var sql = "SELECT * FROM aspnet_Applications WHERE ApplicationId = @Id";
            var app = await connection.QueryFirstOrDefaultAsync<AspnetApplication>(sql, new { Id = id });

            if (app == null)
                return NotFound();

            return Ok(app);
        }

        // POST: api/AspnetApplications
        [HttpPost]
        public async Task<ActionResult> Create(AspnetApplication app) {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO aspnet_Applications (ApplicationName, LoweredApplicationName, ApplicationId, Description)
                VALUES (@ApplicationName, @LoweredApplicationName, @ApplicationId, @Description)";

            await connection.ExecuteAsync(sql, app);

            return Ok();
        }

        // PUT: api/AspnetApplications/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, AspnetApplication app) {
            if (id != app.ApplicationId)
                return BadRequest("ID mismatch.");

            using var connection = GetConnection();
            var sql = @"
                UPDATE aspnet_Applications
                SET ApplicationName = @ApplicationName,
                    LoweredApplicationName = @LoweredApplicationName,
                    Description = @Description
                WHERE ApplicationId = @ApplicationId";

            var rows = await connection.ExecuteAsync(sql, app);

            if (rows == 0)
                return NotFound();

            return Ok();
        }

        // DELETE: api/AspnetApplications/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) {
            using var connection = GetConnection();
            var sql = "DELETE FROM aspnet_Applications WHERE ApplicationId = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            if (rows == 0)
                return NotFound();

            return Ok();
        }
    }
}
