using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostDto : BaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Photo { get; set; }
        public bool IsApproved { get; set; } = false;
        public int Likes { get; set; }
        public List<PostCategory> PostCategories { get; set; }
        public List<PostTag> PostTags { get; set; }
        public List<PostComment> PostComments { get; set; }
    }
}
