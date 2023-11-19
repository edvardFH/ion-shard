using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IonShard.Swagger;

public class RequestBodiesDocumentFilter : IDocumentFilter
{
    private readonly string[] _blacklist = { "CreateUserPutRequestBody", "MoveUnitPutRequestBody", "MoveUnitPutRequestBody", "CreateBuildingPostRequestBody" };
    public void Apply(OpenApiDocument schema, DocumentFilterContext context) 
        => _blacklist
            .ToList()
            .ForEach(key => context.SchemaRepository.Schemas.Remove(key));
}
