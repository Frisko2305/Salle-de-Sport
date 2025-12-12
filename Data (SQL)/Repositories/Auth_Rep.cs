using System.ComponentModel.DataAnnotations.Schema;
using MySql.Data.MySqlClient;
using Salle_Sport.Data;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    public class AuthRepository
    {
        public User? Login(string email, string pwd)
        {
            User? foundUser = null;

            using(var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Utilisateur WHERE email = @email AND mdp = @pwd";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pwd", pwd); // Dans la vraie vie, on hacherait ici

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foundUser = new User
                            {
                                Id = reader.GetInt32("id_User"),
                                Nom = reader.GetString("Nom"),
                                Prenom = reader.GetString("Prenom"),
                                Email = reader.GetString("email"),
                                Role = reader.GetString("rôle")
                            };
                        }
                    }
                }
                
                if(foundUser != null)
                {
                    Database.SwitchUserRole(foundUser.Role);
                }

                return foundUser;
            }
        }

        public bool Register(User user)
        {
            using(var conn = Database.GetConnection())
            {
                conn.Open();

                // CODE SQL INSERT ETC
                return true;
            }
        }
    }
}
