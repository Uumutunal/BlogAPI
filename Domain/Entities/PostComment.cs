using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PostComment : BaseEntity
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
