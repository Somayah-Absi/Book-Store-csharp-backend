using Microsoft.AspNetCore.Mvc;


namespace Backend.Helpers
{
    public static class ApiResponse
    {
        // Method to return a successful response with data
        public static IActionResult Success<T>(T data, string message = "Success")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 200));
        }

        // Method to return a successful response for resource creation
        public static IActionResult Created<T>(T data, string message = "Resource Created")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 201));
        }

        // Method to return a response for a resource not found
        public static IActionResult NotFound(string message = "Resource not found")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 404));
        }

        // Method to return a response for a conflict
        public static IActionResult Conflict(string message = "Conflict Detected")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 409));
        }

        // Method to return a response for a bad request
        public static IActionResult BadRequest(string message = "Bad request")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 400));
        }

        // Method to return a response for unauthorized access
        public static IActionResult UnAuthorized(string message = "Unauthorized access")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 401));
        }

        // Method to return a response for forbidden access
        public static IActionResult Forbidden(string message = "Forbidden access")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 403));
        }

        // Method to return a response for internal server error
        public static IActionResult ServerError(string message = "Internal server error")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 500));
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