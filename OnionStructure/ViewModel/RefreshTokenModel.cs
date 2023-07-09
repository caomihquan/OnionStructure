using System.ComponentModel.DataAnnotations;

namespace OnionStructure.ViewModel
{
    public class RefreshTokenModel
    {
        [Required]
        public string RefeshToken { get; set; }
    }
}
