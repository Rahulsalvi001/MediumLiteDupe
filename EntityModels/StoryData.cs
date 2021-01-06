using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityModels
{
    public class StoryData
    {
        [Key]
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
