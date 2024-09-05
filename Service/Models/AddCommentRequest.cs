using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class AddCommentRequest
    {
        public CommentDto Comment { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
