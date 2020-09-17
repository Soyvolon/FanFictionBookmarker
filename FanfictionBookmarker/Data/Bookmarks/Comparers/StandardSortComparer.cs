using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FanfictionBookmarker.Data.Bookmarks.Comparers
{
    public class StandardSortComparer : IComparer<BaseBookmarkData>
    {
        public int Compare([AllowNull] BaseBookmarkData x, [AllowNull] BaseBookmarkData y)
        {
            if (x is null && y is null) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            var comp = x.Priority.CompareTo(y.Priority);

            if (comp == 0)
                return x.DisplayName.CompareTo(y.DisplayName);

            return comp;
        }
    }
}
