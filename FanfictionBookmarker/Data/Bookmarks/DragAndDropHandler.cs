using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanfictionBookmarker.Data.Bookmarks.ActiveData;
using FanfictionBookmarker.Data.Bookmarks.DataModels;

using Microsoft.CodeAnalysis.CodeFixes;

namespace FanfictionBookmarker.Data.Bookmarks
{
    public static class DragAndDropHandler
    {
        public static void CompleteReorder(FolderSystem data, BaseBookmarkData dropped, BaseBookmarkData droppedOn)
        {
            if (!DropChecks(data, dropped, droppedOn)) return;

            switch(dropped)
            {
                case InteractiveFolder folder:
                    DroppedFolder(data, folder, droppedOn);
                    break;
                case FanficBookmark fic:
                    DroppedFic(data, fic, droppedOn);
                    break;
            }
        }

        private static void DroppedFolder(FolderSystem data, InteractiveFolder folder, BaseBookmarkData droppedOn)
        {
            var index = GetIndex(data, droppedOn);
            FolderModel folderModel;

            switch(droppedOn)
            {
                case InteractiveFolder droppedOnFolder:
                    folderModel = new FolderModel(folder, 0)
                    {
                        ParentFolder = droppedOnFolder.Id.ToString()
                    };
                    data.UpdateFolder(folderModel);
                    break;
                case FanficBookmark droppedOnFic:
                    folderModel = new FolderModel(folder, index)
                    {
                        ParentFolder = droppedOnFic.Parent?.Id.ToString() ?? Guid.Empty.ToString()
                    };
                    data.UpdateFolder(folderModel);
                    break;
            }
        }

        private static void DroppedFic(FolderSystem data, FanficBookmark fic, BaseBookmarkData droppedOn)
        {
            var index = GetIndex(data, droppedOn);
            FanficModel ficModel;

            switch (droppedOn)
            {
                case InteractiveFolder droppedOnFolder:
                    ficModel = new FanficModel(fic, 0)
                    {
                        ParentFolder = droppedOnFolder.Id.ToString()
                    };
                    data.UpdateFanfic(ficModel);
                    break;
                case FanficBookmark droppedOnFic:
                    ficModel = new FanficModel(fic, index)
                    {
                        ParentFolder = droppedOnFic.Parent?.Id.ToString() ?? Guid.Empty.ToString()
                    };
                    data.UpdateFanfic(ficModel);
                    break;
            }
        }

        private static int GetIndex(FolderSystem data, BaseBookmarkData item)
        {
            if (InteractiveFolder.TryGetFolder(data.Home, item.Parent ?? data.Home, out InteractiveFolder f))
            {
                switch (item)
                {
                    case FanficBookmark mark:
                        return f.Contents.IndexOf(mark);
                    case InteractiveFolder folder:
                        return f.Folders.IndexOf(folder);
                    case BookmarkFolder bFolder:
                        return f.Folders.FindIndex(x => x.Id == bFolder.Id);
                }
            }

            return -1;
        }

        private static bool DropChecks(FolderSystem data, BaseBookmarkData dropped, BaseBookmarkData droppedOn)
        {
            if (data is null || dropped is null || droppedOn is null) return false;

            if (dropped.Id == droppedOn.Id) return false;

            return true;
        }
    }
}
