using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks;
using FanfictionBookmarker.Data.Bookmarks.ActiveData;
using FanfictionBookmarker.Data.Bookmarks.Comparers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanfictionBookmarker.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FFBUser class
    public class FFBUser : IdentityUser<Guid>
    {
        public List<BookmarkFolder> Folders { get; private set; }
        public List<FanficBookmark> Bookmarks { get; private set; }
        public BookmarkFolder DefaultFolder { get; set; }

        public FFBUser() { }
        
        public void Initialize()
        {
            Folders = new List<BookmarkFolder>();
            Bookmarks = new List<FanficBookmark>();
            DefaultFolder = new BookmarkFolder("Bookmarks", -1, null);
        }

        public void Update(FolderSystem system)
        {
            Folders = system.Folders;
            Bookmarks = system.Bookmarks;
            DefaultFolder = system.Home;
        }
    }
}