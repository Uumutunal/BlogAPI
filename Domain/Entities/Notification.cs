using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public User User { get; set; }
        public string UserId { get; set; } 
        public bool IsRead { get; set; } = false; 
    }
}
