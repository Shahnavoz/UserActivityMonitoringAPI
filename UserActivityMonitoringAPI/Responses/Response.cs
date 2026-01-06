using System.Net;

namespace UserActivityMonitoringAPI.Responses;

public class Response<T>
{
    public int StatusCode { get; set; }
    public List<string> Descriptions { get; set; } = new List<string>();
    public T Data { get; set; }

    public Response(HttpStatusCode statusCode,List<string> descriptions, T data)
    {
        StatusCode = (int)statusCode;
        Descriptions = descriptions;
        Data = data;
    } 
    public Response(HttpStatusCode statusCode, string message, T data)
    {
        StatusCode = (int)statusCode;
        Descriptions.Add(message);
        Data = data;
    }
    public Response(HttpStatusCode statusCode, string message)
    {
        StatusCode = (int)statusCode;
        Descriptions.Add(message);
    }
    public Response(HttpStatusCode statusCode, List<string> message)
    {
        StatusCode = (int)statusCode;
        Descriptions=message;
    }
    
}