
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ReactApp1.Server.DTOs.UserDTOs;
using ReactApp1.Server.DTOs;
using Microsoft.AspNetCore.Cors;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserServices _userService;

        public UserController(UserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTOs request)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.AddUserAsync(request);
                if (response._isCreated == true)
                    return Ok(new APIResponse<CreateUserResponseDTOs>(response._Data,response._Message,response._StatusCode));

                    switch (response._StatusCode)
                    {
                        case HttpStatusCode.Conflict:
                            return Conflict(new APIResponse<CreateUserResponseDTOs>(HttpStatusCode.Conflict, response._Message));
                        default:
                            return StatusCode((int)HttpStatusCode.InternalServerError,new APIResponse<CreateUserResponseDTOs>(HttpStatusCode.InternalServerError, response._Message));
                    }
            }
            else
            {
                return UnprocessableEntity(new APIResponse<CreateUserResponseDTOs>(HttpStatusCode.UnprocessableEntity, "Invalid Data in Request Body"));
            }
        }

        [HttpGet("FetchAllUsers")]
        public async Task<IActionResult> FetchAllUsers(bool? isActive = null)
        {
            var response = await _userService.GetAllUsersAsync(isActive);
            if (response._isSuccees == true)
            {
                return  Ok(new APIResponse<List<UserResponseDTOs>>(response._Data,response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)response._StatusCode, response._Message);
                    default:
                      return StatusCode((int)response._StatusCode, response._Message);
                }
            }
        }

        [HttpGet("GetUserById/{UserID}")]
        public async Task<IActionResult> GetUserById(int UserID)
        {
            var response = await _userService.GetUserByIdAsync(UserID);
            if(response._isSuccees == true)
                return Ok(new APIResponse<UserResponseDTOs>(response._Data, response._Message, response._StatusCode));
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return StatusCode((int)HttpStatusCode.NotFound,new APIResponse<UserResponseDTOs>(response._Data,response._Message));
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<UserResponseDTOs>(response._Data, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<UserResponseDTOs>(response._Data, response._Message));
                }
            }
        }

        [HttpPut("UpdateUser/{UserID}")]
        public async Task<IActionResult> UpdateUser(int UserID,[FromBody] UpdateUserDTOs request)
        {
            var response = await _userService.UpdateUserAsync(UserID, request);
            if (response._isUpdated == true) return Ok(new APIResponse<UpdateUserResponseDTOs>(response._Data, response._Message));
            else
            {
                switch(response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<UpdateUserResponseDTOs>(response._StatusCode, response._Message));
                    case HttpStatusCode.Conflict:
                        return Conflict(new APIResponse<UpdateUserResponseDTOs>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<UpdateUserResponseDTOs>(response._StatusCode, response._Message));
                }
            }
        }

        [HttpDelete("DeleteUser/{UserID}")]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            var response = await _userService.DeleteUser(UserID);
            if (response._isDeleted == true) return Ok(new APIResponse<DeleteUserResponseDTOs>(response._Data, response._Message));
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<DeleteUserResponseDTOs>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<DeleteUserResponseDTOs>(response._StatusCode, response._Message));
                }
            }
        }

        [HttpPut("AssignRole")]
        public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleRequestDTO request)
        {
           var response = await _userService.AssignUserRoleAsync(request);
            if(response._isSuccees == true)
            {
                return Ok(new APIResponse<AssignUserRoleResponseDTO>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<AssignUserRoleResponseDTO>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<AssignUserRoleResponseDTO>(response._StatusCode, response._Message));
                }
            }

        }

        [HttpPost("ToggleActiveStatus/{UserId}")]

        public async Task<IActionResult> ToggleActiveStatus(int UserId)
        {
            var response = await _userService.ToggleUserActiveAsync(UserId);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<ToggleUserActiveResponseDTO>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(new APIResponse<ToggleUserActiveResponseDTO>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<ToggleUserActiveResponseDTO>(response._StatusCode, response._Message));
                }
            }

        }

        [HttpPost("LoginUser")]

        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserDTOs request )
        {
            var response = await _userService.LoginUserAsync(request);
            if (response._isSuccees == true)
            {
                return Ok(new APIResponse<LoginUserResponseDTOs>(response._Data, response._Message));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        return  StatusCode((int)HttpStatusCode.Forbidden,new APIResponse<LoginUserResponseDTOs>(response._StatusCode, response._Message));
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse<LoginUserResponseDTOs>(response._StatusCode, response._Message));
                    default:
                        return StatusCode((int)response._StatusCode, new APIResponse<LoginUserResponseDTOs>(response._StatusCode, response._Message));
                }
            }
        }

        [HttpGet("GetCustomerDetailsByReservation")]

        public async Task<IActionResult> GetCustomerDetailsByReservation(int GuestId = 0,int ReservationId = 0)
        {
            var response = await _userService.GetCustomerDetailsByReservation(GuestId,ReservationId);
            return Ok(new APIResponse<dynamic>(response._Data, response._Message));
        }

    }
}
