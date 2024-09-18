using Domain.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class User : IdentityUser, IAuditableEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Photo { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        //public List<Post> Posts { get; set; }

        public List<Notification>? Notifications { get; set; }

        public List<PostComment> PostComments { get; set; }
    }


}

