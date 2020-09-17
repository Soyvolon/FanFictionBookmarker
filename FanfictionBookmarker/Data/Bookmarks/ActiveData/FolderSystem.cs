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
            Home = new InteractiveFolder();

            Folders = user.Folders;
            Bookmarks = user.Bookmarks;
            PopulateFolderSystem();
        }

        public bool UpdateFolder(FolderModel model)
        {
            var folder = Folders.FirstOrDefault(x => x.Id == model.Id);

            var guid = new Guid(model.ParentFolder);
            var modelFolder = Folders.FirstOrDefault(x => x.Id == guid);
            if (modelFolder == null) modelFolder = null;

            if (folder != null)
            {
                if(folder.Parent != modelFolder)
                {
                    folder.DisplayName = model.Name;
                    var old = folder.Parent;
                    folder.Parent = modelFolder;

                    return UpdatePosition(folder, model.Index, old);
                }
                else
                {
                    folder.DisplayName = model.Name;

                    if (model.Index >= 0)
                    {
                        return UpdatePosition(folder, model.Index);
                    }
                    else return true;
                }
            }
            else
            {
                var newFolder = new BookmarkFolder(model.Name, modelFolder);
                Folders.Add(newFolder);
                return TryAddFolder(newFolder, out _);
            }
        }

        public bool UpdateFanfic(FanficModel model)
        {
            var fanfic = Bookmarks.FirstOrDefault(x => x.Id == model.Id);

            var guid = new Guid(model.ParentFolder);
            var folder = Folders.FirstOrDefault(x => x.Id == guid);
            if (folder == default) folder = null;

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

                    return UpdatePosition(fanfic, model.Index, old);
                }
                else
                {
                    fanfic.FicLink = model.FanficUrl;
                    fanfic.Description = model.Description;
                    fanfic.DisplayName = model.Name;
                    fanfic.Title = model.FanficTitle;

                    if (model.Index >= 0)
                    {
                        return UpdatePosition(fanfic, model.Index);
                    }
                    else return true;
                }
            }
            else
            {
                var newFic = new FanficBookmark(model.FanficUrl, model.FanficTitle, model.Description, model.Name, folder);
                Bookmarks.Add(newFic);
                return AddFanfic(newFic);
            }
        }

        public bool TryAddFolder(BookmarkFolder folder, out InteractiveFolder iFolder)
        {
            if(InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder res))
            {
                return res.TryAddFolder(folder, out iFolder);
            }

            iFolder = null;
            return false;
        }

        public bool AddFanfic(FanficBookmark fanfic)
        {
            if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent ?? Home, out InteractiveFolder f))
            {
                f.Contents.Insert(0, fanfic);
                return true;
            }

            return false;
        }

        public bool UpdatePosition(FanficBookmark fanfic, int index, BookmarkFolder oldParent)
        {
            if(InteractiveFolder.TryGetFolder(Home, oldParent ?? Home, out InteractiveFolder old))
            {
                if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent ?? Home, out InteractiveFolder newParent))
                {
                    return UpdatePosition(fanfic, index, newParent, old);
                }
            }

            return false;
        }

        public bool UpdatePosition(FanficBookmark bookmark, int index)
        {
            if (InteractiveFolder.TryGetFolder(Home, bookmark.Parent ?? Home, out InteractiveFolder iParent))
            {
                return UpdatePosition(bookmark, index, iParent, iParent);
            }

            return false;
        }

        public bool UpdatePosition(FanficBookmark fanfic, int index, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            if (oldParent.Contents.Remove(fanfic))
            {
                try
                {
                    newParent.Contents.Insert(index, fanfic);

                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }

            return false;
        }

        public bool UpdatePosition(BookmarkFolder folder, int index, BookmarkFolder oldParent)
        {
            if (InteractiveFolder.TryGetFolder(Home, folder, out InteractiveFolder thisFolder))
            {
                if (InteractiveFolder.TryGetFolder(Home, oldParent ?? Home, out InteractiveFolder old))
                {
                    if (InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder newFolder))
                    {
                        return UpdatePosition(thisFolder, index, newFolder, old);
                    }
                }
            }

            return false;
        }

        public bool UpdatePosition(BookmarkFolder folder, int index)
        {
            if (InteractiveFolder.TryGetFolder(Home, folder, out InteractiveFolder thisFolder))
            {
                if (InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder iFolder))
                {
                    return UpdatePosition(thisFolder, index, iFolder, iFolder);
                }
            }
            return false;
        }

        public bool UpdatePosition(InteractiveFolder folder, int index, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            if (oldParent.Folders.Remove(folder))
            {
                try
                {
                    newParent.Folders.Insert(index, folder);

                    return true;
                }
                catch(ArgumentOutOfRangeException)
                {
                    return false;
                }
            }

            return false;
        }

        private void PopulateFolderSystem()
        {
            var data = new List<BaseBookmarkData>();
            data.AddRange(Folders);
            data.AddRange(Bookmarks);

            FillSystemData(Home, Home, data);
        }

        private static void FillSystemData(InteractiveFolder Home, InteractiveFolder start, List<BaseBookmarkData> data)
        {
            // Get children of the folder ....
            var set = data.Where(x => x.Parent is null ? start.Id == Home.Id : x.Parent.Id == start.Id);
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
                    FillSystemData(Home, interactiveFolder, data);
                }
            }
        }
    }
}