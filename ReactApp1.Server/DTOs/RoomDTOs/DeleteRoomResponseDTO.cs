namespace ReactApp1.Server.DTOs.RoomDTOs
{
    public class DeleteRoomResponseDTO
    {
        public string Message { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedRoomId { get; set; }
    }
}
