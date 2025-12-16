using MySql.Data.MySqlClient;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    /// <summary>
    /// Repository gérant l'authentification et l'inscription des utilisateurs
    /// </summary>
    public class AuthRepository
    {
        /// <summary>
        /// Authentifie un utilisateur avec son email et mot de passe
        /// </summary>
        /// <param name="email">Adresse email de l'utilisateur</param>
        /// <param name="pwd">Mot de passe de l'utilisateur (non haché)</param>
        /// <returns>L'objet User si les identifiants sont corrects, null sinon</returns>
        public User? Login(string? email, string? pwd)
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

                    // Exécution de la requête et lecture du résultat
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
                
                // Si l'authentification réussit, on change le rôle de connexion à la BDD
                if(foundUser != null)
                {
                    Database.SwitchUserRole(foundUser.Role);
                }

                return foundUser;
            }
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur dans la base de données
        /// </summary>
        /// <param name="user">Objet User contenant les informations de l'utilisateur à créer</param>
        /// <returns>true si l'inscription a réussi, false en cas d'erreur</returns>
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

                        // Si une ligne a été affectée, l'insertion a réussi
                        return cmd.ExecuteNonQuery() > 0;
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
