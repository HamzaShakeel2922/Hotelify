using ReactApp1.Server.DTOs.RoomDTOs;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ReactApp1.Server.DTOs.RoomTypeDTOs;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var response = await _roomService.GetAllRoomsAsync();
            if(response._isSuccees == true)
            {
                return Ok(new APIResponse<List<RoomDetailsResponseDTO>>(response._Data,response._Message,response._StatusCode));
            }
            else
            {
                switch(response._StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return StatusCode((int)response._StatusCode, new APIResponse<List<RoomDetailsResponseDTO>>(response._Data, response._Message, response._StatusCode));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<List<RoomDetailsResponseDTO>>(response._Data, response._Message, response._StatusCode));
                }
            }
        }

        [HttpGet("GetRoomById/{RoomID}")]
        public async Task<IActionResult> GetRoomTypeById(int RoomID)
        {
            var response = await _roomService.GetRoomById(RoomID);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<RoomDetailsResponseDTO>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<RoomDetailsResponseDTO>(response._Data, response._Message, response._StatusCode));
                    case HttpStatusCode.NoContent:
                        return StatusCode((int)response._StatusCode, new APIResponse<RoomDetailsResponseDTO>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<object>(response._StatusCode, response._Message));
                }
            }
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom(CreateRoomRequestDTO request)
        {
            var response = await _roomService.CreateRoomAsync(request);
            if (response._isCreated == true)
                return Ok(new APIResponse<CreateRoomResponseDTO>(response._Data, response._Message));
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        return Conflict(new APIResponse<CreateRoomTypeResponseDTO>(HttpStatusCode.Conflict, response._Message));
                    default:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<CreateRoomTypeResponseDTO>(HttpStatusCode.InternalServerError, response._Message));
                }
            }
        }

        [HttpPut("UpdateRoomById/{RoomID}")]
        public async Task<IActionResult> UpdateRoomById(int RoomID, [FromBody] UpdateRoomRequestDTO request)
        {
            var response = await _roomService.UpdateRoomByIdAsync(RoomID, request);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<UpdateRoomResponseDTO>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return StatusCode((int)HttpStatusCode.NoContent, new APIResponse<UpdateRoomResponseDTO>(response._Data, response._Message, HttpStatusCode.NoContent));
                    case HttpStatusCode.Conflict:
                        return StatusCode((int)HttpStatusCode.Conflict, new APIResponse<UpdateRoomResponseDTO>(response._Data, response._Message, HttpStatusCode.NoContent));
                    default:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<UpdateRoomResponseDTO>(response._StatusCode , response._Message));
                }
            }
        }

        [HttpDelete("DeleteRoomById/{RoomId}")]
        public async Task<IActionResult> DeleteRoomById(int RoomId)
        {
            var response = await _roomService.DeleteRoomByIdAsync(RoomId);
            if(response._isSuccees == true)
                return Ok(new APIResponse<DeleteRoomResponseDTO>(response._Data,response._Message,response._StatusCode));
            else
            {
                switch(response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<DeleteRoomResponseDTO>(response._StatusCode,response._Message));
                    default:
                        return StatusCode((int)response._StatusCode,new APIResponse<DeleteRoomResponseDTO>(response._StatusCode, response._Message));
                }
            }
        }
    }
}
