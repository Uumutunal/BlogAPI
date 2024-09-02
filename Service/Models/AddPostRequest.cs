using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class AddPostRequest
    {
        public PostDto Post { get; set; }
        public List<Guid> CategoryIds { get; set; } = null;
        public string UserId { get; set; }
    }
}
