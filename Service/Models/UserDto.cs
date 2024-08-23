using Domain.Core.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class UserDto : IdentityUser, IAuditableEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Photo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<PostComment> PostComments { get; set; }
    }
}
