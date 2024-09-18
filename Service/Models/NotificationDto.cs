using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class NotificationDto : BaseDto
    {
        
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
