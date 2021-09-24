using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace GFElevInterview.Models
{
    public class LoginModel
    {
        [Key]
        public int id { get; set; }

        //-brugernavn
        public string brugernavn { get; set; }

        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public void OpdaterPassword(string nytPassword)
        {
            password = BC.HashPassword(nytPassword);
        }

        [NotMapped]
        public string Password { get { return password; } set { password = BC.HashPassword(value); } }

        public LoginModel CreateInitialLogin() {
            return new LoginModel() {
                id = 1,
                brugernavn = "admin",
                password = BC.HashPassword(RessourceFil.standardPassword)
            };
        }

        
    }
}