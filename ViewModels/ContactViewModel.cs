using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CarInfoFromDatabase.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [MaxLength(5)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email{ get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [MaxLength(20,ErrorMessage="Too long")]
        public string Message { get; set; }

    }
}
