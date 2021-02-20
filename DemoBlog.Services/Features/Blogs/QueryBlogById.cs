using DemoBlog.Core;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBlog.Services.Features.Blogs
{
    public class QueryBlogById
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly BlogContext _context;

            public Handler(BlogContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var post = await BlogHelpers.GetBlog(_context, request.Id);
                var response = new Response
                {
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text,
                    CreatedOn = post.CreatedOn
                };

                return await Task.FromResult(response);
            }
        }
    }
}