using FluentAssertions;
using FlyTiger.IntegrationTest.Blogs;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace FlyTiger.IntegrationTest
{
    [Mapper(typeof(Blog), typeof(BlogDto))]
    [AutoConstructor]
    public partial class EFCoreQueryTest
    {
        ITestOutputHelper _output;
        [Fact]
        public void ShouldOrderBySubItem()
        {
            Helpers.RecreateCleanDatabase();
            Helpers.PopulateDatabase();
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
