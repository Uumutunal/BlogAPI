using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class FollowerDto : BaseDto
    {
        public string AuthorId { get; set; }
        public string SubscriberId { get; set; }
    }
}
