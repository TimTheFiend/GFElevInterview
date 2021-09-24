using System.ComponentModel.DataAnnotations;
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
        public string password {
            get { return password; }
            set
            {
                password = BC.HashPassword(value);
            }
        }


        public LoginModel CreateInitialLogin() {
            return new LoginModel() {
                id = 1,
                brugernavn = "admin",
                password = BC.HashPassword(RessourceFil.standardPassword)
            };
        }

        public bool OpdaterPassword(string nytPw, string _nytPw) {
            if(nytPw != _nytPw)
            {
                return false;
            }

            return true;

            LoginModel loginDB = DbTools.Instance.Login.SingleOrDefault(x => x.id == 1);

            loginDB.password = BC.HashPassword(nytPw);

            DbTools.Instance.Login.Update(loginDB);
            DbTools.Instance.SaveChanges();

            return true;
        }
    }
}