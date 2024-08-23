using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class CommentDto : BaseDto
    {
        public string Content { get; set; }
        public bool IsApproved { get; set; }
        public List<PostComment> PostComments { get; set; }
    }
}
