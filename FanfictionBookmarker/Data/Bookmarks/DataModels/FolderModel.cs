using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FanfictionBookmarker.Data.Bookmarks.DataModels
{
    public class FolderModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ParentFolder { get; set; }

        public Guid Id { get; private set; }

        public FolderModel()
        {

        }

        public FolderModel(BookmarkFolder f)
        {
            Name = f?.DisplayName ?? "";
            ParentFolder = f?.Parent.Id.ToString() ?? null;

            Id = f?.Id ?? default;
        }
    }
}
