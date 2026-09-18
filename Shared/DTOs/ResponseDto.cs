using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public sealed class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Code { get; set; }
        public T? Data { get; set; }
    }
}
