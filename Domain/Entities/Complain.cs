﻿using Domain.Core.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Complain : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        //public string UserId { get; set; }
        //public User User { get; set; }
        //public Guid PostId { get; set; }
        //public Post Post { get; set; }
    }
}
