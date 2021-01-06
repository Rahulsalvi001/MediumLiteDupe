using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityModels
{
    public class UserImage
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExtensionType { get; set; }        
        public byte[] Data { get; set; }
    }
}
