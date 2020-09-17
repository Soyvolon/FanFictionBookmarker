using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanfictionBookmarker.Data.Bookmarks
{
    public class FanficBookmark : BaseBookmarkData
    {
        public string FicLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public FanficBookmark() : base("New Bookmark", 0, null) { }

        public FanficBookmark(string FicLink, string DisplayName, int Priority, BookmarkFolder Folder) 
            : this(FicLink, "", "", DisplayName, Priority, Folder)
        {
            
        }

        public FanficBookmark(string FicLink, string Title, string Description, string DisplayName, int Priority, BookmarkFolder Folder) 
            : base(DisplayName, Priority, Folder)
        {
            this.FicLink = FicLink;
            this.Title = Title;
            this.Description = Description;
        }
    }
}