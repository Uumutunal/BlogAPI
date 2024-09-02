
﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostCategoryDto 
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public UserDto User { get; set; }
        public string UserId { get; set; }
    }
}
