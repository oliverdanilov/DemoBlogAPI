using DemoBlog.Core;
using DemoBlog.Core.Blogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlog.Services
{
    public static class BlogHelpers
    {
        public static async Task<Blog> GetBlog(BlogContext context, int id)
        {
            var post = await context.Blogs.FirstOrDefaultAsync(x => x.Id == id && !x.DeletedOn.HasValue);
            if (post == null)
                throw new ArgumentNullException();

            return post;
        }
    }
}
