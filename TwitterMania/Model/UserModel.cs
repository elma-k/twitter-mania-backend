using Google.Apis.Compute.v1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterMania.Model
{
    public class UserModel
    {
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Bio { get; set; }
        public DateTime Birthday{ get; set; }
        public string Location { get; set; }
        public string Website{ get; set; }
        
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }


        // public Image  ProfilePicture{ get; set; }
        // public Image BackgroundPicture { get; set; }

    }
}
