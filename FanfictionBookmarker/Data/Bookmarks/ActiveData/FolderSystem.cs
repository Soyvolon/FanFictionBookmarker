using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Areas.Identity.Data;
using FanfictionBookmarker.Data.Bookmarks.DataModels;

namespace FanfictionBookmarker.Data.Bookmarks.ActiveData
{
    public class FolderSystem
    {
        public InteractiveFolder Home { get; private set; }
        public List<BookmarkFolder> Folders { get; private set; }
        public List<FanficBookmark> Bookmarks { get; private set; }

        public FolderSystem(FFBUser user)
        {
            Home = new InteractiveFolder(user.DefaultFolder);
            Folders = user.Folders;
            Bookmarks = user.Bookmarks;
            PopulateFolderSystem();
        }

        public bool UpdateFolder(FolderModel model)
        {
            var folder = Folders.FirstOrDefault(x => x.Id == model.Id);

            var guid = new Guid(model.ParentFolder);
            var modelFolder = Folders.FirstOrDefault(x => x.Id == guid);
            if (modelFolder == null) modelFolder = Home;

            if (folder != null)
            {
                if(folder.Parent != modelFolder)
                {
                    folder.DisplayName = model.Name;
                    var old = folder.Parent;
                    folder.Parent = modelFolder;

                    return UpdatePosition(folder, old);
                }
                else
                {
                    folder.DisplayName = model.Name;

                    return true;
                }
            }
            else
            {
                var newFolder = new BookmarkFolder(model.Name, 0, modelFolder);
                Folders.Add(newFolder);
                return TryAddFolder(newFolder, out _);
            }
        }

        public bool UpdateFanfic(FanficModel model)
        {
            var fanfic = Bookmarks.FirstOrDefault(x => x.Id == model.Id);

            var guid = new Guid(model.ParentFolder);
            var folder = Folders.FirstOrDefault(x => x.Id == guid);
            if (folder == default) folder = Home;

            if (fanfic != default)
            {
                if(fanfic.Parent != folder)
                {
                    fanfic.FicLink = model.FanficUrl;
                    fanfic.Description = model.Description;
                    fanfic.DisplayName = model.Name;
                    fanfic.Title = model.FanficTitle;
                    var old = fanfic.Parent;
                    fanfic.Parent = folder;

                    return UpdatePosition(fanfic, old);
                }
                else
                {
                    fanfic.FicLink = model.FanficUrl;
                    fanfic.Description = model.Description;
                    fanfic.DisplayName = model.Name;
                    fanfic.Title = model.FanficTitle;

                    return true;
                }
            }
            else
            {
                var newFic = new FanficBookmark(model.FanficUrl, model.FanficTitle, model.Description, model.Name, 0, folder);
                Bookmarks.Add(newFic);
                return AddFanfic(newFic);
            }
        }

        public bool TryAddFolder(BookmarkFolder folder, out InteractiveFolder iFolder)
        {
            if(InteractiveFolder.TryGetFolder(Home, folder.Parent, out InteractiveFolder res))
            {
                return res.TryAddFolder(folder, out iFolder);
            }

            iFolder = null;
            return false;
        }

        public bool AddFanfic(FanficBookmark fanfic)
        {
            if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent, out InteractiveFolder f))
            {
                return f.Contents.Add(fanfic);
            }

            return false;
        }

        public bool UpdatePosition(FanficBookmark fanfic, BookmarkFolder oldParent)
        {
            if(InteractiveFolder.TryGetFolder(Home, oldParent, out InteractiveFolder old))
            {
                if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent, out InteractiveFolder newParent))
                {
                    return UpdatePosition(fanfic, newParent, old);
                }
            }

            return false;
        }

        public bool UpdatePosition(FanficBookmark fanfic, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            return oldParent.Contents.Remove(fanfic) && newParent.Contents.Add(fanfic);
        }

        public bool UpdatePosition(BookmarkFolder folder, BookmarkFolder oldParent)
        {
            if (InteractiveFolder.TryGetFolder(Home, folder, out InteractiveFolder thisFolder))
            {
                if (InteractiveFolder.TryGetFolder(Home, oldParent, out InteractiveFolder old))
                {
                    if (InteractiveFolder.TryGetFolder(Home, folder.Parent, out InteractiveFolder newFolder))
                    {
                        return UpdatePosition(thisFolder, newFolder, old);
                    }
                }
            }

            return false;
        }

        public bool UpdatePosition(InteractiveFolder folder, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            return oldParent.Folders.Remove(folder) && newParent.Folders.Add(folder);
        }

        private void PopulateFolderSystem()
        {
            var data = new List<BaseBookmarkData>();
            data.AddRange(Folders);
            data.AddRange(Bookmarks);

            data.ForEach(x =>
            {
                if (x.Parent is null)
                    x.Parent = Home;
            });

            FillSystemData(Home, data);
        }

        private static void FillSystemData(InteractiveFolder start, List<BaseBookmarkData> data)
        {
            // Get children of the folder ....
            var set = data.Where(x => x.Parent?.Id == start.Id);
            foreach (var item in set)
            {
                //... if its a bookmark, save it ...
                if (item is FanficBookmark bookmark)
                {
                    start.Contents.Add(bookmark);
                }
                // ... but if it is a folder ....
                else if (item is BookmarkFolder folder)
                {
                    // ... create a new interactive version of the folder ...
                    var interactiveFolder = new InteractiveFolder(folder);
                    // ... add it to the parents folder list ...
                    start.Folders.Add(interactiveFolder);
                    // ... and fill that folder before returning to this loop.
                    FillSystemData(interactiveFolder, data);
                }
            }
        }
    }
}