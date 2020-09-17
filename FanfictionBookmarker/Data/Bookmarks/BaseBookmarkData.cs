using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FanfictionBookmarker.Data.Bookmarks
{
    public abstract class BaseBookmarkData : IComparable<BaseBookmarkData>, IComparable<Guid>
    {
        public Guid Id { get; set; }
        [Key]
        public Guid InternalKey { get; set; }
        public string DisplayName { get; set; }
        public BookmarkFolder Parent { get; set; }
        public int Index { get; set; } = -1;

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

        public BaseBookmarkData(string DisplayName, BookmarkFolder Folder, Guid id, Guid Key)
        {
            this.DisplayName = DisplayName;
            this.Parent = Folder;
            Id = id;
            InternalKey = Key;
        }

        public int CompareTo([AllowNull] BaseBookmarkData other)
        {
            return CompareTo(other.Id);
        }

        public int CompareTo([AllowNull] Guid other)
        {
            return this.Id.CompareTo(other);
        }
    }
}
