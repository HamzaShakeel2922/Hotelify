using ReactApp1.Server.DTOs.RoomTypeDTOs;
using Microsoft.AspNetCore.Http;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReactApp1.Server.DTOs.RoomTypeDTOs
{
    public class CustomResponse<T>
    {
        public T _Data { get; set; }
        public bool? _isSuccees { get; set; }
        public bool? _isCreated { get; set; }
        public bool? _isUpdated { get; set; }
        public bool? _isDeleted { get; set; }

        public string _Message { get; set; }
        public HttpStatusCode _StatusCode { get; set; }

        public CustomResponse()
        {
        }
        public CustomResponse( T data, string Message, HttpStatusCode StatusCode, bool? isSuccess = true, bool? isUpdated = null, bool? isCreated = null, bool? isDeleted = null)
        {
            _Data = data;
            _isSuccees = isSuccess;
            _Message = Message;
            _StatusCode = StatusCode;
            _isUpdated = isUpdated;
            _isCreated = isCreated;
            _isDeleted = isDeleted;
        }


    }
}
