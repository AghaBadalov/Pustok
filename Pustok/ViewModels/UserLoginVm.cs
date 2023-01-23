using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels
{
    public class UserLoginVm
    {

        [Required]
        [StringLength(maximumLength: 50)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    
}
