using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Helpers
{
    // A class to configure Swagger examples for API operations
    public class ConfigureSwaggerExamples : IOperationFilter
    {
        // Implementation of the Apply method from IOperationFilter interface
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if request body is null or has no content
            if (operation.RequestBody == null || operation.RequestBody.Content.Count == 0)
                return;

            // Get the type of the request DTO (Data Transfer Object)
            var requestDtoType = context.MethodInfo.GetParameters().Select(p => p.ParameterType)
                                        .FirstOrDefault(t => t.Name.Contains("RegisterDto"));

            // If request DTO type is not found, return
            if (requestDtoType == null)
                return;

            // Define an example object for the request body
            var registerationExample = new OpenApiObject
            {
                ["firstName"] = new OpenApiString("Raghad"),
                ["lastName"] = new OpenApiString("Ali"),
                ["email"] = new OpenApiString("Raghad.ali@example.com"),
                ["password"] = new OpenApiString("P@ssw0rd"),
                ["mobile"] = new OpenApiString("1234567890")
            };

            // Create a dictionary to hold the example with a key
            var examplesDictionary = new Dictionary<string, OpenApiExample>
            {
                {
                    "registerationExample",
                    new OpenApiExample
                    {
                        Value = registerationExample,
                        Summary = "Register User Example",
                        Description = "An example of a request body for registering a new user."
                    }
                }
            };

            // Assign the examples dictionary to the request body content
            operation.RequestBody.Content["application/json"].Examples = examplesDictionary;
        }
    }

    // Extension method to add Swagger examples configuration
    public static class SwaggerServiceExtensions
    {

        // Method to add Swagger examples using SwaggerGenOptions
        public static void AddSwaggerExamples(this SwaggerGenOptions options)
        {
            // Register the ConfigureSwaggerExamples class as an operation filter
            options.OperationFilter<ConfigureSwaggerExamples>();
        }
    }
}
