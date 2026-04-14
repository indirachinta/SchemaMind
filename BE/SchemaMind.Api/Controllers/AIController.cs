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
        private readonly SchemaService _schemaService;
        private readonly ContextBuilder _contextBuilder;
        private readonly AIService _aiService;

        public AIController(
            SchemaService schemaService,
            ContextBuilder contextBuilder,

            AIService aiService)
        {
            _schemaService = schemaService;
            _contextBuilder = contextBuilder;
            _aiService = aiService;
        }

        [HttpPost("generate-sql")]
        public async Task<QueryAndResults> GenerateSql([FromBody] RequestModel requestModel)
        {
            if (ModelState.IsValid)
            {

                var results = await _aiService.GenerateSql(requestModel.question, requestModel.connection);


                return results;
            }
            return null;
        }
    }
}
