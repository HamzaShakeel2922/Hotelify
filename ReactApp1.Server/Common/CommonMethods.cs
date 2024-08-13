using System.Net;
using ReactApp1.Server.DTOs.RoomTypeDTOs;

namespace ReactApp1.Server.Common
{
    public class CommonMethods
    {
        public static CustomResponse<T> GenerateResponse<T>(T data, bool isSuccess, string Message, HttpStatusCode StatusCode, bool? isCreated = null, bool? isUpdated = null, bool? isDeleted = null)
        {
            return new CustomResponse<T>(data, Message, StatusCode, isSuccess, isUpdated, isCreated, isDeleted);
        }
    }
}
