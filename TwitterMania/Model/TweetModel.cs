using Google.Apis.Compute.v1.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterMania.Model
{
    public class TweetModel
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string TextContent { get; set; }
        // public ImageList ImagesList { get; set; }
        public DateTime DateTime { get; set; }
        


    }
}
