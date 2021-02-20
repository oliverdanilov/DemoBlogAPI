using System;
using System.Collections.Generic;
using System.Text;

namespace DemoBlog.Core
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; private set; }
        public void Delete()
        {
            DeletedOn = DateTime.UtcNow;
        }
    }
}