using System.Net;

namespace YashfeenMedical.Infrastructure.Exceptions
{
    public class UnprocessableEntityException : AppException
    {
        public UnprocessableEntityException(string message) : base(message, HttpStatusCode.UnprocessableEntity)
        {
        }
    }
}
