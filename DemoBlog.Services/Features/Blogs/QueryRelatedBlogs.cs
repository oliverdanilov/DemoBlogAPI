﻿using DemoBlog.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBlog.Services.Features.Blogs
{
    public class QueryRelatedBlogs
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();

            public class Item
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public string Text { get; set; }
                public DateTime CreatedOn { get; set; }
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
                var post = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == request.Id && !x.DeletedOn.HasValue);
                var relatedIds = post.RelatedBlogs.Select(x => int.Parse(x));
                var result = _context.Blogs.Where(x => !x.DeletedOn.HasValue && relatedIds.Contains(x.Id)).OrderByDescending(x => x.CreatedOn)
                    .Select(c => new Response.Item
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Text = c.Text,
                        CreatedOn = c.CreatedOn
                    });

                return await Task.FromResult(new Response { Items = result });
            }
        }
    }
}