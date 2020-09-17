using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanfictionBookmarker.Data.Bookmarks
{
    public abstract class BaseBookmarkData
    {
        public Guid Id { get; set; }
        [Key]
        public Guid InternalKey { get; set; }
        public string DisplayName { get; set; }
        public BookmarkFolder Parent { get; set; }

        public BaseBookmarkData(string DisplayName, BookmarkFolder Folder)
        {
            this.DisplayName = DisplayName;
            this.Parent = Folder;
            Id = Guid.NewGuid();
        }

        public BaseBookmarkData(string DisplayName, BookmarkFolder Folder, Guid id)
        {
            this.DisplayName = DisplayName;
            this.Parent = Folder;
            Id = id;
        }
    }
}
