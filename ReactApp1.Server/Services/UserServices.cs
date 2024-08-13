using ReactApp1.Server.DTOs;
using ReactApp1.Server.Database;
using ReactApp1.Server.DTOs.RoomTypeDTOs;
using ReactApp1.Server.DTOs.UserDTOs;
using ReactApp1.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace ReactApp1.Server.Services
{
    public class UserServices
    {
        private readonly HotelDbContext _context;

        public UserServices(HotelDbContext context)
        {
            _context = context;
        }

        private CustomResponse<T> GenerateResponse<T>(T data, bool isSuccess, string Message, HttpStatusCode StatusCode, bool? isCreated = null, bool? isUpdated = null, bool? isDeleted = null)
        {
            return new CustomResponse<T>(data, Message, StatusCode, isSuccess, isUpdated, isCreated, isDeleted);
        }

        public async Task<CustomResponse<CreateUserResponseDTOs>> AddUserAsync(CreateUserDTOs request)
        {
            CreateUserResponseDTOs response = new CreateUserResponseDTOs();
            if (_context.Users.Any(u => u.Email == request.Email))
                return GenerateResponse<CreateUserResponseDTOs>(null, false, "A user with the given email already exists.", HttpStatusCode.Conflict, isCreated: false);
            
            var newUser = new User
            {
                RoleId = 1, // Guest User by Default
                Email = request.Email,
                PasswordHash = request.Password,
                CreatedBy = "System",
            };
                try
                {
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    response.UserId = newUser.UserId;
                    return GenerateResponse<CreateUserResponseDTOs>(response, true, "User Created Successfully.", HttpStatusCode.OK, isCreated: true);
                }
                catch (Exception ex)
                {
                    response.UserId = -1;
                    return GenerateResponse<CreateUserResponseDTOs>(response, false, ex.Message, HttpStatusCode.InternalServerError, isCreated: false);
                }
            
        }

        public async Task<CustomResponse<List<UserResponseDTOs>>> GetAllUsersAsync(bool? isActive)
        {
            try
            {
                bool usersExist = await _context.Users.AnyAsync();
                if (!usersExist)
                    return GenerateResponse<List<UserResponseDTOs>> (null,true,"No Users in the Database.",HttpStatusCode.OK);

                var query = _context.Users.Select(user =>
                    new UserResponseDTOs()
                    {
                        UserID = user.UserId,
                        Email = user.Email,
                        IsActive = (bool)user.IsActive,
                        LastLogin = user.LastLogin,
                        RoleID = (int)user.RoleId,
                    });

                var users = isActive !=null ? query.Where(emp=>emp.IsActive == isActive).ToList() : query.ToList();
                if(users.Count == 0)
                 return GenerateResponse<List<UserResponseDTOs>>(null, true, $"There are No Users with IsActive {isActive}.", HttpStatusCode.OK);

                return GenerateResponse<List<UserResponseDTOs>>(users, true, "All Users Fetched.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return GenerateResponse<List<UserResponseDTOs>>(null, false, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CustomResponse<UserResponseDTOs>> GetUserByIdAsync(int UserId)
        {
            try
            {
                var user = await _context.Users.Select(user => new UserResponseDTOs()
                {
                    UserID = user.UserId,
                    Email = user.Email,
                    IsActive = (bool)user.IsActive,
                    LastLogin = user.LastLogin,
                    RoleID = (int)user.RoleId,

                }).FirstOrDefaultAsync(user => user.UserID == UserId);
                if (user != null)
                    return GenerateResponse<UserResponseDTOs>(user, true, "User Fetched Successfully.", HttpStatusCode.OK);
                else
                    return GenerateResponse<UserResponseDTOs>(null, false, "No User Found Against the Id.",HttpStatusCode.NotFound);
            }
                catch (Exception ex)
                {
                    return GenerateResponse<UserResponseDTOs>(null,false,ex.Message,HttpStatusCode.InternalServerError);
                }
        }

        public async Task<CustomResponse<UpdateUserResponseDTOs>> UpdateUserAsync(int UserId,UpdateUserDTOs request)
        {
            UpdateUserResponseDTOs response = new UpdateUserResponseDTOs();
            
            try
            {
                if(UserId != request.UserID) { 
                    return GenerateResponse<UpdateUserResponseDTOs>(null, false,"User Id Mismatch",HttpStatusCode.Conflict,isUpdated:false);
                }                       
                bool isEmailNotUnique = await _context.Users.AnyAsync(user => user.UserId != UserId && user.Email == request.Email);
                if (isEmailNotUnique)
                    return GenerateResponse<UpdateUserResponseDTOs>(null, false, "A User already exists with this Email.", HttpStatusCode.Conflict, isUpdated: false);

                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == UserId);
                if (user != null)
                {
                    user.Email = request.Email;
                    user.PasswordHash = request.Password;
                    user.ModifiedBy = "system";
                    user.ModifiedDate = DateTime.Now;

                    _context.Users.Update(user);
                    response.IsUpdated = true;
                    response.UserId = user.UserId;
                    return GenerateResponse<UpdateUserResponseDTOs>(response, true, "User Updated Successfully.", HttpStatusCode.OK,isUpdated:true);
                }
                else
                    return GenerateResponse<UpdateUserResponseDTOs>(null, false, "No User Found.", HttpStatusCode.NotFound,isUpdated:false);
            }
            catch (Exception ex)
            {
                return GenerateResponse<UpdateUserResponseDTOs>(null, false, ex.Message, HttpStatusCode.InternalServerError,isUpdated:false);
            }

        }
        public async Task<CustomResponse<DeleteUserResponseDTOs>> DeleteUser(int UserID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user=>user.UserId == UserID);
            try
            {
                if (user != null)
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return GenerateResponse<DeleteUserResponseDTOs>(null, true, "User Deleted Successfully.", HttpStatusCode.OK, isDeleted: true);
                }
                else
                {
                    return GenerateResponse<DeleteUserResponseDTOs>(null, false, "No User against this id.", HttpStatusCode.NotFound, isDeleted: false);
                }
            }
            catch (Exception ex)
            {
                return GenerateResponse<DeleteUserResponseDTOs>(null, false, ex.Message, HttpStatusCode.InternalServerError, isDeleted: false);
            }
        }

        public async Task<CustomResponse<ToggleUserActiveResponseDTO>> ToggleUserActiveAsync(int UserId)
        {
            ToggleUserActiveResponseDTO dto = new ToggleUserActiveResponseDTO();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == UserId);
                if (user == null)
                {
                    dto.UserId = -1;
                    return GenerateResponse<ToggleUserActiveResponseDTO>(dto, false, "No User Found with this Id.", HttpStatusCode.NotFound, isUpdated: false);
                }
                else
                {
                    dto.UserId = user.UserId;
                    dto.IsActive = !user.IsActive;
                    user.IsActive = !user.IsActive;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return GenerateResponse<ToggleUserActiveResponseDTO>(dto, true, "Active Status Updated Successfully.", HttpStatusCode.OK, isUpdated: true);
                }
            }
            catch (Exception ex)
            {
                return GenerateResponse<ToggleUserActiveResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isUpdated: false);
            }
        }

        public async Task<CustomResponse<AssignUserRoleResponseDTO>> AssignUserRoleAsync(AssignUserRoleRequestDTO request)
        {
            try
            {
                AssignUserRoleResponseDTO dto = new AssignUserRoleResponseDTO();
                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == request.UserId);
                var role = await _context.UserRoles.FirstOrDefaultAsync(role => role.RoleId == request.RoleId);

                if (user == null || role == null)
                    return GenerateResponse<AssignUserRoleResponseDTO>(null, false, "Invalid User or Role Id.", HttpStatusCode.NotFound, isUpdated: false);

                user.RoleId = role.RoleId;
                dto.UserId = request.UserId;
                dto.RoleId  = role.RoleId;
                dto.RoleName = role.RoleName;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return GenerateResponse<AssignUserRoleResponseDTO>(dto, true, "Role assigned Successfully.", HttpStatusCode.OK, isUpdated: true);
            }
            catch (Exception ex)
            {

                return GenerateResponse<AssignUserRoleResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isUpdated: false);
            }
        }

        public async Task<CustomResponse<LoginUserResponseDTOs>> LoginUserAsync(LoginUserDTOs request)
        {
            try
            {
                LoginUserResponseDTOs dto = new LoginUserResponseDTOs();
                var user = await _context.Users.FirstOrDefaultAsync(user=>user.Email == request.Email && user.PasswordHash == request.Password);
                if(user == null)
                    return GenerateResponse<LoginUserResponseDTOs>(null, false, "Email or password is Incorrect.",HttpStatusCode.Unauthorized);

                bool isUserAccountActive = user.IsActive;
                if(!isUserAccountActive)
                    return GenerateResponse<LoginUserResponseDTOs>(null, false, "Your Account is not Active.", HttpStatusCode.Forbidden);

                user.LastLogin = DateTime.Now;
                _context.Update(user);
                await _context.SaveChangesAsync();

                dto.IsLogin = true;
                dto.UserId = user.UserId;

                return GenerateResponse<LoginUserResponseDTOs>(dto, true, "Login Successfull.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return GenerateResponse<LoginUserResponseDTOs>(null, false, ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        public async Task<CustomResponse<dynamic>> GetCustomerDetailsByReservation(int GuestId, int ReservationId)
        {
            var db = _context;

            var query = await db.Feedbacks
                .Join(db.ReservationGuests,
                    f => f.ReservationId,
                    r => r.ReservationId,
                    (f, r) => new { f, r })
                .Join(db.Guests,
                    fr => fr.f.GuestId,
                    g => g.GuestId,
                    (fr, g) => new { fr.f, fr.r, g })
                .Join(db.Countries,
                    frg => frg.g.CountryId,
                    c => c.CountryId,
                    (frg, c) => new
                    {
                        frg.f.GuestId,
                        frg.g.FirstName,
                        frg.g.LastName,
                        frg.g.CountryId,
                        frg.g.Phone,
                        frg.g.AgeGroup,
                        c.CountryName,
                        c.CountryCode
                    })
                .Where(frgc => frgc.GuestId == GuestId)
                .ToListAsync();

            return GenerateResponse<dynamic>(query, true, "All Details Fetched.", HttpStatusCode.OK);
        }

    }
}
