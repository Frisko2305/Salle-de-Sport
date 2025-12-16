using MySql.Data.MySqlClient;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    public class Mb_Rep
    {
        public List<Seance> GetSeanceDispo()
        {
            List<Seance> seances = new List<Seance>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT s.id_Seance, s.dateH_debut, s.durée, s.cap_max, s.id_coach, s.id_ade, c.Nom AS NomCoach, c.Prenom AS PrenomCoach, a.Nom_Ade
                    FROM Seance s
                    INNER JOIN Coach c ON s.id_coach = c.id_Coach
                    INNER JOIN Activite a ON s.id_ade = a.id_Ade
                    WHERE s.dateH_debut > NOW()
                    ORDER BY s.dateH_debut ASC";
                
                using (var cmd = new MySqlCommand(query, conn))
                using(var reader = cmd.ExecuteReader())             //using avec deux paramètres
                {
                    while (reader.Read())
                    {
                        seances.Add(new Seance
                        {
                            Id = reader.GetInt32("id_Seance"),
                            DateHDebut = reader.GetDateTime("dateH_debut"),
                            Duree = reader.GetInt32("durée"),
                            Cap_Max = reader.GetInt32("cap_max"),
                            IdCoach = reader.GetInt32("id_coach"),
                            IdAde = reader.GetInt32("id_ade"),
                            NomCoach = $"{reader.GetString("PrenomCoach")} {reader.GetString("NomCoach")}",
                            NomAde = reader.GetString("Nom_Ade")
                        });
                    }
                }
            }

            return seances;
        }

        public bool Inscri_Seance(int idUser, int idSeance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Inscri_Seance (id_user, id_seance, date_insc, present)
                                     VALUES (@IdUser, @IdSeance, NOW(), FALSE)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUser", idUser);
                        cmd.Parameters.AddWithValue("@IdSeance", idSeance);

                        return cmd.ExecuteNonQuery() > 0; //Si une ligne a été affectée, l'insertion a réussi
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR REGISTER SESSION : {ex.Message}");
                return false;
            }
        }

        public List<Inscri_Seance> GetMyInscri_Seances(int idUser)
        {
            List<Inscri_Seance> inscription = new List<Inscri_Seance>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"SELECT i.id_user, i.id_seance, i.date_insc, i.present, s.dateH_debut, s.durée, s.cap_max, a.Nom_Ade, c.Nom AS NomCoach
                                 FROM Inscri_Seance i
                                 INNER JOIN Seance s ON i.id_seance = s.id_Seance
                                 INNER JOIN Activite a ON s.id_ade = a.id_Ade
                                 INNER JOIN Coach c ON s.id_coach = c.id_Coach
                                 WHERE i.id_user = @idUser
                                 ORDER BY s.dateH_debut DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUser", idUser);

                    using(var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            inscription.Add(new Inscri_Seance
                            {
                                IdUser = reader.GetInt32("id_user"),
                                IdSeance = reader.GetInt32("id_seance"),
                                DateInsc = reader.GetDateTime("date_insc"),
                                Present = reader.GetBoolean("present"),
                                Seance = new Seance
                                {
                                    DateHDebut = reader.GetDateTime("dateH_debut"),
                                    Duree = reader.GetInt32("durée"),
                                    Cap_Max = reader.GetInt32("cap_max"),
                                    NomAde = reader.GetString("Nom_Ade"),
                                    NomCoach = reader.GetString("NomCoach")
                                }
                            });
                        }
                    }
                }
            }

            return inscription;
        }

        public bool Desinscrire_Seance(int idUser, int idSeance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"DELETE FROM Inscri_Seance 
                                    WHERE id_user = @IdUser AND id_seance = @IdSeance";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUser", idUser);
                        cmd.Parameters.AddWithValue("@IdSeance", idSeance);

                        return cmd.ExecuteNonQuery() > 0; // Retourne true si une ligne a été supprimée
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR UNREGISTER SESSION : {ex.Message}");
                return false;
            }
        }

        public string GetStatutDossier(int idUser)
        {
            string statut = "INEXISTANT";

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"SELECT statut 
                                 FROM Dossier_Mb 
                                 WHERE id_user = @idUser";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUser", idUser);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            statut = reader.GetString("statut");
                        }
                    }
                }
            }

            return statut;
        }

        public bool ModifierProfil(int idUser, string nom, string prenom, string email, string mdp)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Utilisateur 
                                     SET Nom = @nom, Prenom = @prenom, email = @email, mdp = @mdp 
                                     WHERE id_User = @idUser";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUser", idUser);
                        cmd.Parameters.AddWithValue("@nom", nom);
                        cmd.Parameters.AddWithValue("@prenom", prenom);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@mdp", mdp);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR MODIFIER PROFIL : {ex.Message}");
                return false;
            }
        }

        public bool JeQuitteLaSalle(int idUser)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "CALL JeQuitte(@idUser)";
            
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUser", idUser);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR JE QUITTE : {ex.Message}");
                return false;
            }
        }
    }
}
