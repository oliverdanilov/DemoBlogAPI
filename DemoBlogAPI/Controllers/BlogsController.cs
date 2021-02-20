using DemoBlog.Services.Features.Blogs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<QueryBlogById.Response> QueryBlogById([FromRoute] int id) =>
          await _mediator.Send(new QueryBlogById.Request { Id = id });

        [HttpGet]
        public async Task<QueryBlogs.Response> QueryBlogs([FromQuery] QueryBlogs.Request request) =>
           await _mediator.Send(request ?? new QueryBlogs.Request());

        [HttpPost("create")]
        public async Task<CreateBlogPost.Response> CreateBlog([FromBody] CreateBlogPost.Request request) =>
            await _mediator.Send(request);

        [HttpPut("update/{id}")]
        public async Task<UpdateBlogPost.Response> UpdateBlog([FromRoute] int id, [FromBody] UpdateBlogPost.Request request)
        {
            request = request ?? new UpdateBlogPost.Request();
            request.Id = id;
            return await _mediator.Send(request);
        }

        [HttpDelete("delete/{id}")]
        public async Task<DeleteBlog.Response> DeleteBlog([FromRoute] int id) =>
            await _mediator.Send(new DeleteBlog.Request { Id = id });

        [HttpPost("related/create")]
        public async Task<CreateOrUpdateRelatedBlog.Response> SaveRelatedBlogs([FromBody] CreateOrUpdateRelatedBlog.Request request) =>
            await _mediator.Send(request);

        [HttpDelete("delete/related")]
        public async Task<DeleteRelatedBlog.Response> DeleteRelatedBlog([FromBody] DeleteRelatedBlog.Request request) =>
            await _mediator.Send(request);

        [HttpGet("related")]
        public async Task<QueryRelatedBlogs.Response> QueryRelatedBlogs([FromQuery] QueryRelatedBlogs.Request request) =>
           await _mediator.Send(request ?? new QueryRelatedBlogs.Request());
    }
}
