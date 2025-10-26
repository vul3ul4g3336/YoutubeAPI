using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Videos.Model
{
    public class UploadVideoModel
    {
        public Snippet snippet { get; set; } = new Snippet();
        public Status status { get; set; } = new Status();
        //public string filePath { get; set; }
        public UploadVideoModel(string title, string description,  string privacyStatus, /*string filePath,*/string[] tags = null)
        {
            snippet.title = title;
            snippet.description = description;
            snippet.tags = tags;
            
            status.privacyStatus = privacyStatus;
            //this.filePath = filePath;
        }
        public class Snippet
        {
            public string title { get; set; }
            public string description { get; set; }
            public string[] tags { get; set; }
            public string categoryId { get; set; } = "22";
        }

        public class Status
        {
            public string privacyStatus { get; set; }
        }
    }
}
