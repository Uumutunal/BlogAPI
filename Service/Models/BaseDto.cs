﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class BaseDto
    {
        public Guid Id { get; set; } 
        public DateTime? CreatedDate { get; set; } 

    }
}
