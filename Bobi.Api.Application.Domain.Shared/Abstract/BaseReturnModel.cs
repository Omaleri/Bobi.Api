using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Domain.Shared.Abstract
{
    public class BaseReturnModel<T>
    {
        public List<ErrorModel> Error { get; set; } = new List<ErrorModel>();
        public bool IsSuccess { get { return !Error.Any(); } }
        public T Data { get; set; }
    }

    public class ErrorModel
    {
        public ErrorModel(ErrorCodes errorCode, string message = "default")
        {
            Code = (int)errorCode;
            if (message != "default")
                Message = message;
            else
                Message = errorCode.ToString();

        }
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public enum ErrorCodes
    {
        DataNotFound = 404,
        DataAlreadyExists = 409,
        DataNotValid = 400,
        ProcessNotCompleted = 500,
        ProcessNotAuthorized = 401,
        InvalidData = 501
    }
}
