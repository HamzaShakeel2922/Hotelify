using ReactApp1.Server.DTOs.RoomDTOs;
using ReactApp1.Server.Common;
using ReactApp1.Server.Database;
using ReactApp1.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ReactApp1.Server.DTOs.RoomTypeDTOs;

namespace ReactApp1.Server.Services
{
    public class RoomService
    {
        private readonly HotelDbContext _context;

        public RoomService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponse<List<RoomDetailsResponseDTO>>> GetAllRoomsAsync()
        {
            try
            {
                var rooms = await _context.Rooms.AnyAsync();
                if (!rooms)
                {
                    return CommonMethods.GenerateResponse<List<RoomDetailsResponseDTO>>(null, true, "No Rooms Data Found.", HttpStatusCode.NoContent);
                }
                var Allrooms = await _context.Rooms.Select(r => new RoomDetailsResponseDTO
                {
                    RoomID = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    RoomTypeID = (int)r.RoomTypeId,
                    Price = (decimal)r.Price,
                    BedType = r.BedType,
                    ViewType = r.ViewType,
                    Status = r.Status,
                    IsActive = (bool)r.IsActive,
                }).ToListAsync();

                return CommonMethods.GenerateResponse<List<RoomDetailsResponseDTO>>(Allrooms, true, "All Rooms Fetched Successfully.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return CommonMethods.GenerateResponse<List<RoomDetailsResponseDTO>>(null, false, ex.Message, HttpStatusCode.InternalServerError);
            }

           
        }

        public async Task<CustomResponse<RoomDetailsResponseDTO>> GetRoomById(int RoomID)
        {
            try
            {
                var isDataPresent = await _context.Rooms.AnyAsync();
                if (!isDataPresent)
                    return CommonMethods.GenerateResponse<RoomDetailsResponseDTO>(null, false, "No Rooms Data in Database.", HttpStatusCode.NoContent);
                else
                {
                    var isValidId = await _context.Rooms.AnyAsync(room => room.RoomId == RoomID);
                    if (!isValidId)
                        return CommonMethods.GenerateResponse<RoomDetailsResponseDTO>(null, false, "No Room against provided Id.", HttpStatusCode.NotFound);
                    else
                    {
                        var roomType = _context.Rooms.Where(room => room.RoomId == RoomID).Select(room => new RoomDetailsResponseDTO()
                        {
                            RoomID = room.RoomId ,
                            RoomNumber = room.RoomNumber,
                            RoomTypeID = (int)room.RoomTypeId,
                            Price = (decimal)room.Price,
                            BedType = room.BedType,
                            ViewType = room.ViewType,
                            Status = room.Status,
                            IsActive = (bool)room.IsActive,
                        }).FirstOrDefault();
                        return CommonMethods.GenerateResponse<RoomDetailsResponseDTO>(roomType, true, "Room Fetched Successfully.", HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return CommonMethods.GenerateResponse<RoomDetailsResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CustomResponse<CreateRoomResponseDTO>> CreateRoomAsync(CreateRoomRequestDTO request)
        {
            CreateRoomResponseDTO response = new CreateRoomResponseDTO();
            if (_context.Rooms.Any(room => room.RoomNumber == request.RoomNumber))
            {
                response.RoomID = -1;
                return CommonMethods.GenerateResponse<CreateRoomResponseDTO>(response, false, "Another Room with the same Room Number already exists.", HttpStatusCode.Conflict, false);
            }
            else
            {
                try
                {
                    var entity = new Room()
                    {
                       RoomNumber = request.RoomNumber,
                       RoomTypeId = request.RoomTypeID,
                       CreatedBy = "System",
                       CreatedDate = DateTime.Now,
                       Price = request.Price,
                       BedType = request.BedType,
                       ViewType = request.ViewType,
                       IsActive = request.IsActive,
                       Status = request.Status,
                    };

                    _context.Rooms.Add(entity);
                    await _context.SaveChangesAsync();

                    response.RoomID = entity.RoomId;
                    return CommonMethods.GenerateResponse<CreateRoomResponseDTO>(response, true, "RoomType Added Successfully.", HttpStatusCode.OK, true);
                }
                catch (Exception ex)
                {
                    response.RoomID = -1;
                    return CommonMethods.GenerateResponse<CreateRoomResponseDTO>(response, false, ex.Message, HttpStatusCode.InternalServerError, false);
                }
            }

        }


        public async Task<CustomResponse<UpdateRoomResponseDTO>> UpdateRoomByIdAsync(int RoomID, UpdateRoomRequestDTO request)
        {
            UpdateRoomResponseDTO updateRoomTypeResponseDTO = new UpdateRoomResponseDTO();
            try
            {
                if (RoomID != request.RoomID)
                    return CommonMethods.GenerateResponse<UpdateRoomResponseDTO>(null, false, "Room Id mismatch", HttpStatusCode.Conflict, isUpdated: false);
                bool isValidRoom = await _context.Rooms.AnyAsync(room => room.RoomId == RoomID);
                if (!isValidRoom)
                    return CommonMethods.GenerateResponse<UpdateRoomResponseDTO>(null, false, "Invalid Room Id.", HttpStatusCode.NotFound, isUpdated: false);

                bool isNotUnique = await _context.Rooms.AnyAsync(room => room.RoomId != RoomID && room.RoomNumber == request.RoomNumber);
                if (isNotUnique)
                    return CommonMethods.GenerateResponse<UpdateRoomResponseDTO>(null, false, "Another Room Already exists with the same Room number.", HttpStatusCode.Conflict, isUpdated: false);

                var room = _context.Rooms.FirstOrDefault(room => room.RoomId == RoomID);
                if (room != null)
                {
                    room.RoomTypeId = request.RoomTypeID;
                    room.RoomNumber = request.RoomNumber;
                    room.Price = request.Price;
                    room.Status = request.Status;
                    room.ViewType = request.ViewType;
                    room.BedType = request.BedType;
                    room.IsActive = request.IsActive;
                }
                await _context.SaveChangesAsync();
                return CommonMethods.GenerateResponse<UpdateRoomResponseDTO>(null, true, "Room Updated Successfully.", HttpStatusCode.OK, isUpdated: true);
            }
            catch (Exception ex)
            {
                return CommonMethods.GenerateResponse<UpdateRoomResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isUpdated: false);
            }
        }


        public async Task<CustomResponse<DeleteRoomResponseDTO>> DeleteRoomByIdAsync(int RoomID)
        {
            DeleteRoomResponseDTO response = new DeleteRoomResponseDTO();
            try
            {
                var Room = _context.Rooms.Where(room=>room.RoomId == RoomID).FirstOrDefault();  

                if (Room != null)
                {
                    _context.Rooms.Remove(Room);
                    await _context.SaveChangesAsync();
                    response.DeletedRoomId = RoomID;
                    return CommonMethods.GenerateResponse<DeleteRoomResponseDTO>(response, true, "Room Deleted Successfully.", HttpStatusCode.OK, isDeleted: true);
                }

                else
                {
                    return CommonMethods.GenerateResponse<DeleteRoomResponseDTO>(null, false, "No Room Against this ID exists.", HttpStatusCode.NotFound, isDeleted: false);
                }
            }
            catch (Exception ex)
            {
                return CommonMethods.GenerateResponse<DeleteRoomResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isDeleted: false);
            }
        }
    }
}
