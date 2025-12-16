using MySql.Data.MySqlClient;
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
            try
            {
                using(var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Utilisateur (email, mdp, Nom, Prenom, rôle) 
                                     VALUES (@Email, @Pwd, @Nom, @Prenom, @Role)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Pwd", user.Pwd); // Dans la vraie vie, on hacherait ici
                        cmd.Parameters.AddWithValue("@Nom", user.Nom);
                        cmd.Parameters.AddWithValue("@Prenom", user.Prenom);
                        cmd.Parameters.AddWithValue("@Role", user.Role);

                        return cmd.ExecuteNonQuery() > 0;   //Si une ligne a été affectée, l'insertion a réussi
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR REGISTER : {ex.Message}");
                return false;
            }
        }
    }
}
