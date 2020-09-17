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
        public BookmarkFolder() : this("New Folder", null) { }

        public BookmarkFolder(string DisplayName, BookmarkFolder Folder) : base(DisplayName, Folder)
        {

        }

        public BookmarkFolder(string DisplayName, BookmarkFolder Folder, Guid id) : base(DisplayName, Folder, id) { }
        public BookmarkFolder(string DisplayName, BookmarkFolder Folder, Guid id, Guid iid) : base(DisplayName, Folder, id, iid) { }
    }
}