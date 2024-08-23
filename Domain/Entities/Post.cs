using Domain.Core.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public string? Photo { get; set; }
        public bool IsApproved { get; set; } = false;
        public int Likes { get; set; }


        //public string UserId { get; set; }
        //public User User { get; set; }

        public List<PostCategory> PostCategories { get; set; }
        public List<PostTag> PostTags { get; set; }
        public List<PostComment> PostComments { get; set; }



        //public List<Complain> Complains { get; set; }
        //public List<PostLike> PostLikes { get; set; }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Post other)
        //    {
        //        return this.Id == other.Id;
        //    }
        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    return this.Id.GetHashCode();
        //}

    }
}
