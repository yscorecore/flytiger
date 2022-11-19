using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mapper.EFCore
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseSqlite("Filename=testdb");
        }
    }

    public class BaseEntity
    {
        public Guid Id { get; set; }
    }

    public class User : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Address> Address { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class Address : BaseEntity
    {
        public string City { get; set; }
        public User User { get; set; }
    }

    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
