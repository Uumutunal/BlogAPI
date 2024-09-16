using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class FavoritePostDto : BaseDto
    {
        public string UserId { get; set; }
        public UserDto User { get; set; }
        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
    }
}
