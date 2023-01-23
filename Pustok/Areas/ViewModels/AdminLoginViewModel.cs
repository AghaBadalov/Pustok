using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Pustok.Areas.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 50,MinimumLength =8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
