using System.Net;
using Backend;

// SuccessResponse<T> class represents a successful response with a message and associated data of type T. - written by Nada, commited by Sadeem.
public class SuccessResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
}
