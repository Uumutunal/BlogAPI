﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public  class UpdateRoleRequest
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
