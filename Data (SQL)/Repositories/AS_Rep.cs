using MySql.Data.MySqlClient;
using Salle_Sport.Data;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    public class AsRepository
    {
        
        public List<Activite> GetToutesActivites()
        {
            List<Activite> activites = new List<Activite>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Activite ORDER BY Nom_Ade";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activites.Add(new Activite
                        {
                            Id = reader.GetInt32("id_Ade"),
                            NomAde = reader.GetString("Nom_Ade"),
                            Descri = reader.IsDBNull(reader.GetOrdinal("descri")) ? string.Empty : reader.GetString("descri")
                        });
                    }
                }
            }

            return activites;
        }

        public bool CreerActivite(Activite activite)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Activite (Nom_Ade, descri) 
                                     VALUES (@nom, @descri)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nom", activite.NomAde);
                        cmd.Parameters.AddWithValue("@descri", activite.Descri);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR CreerActivite] {ex.Message}");
                return false;
            }
        }

        public bool ModifierActivite(Activite activite)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Activite 
                                     SET Nom_Ade = @nom, descri = @descri 
                                     WHERE id_Ade = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", activite.Id);
                        cmd.Parameters.AddWithValue("@nom", activite.NomAde);
                        cmd.Parameters.AddWithValue("@descri", activite.Descri);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR ModifierActivite] {ex.Message}");
                return false;
            }
        }

        public bool SupprimerActivite(int idActivite)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Activite WHERE id_Ade = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idActivite);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR SupprimerActivite] {ex.Message}");
                return false;
            }
        }

        public List<Coach> GetTousCoachs()
        {
            List<Coach> coachs = new List<Coach>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Coach ORDER BY Nom, Prenom";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coachs.Add(new Coach
                        {
                            Id = reader.GetInt32("id_Coach"),
                            Nom = reader.GetString("Nom"),
                            Prenom = reader.GetString("Prenom"),
                            Specialite = reader.IsDBNull(reader.GetOrdinal("Spécialité")) ? string.Empty : reader.GetString("Spécialité")
                        });
                    }
                }
            }

            return coachs;
        }

        public bool CreerCoach(Coach coach)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Coach (Nom, Prenom, Spécialité) 
                                     VALUES (@nom, @prenom, @specialite)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nom", coach.Nom);
                        cmd.Parameters.AddWithValue("@prenom", coach.Prenom);
                        cmd.Parameters.AddWithValue("@specialite", coach.Specialite);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR CreerCoach] {ex.Message}");
                return false;
            }
        }

        public bool ModifierCoach(Coach coach)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Coach 
                                     SET Nom = @nom, Prenom = @prenom, Spécialité = @specialite 
                                     WHERE id_Coach = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", coach.Id);
                        cmd.Parameters.AddWithValue("@nom", coach.Nom);
                        cmd.Parameters.AddWithValue("@prenom", coach.Prenom);
                        cmd.Parameters.AddWithValue("@specialite", coach.Specialite);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR ModifierCoach] {ex.Message}");
                return false;
            }
        }

        public bool SupprimerCoach(int idCoach)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Coach WHERE id_Coach = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idCoach);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR SupprimerCoach] {ex.Message}");
                return false;
            }
        }

        public List<Seance> GetToutesSeances()
        {
            List<Seance> seances = new List<Seance>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT s.id_Seance, s.dateH_debut, s.durée, s.cap_max, 
                           s.id_coach, s.id_ade,
                           c.Nom AS NomCoach, c.Prenom AS PrenomCoach,
                           a.Nom_Ade
                    FROM Seance s
                    INNER JOIN Coach c ON s.id_coach = c.id_Coach
                    INNER JOIN Activite a ON s.id_ade = a.id_Ade
                    ORDER BY s.dateH_debut DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
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

        public bool CreerSeance(Seance seance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Seance (dateH_debut, durée, cap_max, id_coach, id_ade) 
                                     VALUES (@dateDebut, @duree, @capMax, @idCoach, @idAde)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dateDebut", seance.DateHDebut);
                        cmd.Parameters.AddWithValue("@duree", seance.Duree);
                        cmd.Parameters.AddWithValue("@capMax", seance.Cap_Max);
                        cmd.Parameters.AddWithValue("@idCoach", seance.IdCoach);
                        cmd.Parameters.AddWithValue("@idAde", seance.IdAde);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR CreerSeance] {ex.Message}");
                return false;
            }
        }

        public bool ModifierSeance(Seance seance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Seance 
                                     SET dateH_debut = @dateDebut, 
                                         durée = @duree, 
                                         cap_max = @capMax, 
                                         id_coach = @idCoach, 
                                         id_ade = @idAde
                                     WHERE id_Seance = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", seance.Id);
                        cmd.Parameters.AddWithValue("@dateDebut", seance.DateHDebut);
                        cmd.Parameters.AddWithValue("@duree", seance.Duree);
                        cmd.Parameters.AddWithValue("@capMax", seance.Cap_Max);
                        cmd.Parameters.AddWithValue("@idCoach", seance.IdCoach);
                        cmd.Parameters.AddWithValue("@idAde", seance.IdAde);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR ModifierSeance] {ex.Message}");
                return false;
            }
        }

        public bool SupprimerSeance(int idSeance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Seance WHERE id_Seance = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idSeance);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR SupprimerSeance] {ex.Message}");
                return false;
            }
        }

        public bool MarquerPresence(int idUser, int idSeance, bool present)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Inscri_Seance 
                                     SET present = @present 
                                     WHERE id_user = @idUser AND id_seance = @idSeance";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@present", present);
                        cmd.Parameters.AddWithValue("@idUser", idUser);
                        cmd.Parameters.AddWithValue("@idSeance", idSeance);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR MarquerPresence] {ex.Message}");
                return false;
            }
        }

        public List<Inscri_Seance> GetInscriptionsPourSeance(int idSeance)
        {
            List<Inscri_Seance> inscriptions = new List<Inscri_Seance>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT I.id_user, I.id_seance, I.date_insc, I.present,
                           U.Nom, U.Prenom, U.email
                    FROM Inscri_Seance I
                    INNER JOIN Utilisateur U ON I.id_user = U.id_User
                    WHERE I.id_seance = @idSeance
                    ORDER BY U.Nom, U.Prenom";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idSeance", idSeance);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inscriptions.Add(new Inscri_Seance
                            {
                                IdUser = reader.GetInt32("id_user"),
                                IdSeance = reader.GetInt32("id_seance"),
                                DateInsc = reader.GetDateTime("date_insc"),
                                Present = reader.GetBoolean("present"),
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
            }

            return inscriptions;
        }

        public bool SupprimerInscription(int idUser, int idSeance)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Inscri_Seance WHERE id_user = @idUser AND id_seance = @idSeance";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUser", idUser);
                        cmd.Parameters.AddWithValue("@idSeance", idSeance);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR SupprimerInscription] {ex.Message}");
                return false;
            }
        }
    }
}
