using DemoBlog.Core;
using DemoBlog.Core.Blogs;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBlog.Services.Features.Blogs
{
    public class CreateBlogPost
    {
        public class Request : IRequest<Response>
        {
            public string Title { get; set; }
            public string Text { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Text).NotEmpty();
            }
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
                Blog post = Blog.CreateBlog(request.Title, request.Text);
                await _context.AddAsync(post);
                await _context.SaveChangesAsync();

                return await Task.FromResult(new Response { Id = post.Id, Title = post.Title, Text = post.Text });
            }
        }
    }
}