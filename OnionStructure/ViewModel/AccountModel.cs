using System.ComponentModel.DataAnnotations;

namespace OnionStructure.ViewModel
{
    public class AccountModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
