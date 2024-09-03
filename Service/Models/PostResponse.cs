using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostResponse
    {
        public PostDto Post { get; set; }
        public CommentDto Comment { get; set; }
        public UserDto User { get; set; }
        public CategoryDto Category { get; set; }
    }   
}
    