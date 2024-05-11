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
            var requestDtoType = context.MethodInfo.GetParameters().Select(p => p.ParameterType).FirstOrDefault();

            // If request DTO type is not found, return
            if (requestDtoType == null)
                return;

            // Dictionary to hold examples for the request body
            var examplesDictionary = new Dictionary<string, OpenApiExample>();

            // Check if the request DTO type is for registration
            if (requestDtoType.Name.Contains("RegisterDto"))
            {
                // Example data for registration request
                var registrationExample = new OpenApiObject
                {
                    ["firstName"] = new OpenApiString("Raghad"),
                    ["lastName"] = new OpenApiString("Ali"),
                    ["email"] = new OpenApiString("Raghad.ali@example.com"),
                    ["password"] = new OpenApiString("P@ssw0rd"),
                    ["mobile"] = new OpenApiString("1234567890")
                };

                // Add registration example to the dictionary
                examplesDictionary.Add("registrationExample", new OpenApiExample
                {
                    Value = registrationExample,
                    Summary = "Register User Example",
                    Description = "An example of a request body for registering a new user."
                });
            }
            // Check if the request DTO type is for creating an order
            else if (requestDtoType.Name.Contains("OrderCreationDto"))
            {
                // Example data for creating an order request
                var orderCreationExample = new OpenApiObject
                {
                    ["newOrder"] = new OpenApiObject
                    {
                        ["orderStatus"] = new OpenApiString("Canceled"),
                        ["payment"] = new OpenApiString("Cash On Delivery"),
                        ["userId"] = new OpenApiInteger(1)
                    },
                    ["products"] = new OpenApiArray
                    {
                        new OpenApiObject
                        {
                            ["productId"] = new OpenApiInteger(1),
                            ["quantity"] = new OpenApiInteger(1)
                        }
                    }
                };

                // Add create order example to the dictionary
                examplesDictionary.Add("orderCreationExample", new OpenApiExample
                {
                    Value = orderCreationExample,
                    Summary = "Create Order Example",
                    Description = "An example of a request body for creating a new order."
                });
            }

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
