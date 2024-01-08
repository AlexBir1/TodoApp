using System;
using System.Collections.Generic;

using TodoAPI.APIResponse.Interfaces;

namespace TodoAPI.APIResponse.Implementations
{
    public class APIResponse<T> : IAPIResponse<T>
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Messages { get; set; }

        public APIResponse(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        public APIResponse(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public APIResponse(bool isSuccess, IEnumerable<string> messages)
        {
            IsSuccess = isSuccess;
            Messages = messages;
        }

        public APIResponse(bool isSuccess, IEnumerable<string> messages, T data)
        {
            IsSuccess = isSuccess;
            Messages = messages;
            Data = data;
        }
    }
}
