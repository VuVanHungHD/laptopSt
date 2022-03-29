using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    public class PictureStoreContext : DbContext
    {
        public PictureStoreContext() : base("PictureStoreConnection")
        {

        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Picture> pictures { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<BillDetail> billDetails { get; set; }

        public DbSet<Blog> Blogs { get; set; }
    }
}
