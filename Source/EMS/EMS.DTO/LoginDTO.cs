﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class LoginDTO
    {
        public string LoginName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }  
    }
}
