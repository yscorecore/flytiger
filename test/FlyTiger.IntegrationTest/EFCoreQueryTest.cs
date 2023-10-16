using System.Linq.Expressions;
using FluentAssertions;
using FlyTiger.IntegrationTest.Blogs;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using System.Reflection;
using Castle.DynamicProxy.Internal;

namespace FlyTiger.IntegrationTest
{
    [Mapper(typeof(Blog), typeof(BlogDto))]
    [Mapper(typeof(Blog), typeof(BlogSummary))]
    [AutoConstructor]
    public partial class EFCoreQueryTest
    {
        ITestOutputHelper _output;
        [Fact]
        public void ShouldOrderBySubItemWithInclude()
        {
            Helpers.RecreateCleanDatabase();
            InitData_OrderBySubItemWithInclude();
            using var context = new BlogsContext(false);
            var normalPost = context.Blogs.To<BlogDto>().FirstOrDefault();
            var normalTitles = normalPost.Posts.Select(p => p.Title).ToArray();
            _output.WriteLine($"normalIds:[{string.Join(", ", normalTitles)}]");
            normalTitles.Should().BeEquivalentTo(new[] { "title1", "title3", "title2" }, options => options.WithStrictOrdering());


            var orderPosts = context.Blogs.Include(p => p.Posts.OrderBy(p => p.Title)).To<BlogDto>().FirstOrDefault();
            var orderTitles = orderPosts.Posts.Select(p => p.Title).ToArray();
            _output.WriteLine($"orderIds:[{string.Join(", ", orderTitles)}]");
            orderTitles.Should().BeEquivalentTo(new[] { "title1", "title2", "title3" }, options => options.WithStrictOrdering());

            var orderbyDescendingPosts = context.Blogs.Include(p => p.Posts.OrderByDescending(p => p.Title)).To<BlogDto>().FirstOrDefault();
            var orderbyDescendingTitles = orderbyDescendingPosts.Posts.Select(p => p.Title).ToArray();
            _output.WriteLine($"orderIds:[{string.Join(", ", orderbyDescendingTitles)}]");
            orderbyDescendingTitles.Should().BeEquivalentTo(new[] { "title3", "title2", "title1" }, options => options.WithStrictOrdering());

        }

        static void InitData_OrderBySubItemWithInclude()
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

        [Fact]
        public void ShouldWithIncludeFilter()
        {
            Helpers.RecreateCleanDatabase();
            InitData_WithIncludeFilter();
            using var context = new BlogsContext(false);

            var blogsWithNoInclude = context.Blogs
               .Where(p => p.Posts.Any()).To<BlogDto>().ToList();
            blogsWithNoInclude.Should().HaveCount(2);


            var blogsWithIncludeFilter = context.Blogs.Include(p => p.Posts.Where(p => p.Title.Contains("4")))
                .To<BlogDto>().ToList();
            blogsWithIncludeFilter.Should().HaveCount(2);
            blogsWithIncludeFilter.SelectMany(p => p.Posts).Should().HaveCount(1);

        }

        static void InitData_WithIncludeFilter()
        {
            using var context = new BlogsContext(quiet: true);

            var person = new Person { Name = "ajcvickers" };

            context.Blogs.AddRange(
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
                },
                new Blog
                {
                    Owner = person,
                    Name = "Java Blog",
                    Posts =
                    {
                    new Post
                    {
                        Title = "title4",
                        Content = "content4",
                        Author = person
                    },
                    new Post
                    {
                        Title = "title5",
                        Content = "content5",
                        Author = person
                    }
                    }
                });

            context.SaveChanges();
        }


        [Fact]
        public void ShouldMapListCountWithInclude()
        {
            Helpers.RecreateCleanDatabase();
            InitData_WithIncludeFilter();
            using var context = new BlogsContext(false);

            var blogsWithNoInclude = context.Blogs
               .Where(p => p.Posts.Any()).To<BlogSummary>().ToList();
            blogsWithNoInclude.Should().BeEquivalentTo(new BlogSummary[]
            {
                new BlogSummary{ Name = ".NET Blog", PostsCount = 3 },
                new BlogSummary{ Name = "Java Blog", PostsCount = 2 }
            });


            var blogsWithIncludeFilter = context.Blogs.Include(p => p.Posts.Where(p => p.Title.Contains("4")))
                .To<BlogSummary>().ToList();
            blogsWithIncludeFilter.Should().BeEquivalentTo(new BlogSummary[]
            {
                new BlogSummary{ Name = ".NET Blog", PostsCount = 0 },
                new BlogSummary{ Name = "Java Blog", PostsCount = 1 }
            });
        }

        internal class BlogSummary
        {
            public string Name { get; set; }
            public int PostsCount { get; set; }
        }

    }
    public record BlogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<PostDto> Posts { get; set; }

        public int OwnerId { get; set; }
    }

    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }

        public int AuthorId { get; set; }
    }
}
