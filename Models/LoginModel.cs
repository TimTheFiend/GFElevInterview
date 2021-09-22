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
        public string password { get; set; }

        public LoginModel CreateInitialLogin() {
            return new LoginModel() {
                id = 1,
                brugernavn = "admin",
                password = BC.HashPassword(RessourceFil.standardPassword)
            };
        }

        //ChangePassword
        //1. UnHash kodeord.
        //2. Ændre kodeord.
        //3. Check om password er blevet ændret.
        //4.
        //Return true/false
        public bool ChangePassword(string newPassword) {
            LoginModel loginDB = DbTools.Instance.Login.SingleOrDefault(x => x.id == 1);

            loginDB.password = BC.HashPassword(newPassword);

            DbTools.Instance.Login.Update(loginDB);
            DbTools.Instance.SaveChanges();

            return true;
        }
    }
}