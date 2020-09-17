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
        public Task<FolderSystem> GetFolderSystem(FFBUser user)
        {
            if (user.DefaultFolder is null) user.Initialize();

            var sys = new FolderSystem(user);

            return Task.FromResult(sys);
        }
    }
}