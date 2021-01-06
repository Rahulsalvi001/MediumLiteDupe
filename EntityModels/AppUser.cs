using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class AppUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int? UserProfileId { get; set; }
        public string Name { get; set; }        
        public string EmailAddress { get; set; }
        public string BioDescription { get; set; }
        public int? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public UserImage UserImage { get; set; }
    }
}
