using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanfictionBookmarker.Data.Bookmarks.DataModels
{
    public struct UpdateResult
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}
