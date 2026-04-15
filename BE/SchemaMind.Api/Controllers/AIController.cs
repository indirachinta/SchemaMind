using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchemaMind.Api.Models;
using SchemaMind.Api.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SchemaMind.Api.Controllers
{
    [ApiController]
    [Route("ai")]
    public class AIController : ControllerBase
    {
        
        private readonly AIService _aiService;

        public AIController(
            
            AIService aiService)
        {
            
            _aiService = aiService;
        }

        [HttpPost("generate-sql")]
        public async Task<ActionResult<QueryAndResults>> GenerateSql([FromBody] QueryRequest requestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var results = await _aiService.GenerateSql(requestModel.Question!, requestModel.ConnectionString!);
            return Ok(results);
        }
    }
}
