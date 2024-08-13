using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.DTOs.RoomTypeDTOs
{
    public class UpdateRoomTypeDTO
    {
        [Required]
        public int RoomTypeID { get; set; }
        [Required]
        public string TypeName { get; set; }
        public string AccessibilityFeatures { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
