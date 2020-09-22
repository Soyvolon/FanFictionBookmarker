using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FanfictionBookmarker.Data.Bookmarks;
using FanfictionBookmarker.Data.Bookmarks.ActiveData;

namespace FanfictionBookmarker.Data.Extensions
{
    public static class BaseBookmarkDataListExtensions
    {
        public static void AssignIndexValues(this List<BaseBookmarkData> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
        }

        public static void AssignIndexValues(this List<FanficBookmark> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
        }

        public static void AssignIndexValues(this List<InteractiveFolder> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
        }
    }
}
