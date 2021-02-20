using DemoBlog.Services.Features.Blogs;
using Flurl.Http;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoBlog.Integration.Tests.Api.Blogs
{
    [Collection("Web")]
    public class Blogs_Tests : IAsyncLifetime
    {
        private WebFixture _fixture;
        public Blogs_Tests(WebFixture fixture)
        {
            _fixture = fixture;
        }
        public Task DisposeAsync()
        {
            return Task.FromResult(0);
        }

        public async Task InitializeAsync()
        {
            await _fixture.InitializeAsync();
        }

        [Fact(DisplayName = "Should create new blog")]
        public async Task CreateBlog_Success_ResultAsync()
        {
            //Arrange
            var request = new CreateBlogPost.Request
            {
                Title = "Dummy Title",
                Text = "Dummy Text"
            };
            //Act
            var createBlog = await $"{_fixture.BaseUrl}/api/blogs/create"
                .PostJsonAsync(request).ReceiveJson<CreateBlogPost.Response>();

            //Assert
            createBlog.Id.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Should get blog by id")]
        public async Task QueryBlog_ResultAsync()
        {
            //Arrange
            var createBlog = await $"{_fixture.BaseUrl}/api/blogs/create"
                .PostJsonAsync(new CreateBlogPost.Request
                {
                    Title = "Dummy Title",
                    Text = "Dummy Text"
                }).ReceiveJson<CreateBlogPost.Response>();

            var query = new QueryBlogById.Request() { Id = createBlog.Id };
            //Act
            var queryBlog = await $"{_fixture.BaseUrl}/api/blogs/{createBlog.Id}"
               .GetJsonAsync<QueryBlogById.Response>();
            //Assert
            queryBlog.Id.ShouldBe(createBlog.Id);
        }
    }
}
