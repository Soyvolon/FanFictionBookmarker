using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks.Comparers;

namespace FanfictionBookmarker.Data.Bookmarks
{
    
    public class BookmarkFolder : BaseBookmarkData
    {
        public BookmarkFolder() : this("New Folder", 0, null) { }

        public BookmarkFolder(string DisplayName, int Priority, BookmarkFolder Folder) : base(DisplayName, Priority, Folder)
        {

        }

        public BookmarkFolder(string DisplayName, int Priority, BookmarkFolder Folder, Guid id) : base(DisplayName, Priority, Folder, id) { }
    }
}