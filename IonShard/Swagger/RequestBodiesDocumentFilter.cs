using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IonShard.Swagger;

public class RequestBodiesDocumentFilter : IDocumentFilter
{
    private readonly string[] blacklist = { "CreateUserPutRequestBody", "MoveUnitPutRequestBody" };
    public void Apply(OpenApiDocument schema, DocumentFilterContext context)
    {
        blacklist.ToList().ForEach(key => context.SchemaRepository.Schemas.Remove(key));
    }
}
