using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSAstorePwned.Models
{
    public class PasswordPnwed
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string retHex { get; set; }
        public string retCount { get; set; }
    }
}