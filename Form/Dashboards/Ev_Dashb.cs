using Salle_Sport.Models;
using Salle_Sport.Data.Repositories;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Salle_Sport.Data;

namespace Salle_Sport.Forms.Dashboards
{
    public class Ev_Dashb : Form
    {
        private User _currentUser;
        private EvRepository _evRepo;
        private TableLayoutPanel mainLayout;
        private TabControl tabControl;
        private Button btnDeconnexion;
        private Label lblDerniereRequete;
        private DataGridView GridResult;
        private FlowLayoutPanel flowButtons;
        private Button btnRequeteSQL, btnSeancesInscrits, btnStatsActivites, btnStatsCoachs;
        private Button btnTousUsers, btnStatutsDossiers, btnInscritsSeance;

        public Ev_Dashb(User user)
        {
            _currentUser = user;
            _evRepo = new EvRepository();

            this.Text = "Espace Événements - Salle de Sport";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(15)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            Label lblHeader = new Label
            {
                Text = $"Espace Événements - {_currentUser.Prenom}",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            btnDeconnexion = new Button
            {
                Text = "Déconnexion",
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnDeconnexion.Click += BtnDeconnexion_Click;

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            InitializeTabResultats();
            InitializeTabRequetes();

            mainLayout.Controls.Add(lblHeader, 0, 0);
            mainLayout.Controls.Add(btnDeconnexion, 1, 0);
            mainLayout.Controls.Add(tabControl, 0, 1);
            mainLayout.SetColumnSpan(tabControl, 2);

            this.Controls.Add(mainLayout);
        }

        private void InitializeTabResultats()
        {
            TabPage tabResultats = new TabPage("Résultats");
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblDerniereRequete = new Label
            {
                Text = "Aucun résultat. Exécutez une requête depuis l'onglet Requêtes",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5)
            };

            GridResult = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            layout.Controls.Add(lblDerniereRequete, 0, 0);
            layout.Controls.Add(GridResult, 0, 1);
            tabResultats.Controls.Add(layout);
            tabControl.TabPages.Add(tabResultats);
        }

        private void InitializeTabRequetes()
        {
            TabPage tabRequetes = new TabPage("Requêtes");

            flowButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            btnRequeteSQL = CreateQueryButton("Exécuter\nRequête SQL\nPersonnalisée");
            btnRequeteSQL.Click += BtnRequeteSQL_Click;
            
            btnSeancesInscrits = CreateQueryButton("Séances\navec\nNb d'inscrits");
            btnSeancesInscrits.Click += BtnSeancesInscrits_Click;

            btnStatsActivites = CreateQueryButton("Statistiques\npar\nActivité");
            btnStatsActivites.Click += BtnStatsActivites_Click;

            btnStatsCoachs = CreateQueryButton("Statistiques\npar\nCoach");
            btnStatsCoachs.Click += BtnStatsCoachs_Click;

            btnTousUsers = CreateQueryButton("Liste de tous\nles\nUtilisateurs");
            btnTousUsers.Click += BtnTousUsers_Click;

            btnStatutsDossiers = CreateQueryButton("Statuts des\nDossiers\nMembres");
            btnStatutsDossiers.Click += BtnStatutsDossiers_Click;

            btnInscritsSeance = CreateQueryButton("Inscrits pour\nune Séance\nSpécifique");
            btnInscritsSeance.Click += BtnInscritsSeance_Click;

            flowButtons.Controls.Add(btnRequeteSQL);
            flowButtons.Controls.Add(btnSeancesInscrits);
            flowButtons.Controls.Add(btnStatsActivites);
            flowButtons.Controls.Add(btnStatsCoachs);
            flowButtons.Controls.Add(btnTousUsers);
            flowButtons.Controls.Add(btnStatutsDossiers);
            flowButtons.Controls.Add(btnInscritsSeance);

            tabRequetes.Controls.Add(flowButtons);
            tabControl.TabPages.Add(tabRequetes);
        }

        private Button CreateQueryButton(string text)
        {
            return new Button
            {
                Text = text,
                Width = 180,
                Height = 100,
                Margin = new Padding(10),
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private void BtnRequeteSQL_Click(object? sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Exécuter une requête SQL personnalisée";
                dialog.Size = new Size(600, 400);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(15),
                    RowCount = 4,
                    ColumnCount = 1
                };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                Label lblInstruction = new Label
                {
                    Text = "Entrez votre requête SQL :",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                TextBox txtQuery = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Consolas", 10F),
                    WordWrap = true
                };

                Label lblWarning = new Label
                {
                    Text = "Attention : Seules les requêtes SELECT sont autorisées pour des raisons de sécurité",
                    Dock = DockStyle.Fill,
                    ForeColor = Color.DarkOrange,
                    Font = new Font("Segoe UI", 9F),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                FlowLayoutPanel panelButtons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(5)
                };

                Button btnExecuter = new Button
                {
                    Text = "Exécuter",
                    Width = 100,
                    Height = 35,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Width = 100,
                    Height = 35
                };

                btnExecuter.Click += (s, ev) =>
                {
                    string query = txtQuery.Text.Trim();
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        MessageBox.Show("Veuillez entrer une requête SQL.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!Regex.IsMatch(query, @"^\s*SELECT", RegexOptions.IgnoreCase))
                    {
                        MessageBox.Show("Seules les requêtes SELECT sont autorisées.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        ExecuterRequetePersonnalisee(query);
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'exécution de la requête :\n{ex.Message}", "Erreur SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnAnnuler.Click += (s, ev) => dialog.Close();

                panelButtons.Controls.Add(btnExecuter);
                panelButtons.Controls.Add(btnAnnuler);

                layout.Controls.Add(lblInstruction, 0, 0);
                layout.Controls.Add(txtQuery, 0, 1);
                layout.Controls.Add(lblWarning, 0, 2);
                layout.Controls.Add(panelButtons, 0, 3);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnSeancesInscrits_Click(object? sender, EventArgs e)
        {
            try
            {
                var seances = _evRepo.GetSeancesAvecInscrits();

                GridResult.DataSource = seances.Select(s => new
                {
                    ID = s.Seance.Id,
                    Date = s.Seance.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                    Durée = $"{s.Seance.Duree} min",
                    Capacité = s.Seance.Cap_Max,
                    Coach = s.Seance.NomCoach,
                    Activité = s.Seance.NomAde,
                    Nb_Inscrits = s.NbInscrits
                }).ToList();

                lblDerniereRequete.Text = "Séances avec nombre d'inscrits";
                tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStatsActivites_Click(object? sender, EventArgs e)
        {
            try
            {
                var stats = _evRepo.GetStatistiquesActivites();

                GridResult.DataSource = stats.Select(s => new
                {
                    Nom_Activité = s.NomActivite,
                    Nb_Séances = s.NbSeances,
                    Total_Inscrits = s.NbInscrits
                }).ToList();

                lblDerniereRequete.Text = "Statistiques par activité";
                tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStatsCoachs_Click(object? sender, EventArgs e)
        {
            try
            {
                var stats = _evRepo.GetStatistiquesCoach();

                GridResult.DataSource = stats.Select(s => new
                {
                    Nom_Coach = s.NomCoach,
                    Nb_Séances = s.NbSeances
                }).ToList();

                lblDerniereRequete.Text = "Statistiques par coach";
                tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTousUsers_Click(object? sender, EventArgs e)
        {
            try
            {
                var users = _evRepo.GetTousUtilisateurs();

                GridResult.DataSource = users.Select(u => new
                {
                    ID = u.Id,
                    Nom = u.Nom,
                    Prénom = u.Prenom,
                    Rôle = u.Role
                }).ToList();

                lblDerniereRequete.Text = "Liste de tous les utilisateurs";
                tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStatutsDossiers_Click(object? sender, EventArgs e)
        {
            try
            {
                var dossiers = _evRepo.GetStatutsDossiers();

                GridResult.DataSource = dossiers.Select(d => new
                {
                    ID_User = d.IdUser,
                    Nom = d.User?.Nom,
                    Prénom = d.User?.Prenom,
                    Statut = d.Statut,
                    Date_Création = d.DateCreatDossier.ToString("dd/MM/yyyy"),
                    Motif_Ban = d.MotifBan ?? "-"
                }).ToList();

                lblDerniereRequete.Text = "Statuts des dossiers membres";
                tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnInscritsSeance_Click(object? sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Inscrits pour une séance";
                dialog.Size = new Size(350, 200);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(15),
                    RowCount = 3,
                    ColumnCount = 1
                };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                Label lblInstruction = new Label
                {
                    Text = "Entrez l'ID de la séance :",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                NumericUpDown nudIdSeance = new NumericUpDown
                {
                    Dock = DockStyle.Fill,
                    Minimum = 1,
                    Maximum = 999999,
                    Value = 1,
                    Font = new Font("Segoe UI", 10F)
                };

                FlowLayoutPanel panelButtons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(5)
                };

                Button btnConsulter = new Button
                {
                    Text = "Consulter",
                    Width = 100,
                    Height = 35,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Width = 100,
                    Height = 35
                };

                btnConsulter.Click += (s, ev) =>
                {
                    int idSeance = (int)nudIdSeance.Value;

                    try
                    {
                        var inscrits = _evRepo.GetInscritsPourSeance(idSeance);

                        if (inscrits.Count == 0)
                        {
                            MessageBox.Show($"Aucun inscrit trouvé pour la séance #{idSeance}.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        GridResult.DataSource = inscrits.Select(i => new
                        {
                            ID_User = i.IdUser,
                            Nom = i.User?.Nom,
                            Prénom = i.User?.Prenom,
                            Email = i.User?.Email,
                            Date_Inscription = i.DateInsc.ToString("dd/MM/yyyy HH:mm"),
                            Présent = i.Present ? "Oui" : "Non"
                        }).ToList();

                        lblDerniereRequete.Text = $"Inscrits pour la séance #{idSeance}";
                        tabControl.SelectedIndex = 0;
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors du chargement des inscrits :\n{ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnAnnuler.Click += (s, ev) => dialog.Close();

                panelButtons.Controls.Add(btnConsulter);
                panelButtons.Controls.Add(btnAnnuler);

                layout.Controls.Add(lblInstruction, 0, 0);
                layout.Controls.Add(nudIdSeance, 0, 1);
                layout.Controls.Add(panelButtons, 0, 2);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void ExecuterRequetePersonnalisee(string query)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    GridResult.DataSource = dt;

                    string queryPreview = query.Length > 50 ? query.Substring(0, 50) + "..." : query;
                    lblDerniereRequete.Text = $"Requête personnalisée : {queryPreview}";
                    tabControl.SelectedIndex = 0;
                }
            }
        }

        private void BtnDeconnexion_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", "Déconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
                loginForm.FormClosed += (s, args) => this.Close();
            }
        }
    }
}