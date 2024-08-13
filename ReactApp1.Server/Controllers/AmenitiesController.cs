using ReactApp1.Server.DTOs.AmenitiesDTOs;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly AmenitiesService _amenitiesService;

        public AmenitiesController(AmenitiesService amenitiesService)
        {
            _amenitiesService = amenitiesService;
        }

        [HttpGet("FetchAllAmenities")]
        public async Task<IActionResult> FetchAllAmenities() 
        { 
            var response = await _amenitiesService.FetchAmenitiesAsync();
            if(response._isSuccees == true)
            {
                return Ok(new APIResponse<List<AmenityDetailsDTO>>(response._Data, response._Message, response._StatusCode));
            }
            else
            {
                switch (response._StatusCode)
                {
                    case HttpStatusCode.NoContent: 
                        return StatusCode((int)response._StatusCode,new APIResponse<List<AmenityDetailsDTO>>(response._Data,response._Message));
                    default: 
                        return StatusCode((int)response._StatusCode,new APIResponse<List<AmenityDetailsDTO>>(response._StatusCode,response._Message));
                }
            }
        }
    }
}
