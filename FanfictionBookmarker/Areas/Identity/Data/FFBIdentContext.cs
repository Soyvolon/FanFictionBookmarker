using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Areas.Identity.Data;
using FanfictionBookmarker.Data.Bookmarks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FanfictionBookmarker.Data
{
    public class FFBIdentContext : IdentityDbContext<FFBUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<BookmarkFolder> Folders { get; set; }
        public DbSet<FanficBookmark> Bookmarks { get; set; }

        public FFBIdentContext(DbContextOptions<FFBIdentContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<BookmarkFolder>().ToTable("BookmarkFolder");
            builder.Entity<FanficBookmark>().ToTable("FanficBookmark");
        }
    }
}
