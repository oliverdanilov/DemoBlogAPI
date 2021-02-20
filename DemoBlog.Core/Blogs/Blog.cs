using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoBlog.Core.Blogs
{
    public class Blog : BaseEntity
    {
        private Blog(string title, string text)
        {
            Title = title;
            Text = text;
            RelatedBlogs = new List<string>();
        }
        public static Blog CreateBlog(string title, string text)
        {
            return new Blog(title, text);
        }
        public string Title { get; set; }

        public string Text { get; set; }
        public List<string> RelatedBlogs { get; set; }
        public void UpdateBlog(string title, string text)
        {
            Title = title;
            Text = text;
        }
        public void AddRelated(Blog relatedPost)
        {
            if (!RelatedBlogs.Any(x => x == relatedPost.Id.ToString()))
                RelatedBlogs.Add(relatedPost.Id.ToString());
        }
        public void DeleteRelated(Blog blog)
        {
            RelatedBlogs.Remove(blog.Id.ToString());
        }
    }
}