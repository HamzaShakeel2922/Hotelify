using ReactApp1.Server.DTOs;
using ReactApp1.Server.DTOs.RoomDTOs;
using ReactApp1.Server.DTOs.RoomTypeDTOs;
using ReactApp1.Server.Database;
using ReactApp1.Server.DTOs.RoomTypeDTOs;
using ReactApp1.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ReactApp1.Server.Services
{
    public class RoomTypeServices
    {
        private readonly HotelDbContext _context;

        public RoomTypeServices(HotelDbContext context)
        {
            _context = context;
        }
        private CustomResponse<T> GenerateResponse<T>(T data, bool isSuccess,string Message, HttpStatusCode StatusCode,bool? isCreated=null,bool? isUpdated=null,bool? isDeleted = null)
        {
            return new CustomResponse<T>(data, Message, StatusCode, isSuccess,isUpdated,isCreated,isDeleted);
        }

        public async Task <CustomResponse<List<RoomTypeDTO>>> GetAllRoomTypes(bool? IsActive)
        {
            try
            {
                var isDataPresent = await _context.RoomTypes.AnyAsync();
                if (!isDataPresent)
                    return  GenerateResponse<List<RoomTypeDTO>>(null, true, "No RoomType Data in Database.",HttpStatusCode.NoContent);
                else { 

                    var query =  _context.RoomTypes.Select(room => new RoomTypeDTO()
                    {
                        RoomTypeID = room.RoomTypeId,
                        IsActive = (bool)room.IsActive,
                        AccessibilityFeatures = room.AccessibilityFeatures,
                        Description = room.Description,
                        TypeName = room.TypeName,
                    });

                    var data = IsActive != null ? query.Where(room => room.IsActive == IsActive).ToList() : query.ToList();
                    return  GenerateResponse<List<RoomTypeDTO>>(data, true, "Rooms Fetched Successfully.", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return  GenerateResponse<List<RoomTypeDTO>>(null, false, ex.Message,HttpStatusCode.InternalServerError);
            }
        }
        public async Task<CustomResponse<RoomTypeDTO>> GetRoomTypeById(int RoomTypeID)
        {
            try
            {
                var isDataPresent = await _context.RoomTypes.AnyAsync();
                if (!isDataPresent)
                    return GenerateResponse<RoomTypeDTO>(null, false, "No RoomType Data in Database.", HttpStatusCode.NoContent);
                else
                {
                    var isValidId = await _context.RoomTypes.AnyAsync(roomtype=>roomtype.RoomTypeId == RoomTypeID);
                    if (!isValidId)
                        return GenerateResponse<RoomTypeDTO>(null, false, "No RoomType against provided Id.", HttpStatusCode.NotFound);
                    else
                    {
                        var roomType = _context.RoomTypes.Where(room => room.RoomTypeId == RoomTypeID).Select(room => new RoomTypeDTO()
                        {
                            RoomTypeID = room.RoomTypeId,
                            IsActive = (bool)room.IsActive,
                            AccessibilityFeatures = room.AccessibilityFeatures,
                            Description = room.Description,
                            TypeName = room.TypeName,
                        }).FirstOrDefault();
                        return GenerateResponse<RoomTypeDTO>(roomType,true, "RoomType Fetched Successfully.",HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return GenerateResponse<RoomTypeDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CustomResponse<UpdateRoomTypeResponseDTO>> UpdateRoomTypeById(int RoomTypeID, UpdateRoomTypeDTO request)
        {
           UpdateRoomTypeResponseDTO updateRoomTypeResponseDTO = new UpdateRoomTypeResponseDTO();
            try
            {
                if (RoomTypeID != request.RoomTypeID)
                    return GenerateResponse<UpdateRoomTypeResponseDTO>(null, false, "RoomType Id mismatch", HttpStatusCode.Conflict, isUpdated: false);

                if (!_context.RoomTypes.Any(roomType => roomType.RoomTypeId == RoomTypeID))
                    return GenerateResponse<UpdateRoomTypeResponseDTO>(null, false, "Invalid RoomType Id.", HttpStatusCode.NotFound, isUpdated: false);

                if (_context.RoomTypes.Any(roomType => roomType.RoomTypeId != RoomTypeID && roomType.TypeName == request.TypeName))
                    return GenerateResponse<UpdateRoomTypeResponseDTO>(null, false, "Another RoomType Already exists with the same name.", HttpStatusCode.Conflict, isUpdated: false);

                var roomType = _context.RoomTypes.FirstOrDefault(room => room.RoomTypeId == RoomTypeID);
                if (roomType != null)
                {
                    roomType.RoomTypeId = request.RoomTypeID;
                    roomType.TypeName = request.TypeName;
                    roomType.AccessibilityFeatures = request.AccessibilityFeatures;
                    roomType.Description = request.Description;
                }
                await _context.SaveChangesAsync();
                return GenerateResponse<UpdateRoomTypeResponseDTO>(null, true, "RoomType Updated Successfully.", HttpStatusCode.OK, isUpdated: true);
            }
            catch (Exception ex)
            {
                return GenerateResponse<UpdateRoomTypeResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isUpdated: false);
            }
        }

        public async Task<CustomResponse<DeleteRoomTypeResponseDTO>> DeleteRoomTypeById(int RoomTypeID)
        {
            DeleteRoomTypeResponseDTO response = new DeleteRoomTypeResponseDTO();
            try
            {
                var RoomType = _context.RoomTypes.Include(rt => rt.Rooms).Where(RoomType => RoomType.RoomTypeId == RoomTypeID).FirstOrDefault();
         
                if (RoomType != null)
                {
                    foreach (var room in RoomType.Rooms)
                    {
                        room.RoomTypeId = null;
                        _context.Rooms.Update(room);
                    }

                    _context.RoomTypes.Remove(RoomType);
                    response.RoomTypeId = RoomTypeID;
                    await _context.SaveChangesAsync();
                    return GenerateResponse<DeleteRoomTypeResponseDTO>(response,true, "RoomType Deleted Successfully.",HttpStatusCode.OK,isDeleted:true);
                }

                else 
                {
                    return GenerateResponse<DeleteRoomTypeResponseDTO>(null, false, "No RoomType Against this ID exists.", HttpStatusCode.NotFound, isDeleted: false);
                }
            }
                catch (Exception ex)
                {
                    return GenerateResponse<DeleteRoomTypeResponseDTO>(null, false, ex.Message, HttpStatusCode.InternalServerError, isDeleted: false);
                }


        }

        public async Task<CustomResponse<CreateRoomTypeResponseDTO>> CreateRoomTypeAsync(CreateRoomTypeDTO request)
        {
            CreateRoomTypeResponseDTO response = new CreateRoomTypeResponseDTO();
            if(_context.RoomTypes.Any(roomType=>roomType.TypeName == request.TypeName))
            {
                response.RoomTypeId = -1;
                return GenerateResponse(response, false, "Another Room Type with the same type name already exists.", HttpStatusCode.Conflict,false);
            }
            else
            {
                try
                {
                    var entity = new RoomType()
                    {
                        TypeName = request.TypeName,
                        Description = request.Description,
                        AccessibilityFeatures = request.AccessibilityFeatures,
                        CreatedBy = "system",
                    };

                    _context.RoomTypes.Add(entity);
                    await _context.SaveChangesAsync();

                    response.RoomTypeId = entity.RoomTypeId;
                    return GenerateResponse(response, true, "RoomType Added Successfully.", HttpStatusCode.OK, true);
                }
                catch (Exception ex)
                {
                    response.RoomTypeId = -1;
                    return GenerateResponse(response, false, ex.Message, HttpStatusCode.InternalServerError, false);
                }
            }

        }

    }

}
