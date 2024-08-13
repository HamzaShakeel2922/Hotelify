using ReactApp1.Server.DTOs.RoomTypeDTOs;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly RoomTypeServices _roomTypeServices;
        public RoomTypeController(RoomTypeServices roomTypeService)
        {
            _roomTypeServices = roomTypeService;
        }

        [HttpGet("GetAllRoomTypes")]
        public async Task<IActionResult> GetAllRoomTypes(bool? IsActive = null)
        {
            var response = await _roomTypeServices.GetAllRoomTypes(IsActive);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<List<RoomTypeDTO>>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return StatusCode((int)HttpStatusCode.NoContent,new APIResponse<List<RoomTypeDTO>>(response._Data,response._Message,HttpStatusCode.NoContent));
                    default:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<List<RoomTypeDTO>>(HttpStatusCode.InternalServerError,response._Message));
                }
            }
        }

        [HttpGet("GetRoomTypeById/{RoomTypeID}")]
        public async Task<IActionResult> GetRoomTypeById(int RoomTypeID)
        {
            var response = await _roomTypeServices.GetRoomTypeById(RoomTypeID);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<RoomTypeDTO>((RoomTypeDTO)response._Data,response._Message));
            }
            else
            {
                switch(response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                            return NotFound(new APIResponse<RoomTypeDTO>(response._Data, response._Message,response._StatusCode));
                    case HttpStatusCode.NoContent:
                            return StatusCode((int)response._StatusCode,new APIResponse<RoomTypeDTO>(response._StatusCode,response._Message));
                    default :
                            return StatusCode((int)response._StatusCode, new APIResponse<object>(response._StatusCode, response._Message));
                }
            }
        }

        [HttpPost("CreateRoomType")]
        public async Task<IActionResult> CreateRoomType(CreateRoomTypeDTO request)
        {
            var response = await _roomTypeServices.CreateRoomTypeAsync(request);
            if (response._isCreated == true)
                return Ok(new APIResponse<CreateRoomTypeResponseDTO>(response._Data,response._Message));
            else 
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.Conflict:
                            return Conflict(new APIResponse<CreateRoomTypeResponseDTO>(HttpStatusCode.Conflict, response._Message));    
                    default:
                           return StatusCode((int)HttpStatusCode.InternalServerError,new APIResponse<CreateRoomTypeResponseDTO>(HttpStatusCode.InternalServerError, response._Message));   
                }
            }
        }

        [HttpPut("UpdateRoomTypeById/{RoomTypeID}")]
        public async Task<IActionResult> UpdateRoomTypeById(int RoomTypeID, [FromBody] UpdateRoomTypeDTO request)
        {
            var response = await _roomTypeServices.UpdateRoomTypeById(RoomTypeID,request);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<UpdateRoomTypeResponseDTO>(response._Data,response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return StatusCode((int)HttpStatusCode.NoContent,new APIResponse<UpdateRoomTypeResponseDTO>(response._Data, response._Message, HttpStatusCode.NoContent));
                    case HttpStatusCode.Conflict:
                        return StatusCode((int)HttpStatusCode.Conflict, new APIResponse<UpdateRoomTypeResponseDTO>(response._Data, response._Message, HttpStatusCode.NoContent));
                    default:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<UpdateRoomTypeResponseDTO>(HttpStatusCode.InternalServerError, response._Message));
                }
            }
        }

        [HttpDelete("DeleteRoomTypeById/{RoomTypeID}")]

        public async Task<IActionResult> DeleteRoomTypeById(int RoomTypeID)
        {
           var response = await _roomTypeServices.DeleteRoomTypeById(RoomTypeID);
            if(response._isDeleted == true)
            {
                return Ok(new APIResponse<DeleteRoomTypeResponseDTO>(response._Data, response._Message,response._StatusCode));
            }
            else
            {
                switch(response._StatusCode)
                {
                    case HttpStatusCode.NotFound :
                        {
                            return StatusCode((int)HttpStatusCode.NotFound, new APIResponse<DeleteRoomTypeResponseDTO>(response._StatusCode,response._Message));
                        }
                     default:
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<DeleteRoomTypeResponseDTO>(response._StatusCode, response._Message));
                        }
                }
            }
        }
    }
}
