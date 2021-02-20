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
    public class DeleteBlog
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
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

                post.Delete();
                _context.Blogs.Update(post);
                await _context.SaveChangesAsync();

                return await Task.FromResult(new Response { Id = post.Id });
            }
        }
    }
}