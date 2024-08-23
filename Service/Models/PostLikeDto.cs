using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostLikeDto : BaseDto
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
