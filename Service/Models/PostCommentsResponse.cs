using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostCommentsResponse
    {
        public List<PostDto> Posts { get; set; }
        public List<UserDto> Users { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
