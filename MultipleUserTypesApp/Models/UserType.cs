using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleUserTypesApp.Models
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string Title { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
