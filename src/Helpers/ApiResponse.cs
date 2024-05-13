using Microsoft.AspNetCore.Mvc;


namespace Backend.Helpers
{
    public static class ApiResponse
    {
        // Method to return a successful response with data
        public static IActionResult Success<T>(T data, string message = "Successfully")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 200));
        }

        // Method to return a successful response for resource creation
        public static IActionResult Created<T>(T data, string message = "Successfully Created")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 201));
        }

 // Method to return a successful response for resource deletion
        public static IActionResult Deleted( string message = "Successfully Deleted")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(true,null ,message, 204));
        }
    }

    // Class to define the structure of the API response template
    public class ApiResponseTemplate<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ApiResponseTemplate(bool success, T? data, string message, int statusCode = 200)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}