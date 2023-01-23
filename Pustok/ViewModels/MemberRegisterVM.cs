using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Pustok.ViewModels
{
    public class MemberRegisterVM
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 50),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 50),DataType(DataType.Password)]
        

        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 50), DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }


    }
}
