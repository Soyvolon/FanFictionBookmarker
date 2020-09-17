using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks.ActiveData;

namespace FanfictionBookmarker.Data.Bookmarks.Comparers
{
    public class FolderComparers : IEqualityComparer<InteractiveFolder>
    {
        public bool Equals([AllowNull] InteractiveFolder x, [AllowNull] InteractiveFolder y)
        {
            if (x is null && y is null) return true;
            if ((x is null && !(y is null)) || !(x is null) && y is null) return false;

            return GetHashCode(x).Equals(GetHashCode(y));
        }

        public int GetHashCode([DisallowNull] InteractiveFolder obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}