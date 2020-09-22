using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Areas.Identity.Data;
using FanfictionBookmarker.Data.Bookmarks.DataModels;
using FanfictionBookmarker.Data.Extensions;
using Microsoft.AspNetCore.Razor.Language;

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

        public UpdateResult UpdateFolder(FolderModel model)
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
                    else return new UpdateResult()
                    {
                        Success = true,
                        Message = "Folder Data Updated."
                    };
                }
            }
            else
            {
                var newFolder = new BookmarkFolder(model.Name, modelFolder);
                Folders.Add(newFolder);
                return TryAddFolder(newFolder, out _);
            }
        }

        public UpdateResult UpdateFanfic(FanficModel model)
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
                    else return new UpdateResult()
                    {
                        Success = true,
                        Message = "Bookmark Data Updated"
                    };
                }
            }
            else
            {
                var newFic = new FanficBookmark(model.FanficUrl, model.FanficTitle, model.Description, model.Name, folder);
                Bookmarks.Add(newFic);
                return AddFanfic(newFic);
            }
        }

        public UpdateResult TryAddFolder(BookmarkFolder folder, out InteractiveFolder iFolder)
        {
            List<string> errors = new List<string>();

            if(InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder res))
            {
                if(res.TryAddFolder(folder, out iFolder))
                {
                    return new UpdateResult()
                    {
                        Success = true,
                        Message = "Folder added."
                    };
                }
                else
                {
                    errors.Add("Failed to add the new folder.");
                }
            }
            else
            {
                errors.Add("Failed to get the parent folder.");
            }

            iFolder = null;
            return new UpdateResult()
            {
                Errors = errors,
                Success = false
            };
        }

        public UpdateResult AddFanfic(FanficBookmark fanfic)
        {
            List<string> errors = new List<string>();

            if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent ?? Home, out InteractiveFolder f))
            {
                f.Contents.Insert(0, fanfic);
                return new UpdateResult()
                {
                    Success = true,
                    Message = "New Bookmark Added"
                };
            }
            else
            {
                errors.Add("Failed to get parent folder.");
            }

            return new UpdateResult()
            {
                Success = false,
                Errors = errors
            };
        }

        public UpdateResult UpdatePosition(FanficBookmark fanfic, int index, BookmarkFolder oldParent)
        {
            List<string> errors = new List<string>();

            if(InteractiveFolder.TryGetFolder(Home, oldParent ?? Home, out InteractiveFolder old))
            {
                if(InteractiveFolder.TryGetFolder(Home, fanfic.Parent ?? Home, out InteractiveFolder newParent))
                {
                    return UpdatePosition(fanfic, index, newParent, old);
                }
                else
                {
                    errors.Add("Failed to get new parent folder.");
                }
            }
            else
            {
                errors.Add("Failed to get old parent folder");
            }

            return new UpdateResult()
            {
                Success = false,
                Errors = errors
            };
        }

        public UpdateResult UpdatePosition(FanficBookmark bookmark, int index)
        {
            List<string> errors = new List<string>();

            if (InteractiveFolder.TryGetFolder(Home, bookmark.Parent ?? Home, out InteractiveFolder iParent))
            {
                return UpdatePosition(bookmark, index, iParent, iParent);
            }
            else
            {
                errors.Add("Failed to get bookmark parent.");
            }

            return new UpdateResult()
            {
                Success = false,
                Errors = errors
            };
        }

        public UpdateResult UpdatePosition(FanficBookmark fanfic, int index, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            List<string> errors = new List<string>();

            if (oldParent.Contents.Remove(fanfic))
            {
                try
                {
                    newParent.Contents.Insert(index, fanfic);

                    newParent.Contents.AssignIndexValues();

                    return new UpdateResult()
                    {
                        Success = true,
                        Message = "Update Complete"
                    };
                }
                catch (ArgumentOutOfRangeException)
                {
                    newParent.Contents.Add(fanfic);

                    newParent.Contents.AssignIndexValues();

                    errors.Add("Failed to insert bookmark at position. Added folder to end of list.");
                    return new UpdateResult()
                    {
                        Success = true,
                        Message = "Failed to instert bookmark at position, added folder to end of list.",
                        Errors = errors
                    };
                }
            }
            else
            {
                errors.Add("Failed to remove bookmark from start position");
            }

            return new UpdateResult()
            {
                Errors = errors,
                Success = false
            };
        }

        public UpdateResult UpdatePosition(BookmarkFolder folder, int index, BookmarkFolder oldParent)
        {
            List<string> errors = new List<string>();
            
            if (InteractiveFolder.TryGetFolder(Home, folder, out InteractiveFolder thisFolder))
            {
                if (InteractiveFolder.TryGetFolder(Home, oldParent ?? Home, out InteractiveFolder old))
                {
                    if (InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder newFolder))
                    {
                        return UpdatePosition(thisFolder, index, newFolder, old);
                    }
                    else
                    {
                        errors.Add("Failed to get folders new parent.");
                    }
                }
                else
                {
                    errors.Add("Failed to get folders old parent.");
                }
            }
            else
            {
                errors.Add("Failed to get folder.");
            }

            return new UpdateResult()
            {
                Errors = errors,
                Success = false
            };
        }

        public UpdateResult UpdatePosition(BookmarkFolder folder, int index)
        {
            List<string> errors = new List<string>();

            if (InteractiveFolder.TryGetFolder(Home, folder, out InteractiveFolder thisFolder))
            {
                if (InteractiveFolder.TryGetFolder(Home, folder.Parent ?? Home, out InteractiveFolder iFolder))
                {
                    return UpdatePosition(thisFolder, index, iFolder, iFolder);
                }
                else
                {
                    errors.Add("Failed to get parent folder.");
                }
            }
            else
            {
                errors.Add("Failed to get folder to update.");
            }

            return new UpdateResult()
            {
                Errors = errors,
                Success = false
            };
        }

        public UpdateResult UpdatePosition(InteractiveFolder folder, int index, InteractiveFolder newParent, InteractiveFolder oldParent)
        {
            List<string> errors = new List<string>();

            if (oldParent.Folders.Remove(folder))
            {
                try
                {
                    newParent.Folders.Insert(index, folder);

                    newParent.Folders.AssignIndexValues();

                    return new UpdateResult()
                    {
                        Success = true,
                        Message = "Update Complete"
                    };
                }
                catch(ArgumentOutOfRangeException)
                {
                    newParent.Folders.Add(folder);

                    newParent.Folders.AssignIndexValues();

                    errors.Add("Failed to insert folder at position. Added folder to end of list.");
                    return new UpdateResult()
                    {
                        Success = true,
                        Message = "Failed to instert folder at position, added folder to end of list.",
                        Errors = errors
                    };
                }
            }
            else
            {
                errors.Add("Failed to remove folder from start position");
            }

            return new UpdateResult()
            {
                Errors = errors,
                Success = false
            };
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
                    // ... see if the index is larger than the count of the list (or is unassigned) ...
                    if (start.Contents.Count <= bookmark.Index || bookmark.Index == -1)
                    { // ... if it is, insert item to the end of the list ...
                        start.Contents.Add(bookmark);
                    }
                    else
                    {
                        // ... otherwise, try and add it to the position it is supposed to be in ...
                        try
                        {
                            start.Contents.Insert(item.Index, bookmark);
                        }
                        catch (ArgumentOutOfRangeException)
                        { // ... if that fails add it to the end of the list ...
                            start.Contents.Add(bookmark);
                        }
                    }
                }
                // ... but if it is a folder ....
                else if (item is BookmarkFolder folder)
                {
                    // ... create a new interactive version of the folder ...
                    var interactiveFolder = new InteractiveFolder(folder);
                    // ... add it to the parents folder list ...
                    if (start.Folders.Count <= interactiveFolder.Index || interactiveFolder.Index == -1)
                    { // ... if the folder's index is higher than the ammount of items (or is unassigned), add it to the end of the list ...
                        start.Folders.Add(interactiveFolder);
                    }
                    else
                    {
                        try
                        { // ... otherwise, try and insert it into its proper place ...
                            start.Folders.Insert(interactiveFolder.Index, interactiveFolder);
                        }
                        catch (ArgumentOutOfRangeException)
                        { // ... and if that fails, add it to the end of the list ...
                            start.Folders.Add(interactiveFolder);
                        }
                    }
                    // ... and fill that folder before returning to this loop.
                    FillSystemData(Home, interactiveFolder, data);
                }
            }
        }
    }
}