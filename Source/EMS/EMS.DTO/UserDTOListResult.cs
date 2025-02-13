using EMS.DataAccess.Models;
using System;

namespace EMS.DTO
{
    public class UserDTOListResult
    {
        public object RoleDTO;

        public int ResultCode { get; set; }

        public string ResultDescription { get; set; } = string.Empty;

        public List<UserDTO> UserDTOs { get; set; } = new List<UserDTO>();

        public UserDTO UserDTO { get; set; } = new UserDTO();
    }
}
