using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FanfictionBookmarker.Data.Bookmarks.DataModels
{
    public class FanficModel
    {
        [Required]
        [Display(Description = "Bookmark Name")]
        public string Name { get; set; }
        [Required]
        public string FanficUrl { get; set; }
        [Required]
        public string FanficTitle { get; set; }

        public string Description { get; set; }

        [Required]
        public string ParentFolder { get; set; }

        public Guid Id { get; private set; }

        public FanficModel()
        {

        }

        public FanficModel(FanficBookmark b)
        {
            Name = b?.DisplayName ?? "";
            FanficUrl = b?.FicLink ?? "";
            Description = b?.Description ?? "";
            ParentFolder = b?.Parent.Id.ToString() ?? null;

            Id = b?.Id ?? default;
        }
    }
}
