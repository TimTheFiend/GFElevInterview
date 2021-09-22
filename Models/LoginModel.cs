using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFElevInterview.Models
{
    class LoginModel
    {
        [Key]
        public int id { get; set; }

        //-brugernavn
        public string brugernavn { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
