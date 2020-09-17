using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks.Comparers;

namespace FanfictionBookmarker.Data.Bookmarks.ActiveData
{
    public class InteractiveFolder : BookmarkFolder
    {
        public SortedSet<FanficBookmark> Contents { get; private set; }
        public SortedSet<InteractiveFolder> Folders { get; private set; }

        public InteractiveFolder(BookmarkFolder f) : base(f.DisplayName, f.Priority, f.Parent, f.Id)
        {
            Contents = new SortedSet<FanficBookmark>(new StandardSortComparer());
            Folders = new SortedSet<InteractiveFolder>(new StandardSortComparer());
        }

        public bool TryAddFolder(BookmarkFolder f, out InteractiveFolder interactive)
        {
            interactive = new InteractiveFolder(f);
            return Folders.Add(interactive);
        }

        public static bool TryGetFolder(InteractiveFolder start, BookmarkFolder folder, out InteractiveFolder f)
        {
            if(folder.Id == start.Id)
            {
                f = start;
                return true;
            }

            foreach(var item in start.Folders)
            {
                if(item.Id == folder.Id)
                {
                    f = item;
                    return true;
                }

                if (TryGetFolder(item, folder, out f))
                    return true;
            }

            f = null;
            return false;
        }
    }
}
