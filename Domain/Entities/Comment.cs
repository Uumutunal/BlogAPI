using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public bool IsApproved { get; set; }
        public List<PostComment> PostComments { get; set; }


    }
}
    