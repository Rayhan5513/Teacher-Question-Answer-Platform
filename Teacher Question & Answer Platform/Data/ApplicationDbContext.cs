﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using Teacher_Question___Answer_Platform.Database;

namespace Teacher_Question___Answer_Platform.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Train> Trains { get; set; }
        public DbSet<UserTable> UserTables { get; set; }
    }
}
