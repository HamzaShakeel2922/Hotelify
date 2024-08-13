namespace ReactApp1.Server.DTOs.RoomTypeDTOs
{
    public class DeleteRoomTypeResponseDTO
    {
        public string Message { get; set; }
        public bool IsDeleted { get; set; }

        public int RoomTypeId { get; set; }
    }
}
