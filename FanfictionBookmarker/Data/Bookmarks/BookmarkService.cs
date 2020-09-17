using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Areas.Identity.Data;
using FanfictionBookmarker.Data.Bookmarks.ActiveData;
using FanfictionBookmarker.Data.Bookmarks.Comparers;

namespace FanfictionBookmarker.Data.Bookmarks
{
    public class BookmarkService
    {
        public Task<SortedDictionary<BookmarkFolder, SortedSet<FanficBookmark>>> GetDefaultBookmarkSort(FFBUser user)
        {
            var data = new SortedDictionary<BookmarkFolder, SortedSet<FanficBookmark>>(new StandardSortComparer())
            {
                { user.DefaultFolder, new SortedSet<FanficBookmark>(new StandardSortComparer()) }
            };

            foreach(var folder in user.Folders)
            {
                data.TryAdd(folder, new SortedSet<FanficBookmark>(new StandardSortComparer()));
            }

            foreach(var fic in user.Bookmarks)
            {
                if(data.TryGetValue(fic.Parent, out SortedSet<FanficBookmark> set))
                {
                    set.Add(fic);
                }
            }

            return Task.FromResult(data);
        }

        public Task<FolderSystem> GetFolderSystem(FFBUser user)
        {
            if (user.DefaultFolder is null) user.Initialize();

            var sys = new FolderSystem(user);

            return Task.FromResult(sys);
        }
    }
}