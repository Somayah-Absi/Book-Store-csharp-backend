using System.Net;
using Backend;

// ErrorResponse class represents an error response with a message. - written by Nada, commited by Sadeem.
public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
}