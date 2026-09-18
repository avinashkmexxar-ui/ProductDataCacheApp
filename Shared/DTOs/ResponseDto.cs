using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public sealed class ResponseDto<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public int Code { get; private set; }
        public T? Data { get; private set; }
        public static ResponseDto<T> Success(T data, string message = "Successfully retrieved") => new()
        {
            IsSuccess = true,
            Code = 200,
            Message = message,
            Data = data
        };

        public static ResponseDto<T> Fail(int code, string message) => new()
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };
    }
}
