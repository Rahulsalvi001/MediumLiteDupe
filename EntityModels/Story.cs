using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class Story
    {
        [Key]
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string StoryContent { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int StoryDataId { get; set; }
        [ForeignKey("StoryDataId")]
        public StoryData StoryData { get; set; }
        public int Status { get; set; }
    }
}
