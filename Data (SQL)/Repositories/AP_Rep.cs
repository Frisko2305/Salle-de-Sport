using MySql.Data.MySqlClient;
using Salle_Sport.Data;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    public class AP_Rep
    {
        // Récupérer tous les dossiers en attente de validation
        public List<MbProfile> GetDossiersEnAttente()
        {
            List<MbProfile> dossiers = new List<MbProfile>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT d.*, u.Nom, u.Prenom, u.email
                    FROM Dossier_Mb d
                    INNER JOIN Utilisateur u ON d.id_user = u.id_User
                    WHERE d.statut = 'EN_ATTENTE'
                    ORDER BY d.date_creat_dossier ASC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dossiers.Add(new MbProfile
                        {
                            IdUser = reader.GetInt32("id_user"),
                            Statut = reader.GetString("statut"),
                            DateCreatDossier = reader.GetDateTime("date_creat_dossier"),
                            User = new User
                            {
                                Id = reader.GetInt32("id_user"),
                                Nom = reader.GetString("Nom"),
                                Prenom = reader.GetString("Prenom"),
                                Email = reader.GetString("email")
                            }
                        });
                    }
                }
            }

            return dossiers;
        }

        // Valider un dossier membre
        public bool ValiderDossier(int Id, int validePar)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Dossier_Mb 
                                     SET statut = 'ACTIF', 
                                         date_valid_admin = NOW(), 
                                         valide_par = @validePar
                                     WHERE id_user = @Id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@validePar", validePar);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR ValiderDossier] {ex.Message}");
                return false;
            }
        }

        // Refuser un dossier membre
        public bool RefuserDossier(int Id)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Dossier_Mb 
                                     SET statut = 'REFUSE'
                                     WHERE id_user = @Id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR RefuserDossier] {ex.Message}");
                return false;
            }
        }

        // Bannir un membre (appelle la procédure stockée)
        public bool BannirMembre(int Id, string motif)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = "CALL Ban_Mb(@Id, @motif)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@motif", motif);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR BannirMembre] {ex.Message}");
                return false;
            }
        }

        // Récupérer les membres absents (Vue SQL)
        public List<User> GetMembresAbsents()
        {
            List<User> membres = new List<User>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Vue_Mb_Absent";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        membres.Add(new User
                        {
                            Id = reader.GetInt32("id_User"),
                            Nom = reader.GetString("nom"),
                            Prenom = reader.GetString("prenom"),
                            Email = reader.GetString("email")
                        });
                    }
                }
            }

            return membres;
        }

        // Voir tous les membres actifs
        public List<MbProfile> GetTousMembres()
        {
            List<MbProfile> membres = new List<MbProfile>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT d.*, u.Nom, u.Prenom, u.email
                    FROM Dossier_Mb d
                    INNER JOIN Utilisateur u ON d.id_user = u.id_User
                    ORDER BY d.date_creat_dossier DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        membres.Add(new MbProfile
                        {
                            IdUser = reader.GetInt32("id_user"),
                            Statut = reader.GetString("statut"),
                            MotifBan = reader.IsDBNull(reader.GetOrdinal("motif_ban")) ? null : reader.GetString("motif_ban"),
                            DateCreatDossier = reader.GetDateTime("date_creat_dossier"),
                            DateValidAdmin = reader.IsDBNull(reader.GetOrdinal("date_valid_admin")) ? null : reader.GetDateTime("date_valid_admin"),
                            User = new User
                            {
                                Id = reader.GetInt32("id_user"),
                                Nom = reader.GetString("Nom"),
                                Prenom = reader.GetString("Prenom"),
                                Email = reader.GetString("email")
                            }
                        });
                    }
                }
            }

            return membres;
        }
    }
}