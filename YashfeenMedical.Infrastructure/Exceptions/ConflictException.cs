using System.Net;

namespace YashfeenMedical.Infrastructure.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message,HttpStatusCode.Conflict)
        {
        }
    }
}
