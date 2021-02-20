using DemoBlog.Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBlog.Services.Features.Blogs
{
    public class CreateOrUpdateRelatedBlog
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }

            public int RelatedPostId { get; set; }
        }

        public class Response
        {
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.RelatedPostId).NotEmpty();
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
                var post = await BlogHelpers.GetBlog(_context, request.Id);
                var relatedPost = await BlogHelpers.GetBlog(_context, request.RelatedPostId);

                post.AddRelated(relatedPost);
                relatedPost.AddRelated(post);

                _context.Update(post);
                _context.Update(relatedPost);
                await _context.SaveChangesAsync();

                return await Task.FromResult(new Response());
            }
        }
    }
}