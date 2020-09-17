using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks.Comparers;

namespace FanfictionBookmarker.Data.Bookmarks.ActiveData
{
    public class InteractiveFolder : BookmarkFolder
    {
        public List<FanficBookmark> Contents { get; private set; }
        public List<InteractiveFolder> Folders { get; private set; }

        public InteractiveFolder(BookmarkFolder f) : base(f.DisplayName, f.Parent, f.Id)
        {
            Initalize();
        }

        public InteractiveFolder() : base("Bookmarks", null, Guid.Empty)
        {
            Initalize();
        }

        public void Initalize()
        {
            Contents = new List<FanficBookmark>();
            Folders = new List<InteractiveFolder>();
        }

        public bool TryAddFolder(BookmarkFolder f, out InteractiveFolder interactive)
        {
            interactive = new InteractiveFolder(f);
            Folders.Add(interactive);
            return true;
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
