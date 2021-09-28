using System.ComponentModel.DataAnnotations;
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

        public void OpdaterPassword(string nytPassword) {
            password = BC.HashPassword(nytPassword);
        }

        /// <summary>
        /// Opretter en "default" admin bruger, efter en ny database bliver lavet.
        /// </summary>
        public LoginModel CreateInitialLogin() {
            return new LoginModel() {
                id = 1,
                brugernavn = "admin",
                password = BC.HashPassword(RessourceFil.standardPassword)
            };
        }
    }
}