﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
