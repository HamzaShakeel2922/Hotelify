using ReactApp1.Server.DTOs.AmenitiesDTOs;
using ReactApp1.Server.Common;
using ReactApp1.Server.Database;
using ReactApp1.Server.DTOs.RoomTypeDTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ReactApp1.Server.Services
{
    public class AmenitiesService
    {
        private readonly HotelDbContext _context;

        public AmenitiesService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponse<List<AmenityDetailsDTO>>> FetchAmenitiesAsync() 
        {
            try
            {
                var AmenitiesPresent = await _context.Amenities.AnyAsync();
                if (AmenitiesPresent == false)
                {
                    return CommonMethods.GenerateResponse<List<AmenityDetailsDTO>>(null, true, "No Amenities Data in Database.", HttpStatusCode.NoContent);
                }
                var amenities = await _context.Amenities.Select(amenity => new AmenityDetailsDTO()
                {
                    AmenityID = amenity.AmenityId,
                    Name = amenity.Name,
                    IsActive = (bool)amenity.IsActive,
                    Description = amenity.Description,
                }).ToListAsync();

                return CommonMethods.GenerateResponse<List<AmenityDetailsDTO>>(amenities, true, "All Amenities Fetched Successfully.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CommonMethods.GenerateResponse<List<AmenityDetailsDTO>>(null, false, ex.Message,HttpStatusCode.InternalServerError);
            }
        }
    }
}
