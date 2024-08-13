using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.DTOs
{
    public class UserRoleDTOs
    {
        [Required(ErrorMessage = "User Id is Required")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Role Id is Required")]
        public int RoleID { get; set; }
    }
}
