using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IonShard;

public class RequestBodiesDocumentFilter : IDocumentFilter
{
    private readonly string[] blacklist = { "CreateUserPutRequestBody", "MoveUnitPutRequestBody" };
    public void Apply(OpenApiDocument schema, DocumentFilterContext context)
    {
        blacklist.ToList().ForEach(key => context.SchemaRepository.Schemas.Remove(key));
    }
}
