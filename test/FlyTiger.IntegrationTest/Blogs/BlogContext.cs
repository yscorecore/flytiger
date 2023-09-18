using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FlyTiger.IntegrationTest.Blogs
{
    public static class Helpers
    {
        public static void RecreateCleanDatabase()
        {
            using var context = new BlogsContext(quiet: true);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void PopulateDatabase()
        {
            using var context = new BlogsContext(quiet: true);

            var person = new Person { Name = "ajcvickers" };

            context.Add(
                new Blog
                {
                    Owner = person,
                    Name = ".NET Blog",
                    Posts =
                    {
                    new Post
                    {
                        Title = "title1",
                        Content = "content1",
                        Author = person
                    },
                    new Post
                    {
                        Title = "title3",
                        Content = "content3",
                        Author = person
                    },
                     new Post
                    {
                        Title = "title2",
                        Content = "content2",
                        Author = person
                    },
                    }
                });

            context.SaveChanges();
        }
    }


    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Post> Posts { get; } = new List<Post>();

        public int OwnerId { get; set; }
        public Person Owner { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int AuthorId { get; set; }
        public Person Author { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Post> Posts { get; } = new List<Post>();

        public Blog OwnedBlog { get; set; }
    }

    public class BlogsContext : DbContext
    {
        private readonly bool _quiet;

        public BlogsContext(bool quiet = false)
        {
            _quiet = quiet;
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Person> People { get; set; }

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Blog>()
                .HasOne(e => e.Owner)
                .WithOne(e => e.OwnedBlog)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlite("DataSource=test.db");

            if (!_quiet)
            {
                optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
            }
        }
    }
}
