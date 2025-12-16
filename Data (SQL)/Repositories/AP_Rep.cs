using MySql.Data.MySqlClient;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    /// <summary>
    /// Repository pour les fonctionnalités de l'Administrateur Principal
    /// Gère la validation des dossiers membres, bannissements et suivi des absences
    /// </summary>
    public class AP_Rep
    {
        /// <summary>
        /// Récupère tous les dossiers membres en attente de validation
        /// </summary>
        /// <returns>Liste des profils membres avec statut EN_ATTENTE</returns>
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

        /// <summary>
        /// Valide un dossier membre et change son statut à ACTIF
        /// </summary>
        /// <param name="Id">ID de l'utilisateur dont le dossier doit être validé</param>
        /// <param name="validePar">ID de l'administrateur qui valide le dossier</param>
        /// <returns>true si la validation a réussi, false en cas d'erreur</returns>
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

        /// <summary>
        /// Refuse un dossier membre et change son statut à REFUSE
        /// </summary>
        /// <param name="Id">ID de l'utilisateur dont le dossier doit être refusé</param>
        /// <returns>true si le refus a réussi, false en cas d'erreur</returns>
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

        /// <summary>
        /// Bannit un membre de la salle en appelant la procédure stockée Ban_Mb
        /// </summary>
        /// <param name="Id">ID de l'utilisateur à bannir</param>
        /// <param name="motif">Raison du bannissement</param>
        /// <returns>true si le bannissement a réussi, false en cas d'erreur</returns>
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

        /// <summary>
        /// Récupère la liste des membres absents via la vue SQL Vue_Mb_Absent
        /// </summary>
        /// <returns>Liste des utilisateurs considérés comme absents</returns>
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

        /// <summary>
        /// Récupère tous les dossiers membres (tous statuts confondus)
        /// </summary>
        /// <returns>Liste complète des profils membres avec leurs informations</returns>
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