

namespace Clinic_System.Application.Common.Bases
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string? Location { get; set; }
        public object Meta { get; set; }
        public object? Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }


        public Response()
        {

        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }
}
