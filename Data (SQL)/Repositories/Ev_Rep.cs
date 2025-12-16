using MySql.Data.MySqlClient;
using Salle_Sport.Models;

namespace Salle_Sport.Data.Repositories
{
    /// <summary>
    /// Repository pour les événements et statistiques
    /// Gère les consultations et analyses des données de la salle de sport
    /// </summary>
    public class EvRepository
    {
        /// <summary>
        /// Récupère toutes les séances avec le nombre de membres inscrits
        /// </summary>
        /// <returns>Liste de tuples contenant la séance et le nombre d'inscrits</returns>
        public List<(Seance Seance, int NbInscrits)> GetSeancesAvecInscrits()
        {
            var resultat = new List<(Seance, int)>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // Jointures avec GROUP BY pour compter les inscrits par séance
                string query = @"
                    SELECT s.id_Seance, s.dateH_debut, s.durée, s.cap_max,
                           c.Nom AS NomCoach, c.Prenom AS PrenomCoach,
                           a.Nom_Ade,
                           COUNT(i.id_user) AS NbInscrits
                    FROM Seance s
                    INNER JOIN Coach c ON s.id_coach = c.id_Coach
                    INNER JOIN Activite a ON s.id_ade = a.id_Ade
                    LEFT JOIN Inscri_Seance i ON s.id_Seance = i.id_seance
                    GROUP BY s.id_Seance
                    ORDER BY s.dateH_debut DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var seance = new Seance
                        {
                            Id = reader.GetInt32("id_Seance"),
                            DateHDebut = reader.GetDateTime("dateH_debut"),
                            Duree = reader.GetInt32("durée"),
                            Cap_Max = reader.GetInt32("cap_max"),
                            NomCoach = $"{reader.GetString("PrenomCoach")} {reader.GetString("NomCoach")}",
                            NomAde = reader.GetString("Nom_Ade")
                        };

                        int nbInscrits = reader.GetInt32("NbInscrits");

                        resultat.Add((seance, nbInscrits));
                    }
                }
            }

            return resultat;
        }

        /// <summary>
        /// Récupère la liste détaillée des inscrits pour une séance spécifique
        /// </summary>
        /// <param name="idSeance">ID de la séance</param>
        /// <returns>Liste des inscriptions avec informations des membres et statut de présence</returns>
        public List<Inscri_Seance> GetInscritsPourSeance(int idSeance)
        {
            List<Inscri_Seance> inscrits = new List<Inscri_Seance>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT i.id_user, i.id_seance, i.date_insc, i.present,
                           u.Nom, u.Prenom, u.email
                    FROM Inscri_Seance i
                    INNER JOIN Utilisateur u ON i.id_user = u.id_User
                    WHERE i.id_seance = @idSeance
                    ORDER BY u.Nom, u.Prenom";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idSeance", idSeance);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inscrits.Add(new Inscri_Seance
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

            return inscrits;
        }

        /// <summary>
        /// Génère des statistiques sur les activités les plus populaires
        /// </summary>
        /// <returns>Liste de tuples avec nom activité, nombre de séances et nombre d'inscrits</returns>
        public List<(string NomActivite, int NbSeances, int NbInscrits)> GetStatistiquesActivites()
        {
            var stats = new List<(string, int, int)>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT a.Nom_Ade,
                           COUNT(DISTINCT s.id_Seance) AS NbSeances,
                           COUNT(i.id_user) AS NbInscrits
                    FROM Activite a
                    LEFT JOIN Seance s ON a.id_Ade = s.id_ade
                    LEFT JOIN Inscri_Seance i ON s.id_Seance = i.id_seance
                    GROUP BY a.id_Ade
                    ORDER BY NbInscrits DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stats.Add((
                            reader.GetString("Nom_Ade"),
                            reader.GetInt32("NbSeances"),
                            reader.GetInt32("NbInscrits")
                        ));
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Génère des statistiques sur les coachs les plus actifs
        /// </summary>
        /// <returns>Liste de tuples avec nom du coach et nombre de séances animées</returns>
        public List<(string NomCoach, int NbSeances)> GetStatistiquesCoach()
        {
            var stats = new List<(string, int)>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT c.Nom, c.Prenom, COUNT(s.id_Seance) AS NbSeances
                    FROM Coach c
                    LEFT JOIN Seance s ON c.id_Coach = s.id_coach
                    GROUP BY c.id_Coach
                    ORDER BY NbSeances DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nomComplet = $"{reader.GetString("Prenom")} {reader.GetString("Nom")}";
                        stats.Add((nomComplet, reader.GetInt32("NbSeances")));
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Récupère tous les utilisateurs avec leurs rôles
        /// </summary>
        /// <returns>Liste de tous les utilisateurs triés par nom et prénom</returns>
        public List<User> GetTousUtilisateurs()
        {
            List<User> users = new List<User>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT id_User, Nom, Prenom, rôle FROM Utilisateur ORDER BY Nom, Prenom";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("id_User"),
                            Nom = reader.GetString("Nom"),
                            Prenom = reader.GetString("Prenom"),
                            Role = reader.GetString("rôle")
                        });
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Récupère tous les dossiers membres avec leurs statuts actuels
        /// </summary>
        /// <returns>Liste des profils membres avec statuts et informations</returns>
        public List<MbProfile> GetStatutsDossiers()
        {
            List<MbProfile> dossiers = new List<MbProfile>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT d.id_user, d.statut, d.date_creat_dossier, d.motif_ban,
                           u.Nom, u.Prenom
                    FROM Dossier_Mb d
                    INNER JOIN Utilisateur u ON d.id_user = u.id_User
                    ORDER BY d.date_creat_dossier DESC";

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
                            MotifBan = reader.IsDBNull(reader.GetOrdinal("motif_ban")) ? null : reader.GetString("motif_ban"),
                            User = new User
                            {
                                Id = reader.GetInt32("id_user"),
                                Nom = reader.GetString("Nom"),
                                Prenom = reader.GetString("Prenom")
                            }
                        });
                    }
                }
            }

            return dossiers;
        }
    }
}
