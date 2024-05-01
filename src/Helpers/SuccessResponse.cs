namespace api.Helpers
{
    public class SuccessResponse<T> 
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public required T Data { get; set; }
    }
    
}