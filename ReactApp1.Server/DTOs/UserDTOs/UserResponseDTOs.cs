namespace ReactApp1.Server.DTOs
{
    public class UserResponseDTOs
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public int RoleID { get; set; }
    }
}