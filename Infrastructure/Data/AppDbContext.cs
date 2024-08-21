﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

    }
}
