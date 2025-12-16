using Salle_Sport.Data.Repositories;
using Salle_Sport.Models;

namespace Salle_Sport.Forms.Dashboards
{
    public class AP_Dashb : Form
    {
        private User _currentUser;
        private AP_Rep APRep;
        private Label lblWelcome;
        private TableLayoutPanel MainLayout;

        /// <summary>Contrôle à onglets permettant de naviguer entre différentes vues (dossiers en attente, tous les membres, absents)</summary>
        private TabControl tabControl;
        /// <summary>Onglets représentant respectivement : les dossiers en attente de validation, la liste complète des membres, et les membres absents</summary>
        private TabPage tabDossiersAttente, tabTousMembres, tabMembresAbsents;
        /// <summary>Grilles de données affichant respectivement : les dossiers en attente, tous les membres, et les membres absents</summary>
        private DataGridView GridDossiersAttente, GridTousMembres, GridMbAbsents;

        /// <summary>Liste déroulante permettant de filtrer les membres par statut (ACTIF, EN_ATTENTE, REFUSE, BANNI)</summary>
        private ComboBox CbBoxFiltreStatut;
        private Button btnValider, btnRefuser, btnActualiserAttente, btnBannir, btnDeconnexion, btnActualiserTous, btnActualiserAbsents, btnBannirAbsent;

        public AP_Dashb(User user)
        {
            _currentUser = user;
            APRep = new AP_Rep();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = $"Espace Approbateur - {_currentUser.Prenom} {_currentUser.Nom}";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            MainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(15)
            };

            MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            lblWelcome = new Label
            {
                Text = $"Espace Approbateur - {_currentUser.Prenom}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
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

            tabDossiersAttente = new TabPage("Dossiers en attente");
            tabTousMembres = new TabPage("Tous les membres");
            tabMembresAbsents = new TabPage("Membres absents");

            tabControl.TabPages.Add(tabDossiersAttente);
            tabControl.TabPages.Add(tabTousMembres);
            tabControl.TabPages.Add(tabMembresAbsents);

            MainLayout.Controls.Add(lblWelcome, 0, 0);
            MainLayout.Controls.Add(tabControl, 0, 1);
            MainLayout.Controls.Add(btnDeconnexion, 1, 0);
            MainLayout.SetColumnSpan(tabControl, 2);

            InitializeTabDossiersEnAttente();
            InitializeTabTousMembres();
            InitializeTabMembresAbsents();

            this.Controls.Add(MainLayout);
        }

        private void InitializeTabDossiersEnAttente()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            GridDossiersAttente = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.None
            };

            FlowLayoutPanel flowButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnValider = new Button
            {
                Text = "Valider ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnValider.Click += BtnValider_Click;

            btnRefuser = new Button
            {
                Text = "Refuser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefuser.Click += BtnRefuser_Click;

            btnActualiserAttente = new Button
            {
                Text = "Actualiser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActualiserAttente.Click += (s, e) => LoadDossiersEnAttente();

            flowButtons.Controls.Add(btnValider);
            flowButtons.Controls.Add(btnRefuser);
            flowButtons.Controls.Add(btnActualiserAttente);

            layout.Controls.Add(GridDossiersAttente, 0, 0);
            layout.Controls.Add(flowButtons, 0, 1);

            tabDossiersAttente.Controls.Add(layout);
        }

        private void InitializeTabTousMembres()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            FlowLayoutPanel flowFiltre = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            Label lblFiltre = new Label
            {
                Text = "Filtrer par statut :",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Padding = new Padding(0, 8, 10, 0)
            };
            // Liste déroulante permettant de filtrer l'affichage des membres par leur statut            
            CbBoxFiltreStatut = new ComboBox
            {
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            CbBoxFiltreStatut.Items.AddRange(new object[] { "Tous", "EN_ATTENTE", "ACTIF", "REFUSE", "BAN", "QUIT" });
            CbBoxFiltreStatut.SelectedIndex = 0;
            CbBoxFiltreStatut.SelectedIndexChanged += (s, e) => FiltrerParStatut();

            flowFiltre.Controls.Add(lblFiltre);
            flowFiltre.Controls.Add(CbBoxFiltreStatut);

            GridTousMembres = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.None
            };

            // Panneau horizontal contenant les boutons d'actions pour les membres (Bannir, Actualiser)
            FlowLayoutPanel flowButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnBannir = new Button
            {
                Text = "Bannir ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBannir.Click += BtnBannir_Click;

            btnActualiserTous = new Button
            {
                Text = "Actualiser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActualiserTous.Click += (s, e) => LoadTousMembres();

            flowButtons.Controls.Add(btnBannir);
            flowButtons.Controls.Add(btnActualiserTous);

            layout.Controls.Add(flowFiltre, 0, 0);
            layout.Controls.Add(GridTousMembres, 0, 1);
            layout.Controls.Add(flowButtons, 0, 2);

            tabTousMembres.Controls.Add(layout);
        }

        private void InitializeTabMembresAbsents()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            GridMbAbsents = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
            };

            FlowLayoutPanel flowButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnBannirAbsent = new Button
            {
                Text = "Bannir ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBannirAbsent.Click += BtnBannirAbsent_Click;

            btnActualiserAbsents = new Button
            {
                Text = "Actualiser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActualiserAbsents.Click += (s, e) => LoadMembresAbsents();

            flowButtons.Controls.Add(btnBannirAbsent);
            flowButtons.Controls.Add(btnActualiserAbsents);

            layout.Controls.Add(GridMbAbsents, 0, 0);
            layout.Controls.Add(flowButtons, 0, 1);

            tabMembresAbsents.Controls.Add(layout);
        }

        private void LoadData()
        {
            LoadDossiersEnAttente();
            LoadTousMembres();
            LoadMembresAbsents();
        }

        private void LoadDossiersEnAttente()
        {
            try
            {
                var dossiers = APRep.GetDossiersEnAttente();

                GridDossiersAttente.DataSource = null;
                GridDossiersAttente.DataSource = dossiers.Select(d => new
                {
                    ID = d.IdUser,
                    Nom = d.User?.Nom,
                    Prénom = d.User?.Prenom,
                    Email = d.User?.Email,
                    Date_Création = d.DateCreatDossier.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

                if (GridDossiersAttente.Columns["ID"] != null)
                {
                    GridDossiersAttente.Columns["ID"].Visible = false;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des dossiers :\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadTousMembres()
        {
            try
            {
                var membres = APRep.GetTousMembres();

                GridTousMembres.DataSource = null;
                GridTousMembres.DataSource = membres.Select(m => new
                {
                    ID = m.IdUser,
                    Nom = m.User?.Nom,
                    Prénom = m.User?.Prenom,
                    Email = m.User?.Email,
                    Statut = m.Statut,
                    Date_Création = m.DateCreatDossier.ToString("dd/MM/yyyy"),
                    Date_Validation = m.DateValidAdmin?.ToString("dd/MM/yyyy") ?? "-",
                    Motif_Ban = m.MotifBan ?? "-"
                }).ToList();

                if (GridTousMembres.Columns["ID"] != null)
                    GridTousMembres.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des membres :\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadMembresAbsents()
        {
            try
            {
                var absents = APRep.GetMembresAbsents();

                GridMbAbsents.DataSource = null;
                GridMbAbsents.DataSource = absents.Select(u => new
                {
                    ID = u.Id,
                    Nom = u.Nom,
                    Prénom = u.Prenom,
                    Email = u.Email,
                    Absences = "≥ 3"
                }).ToList();

                if (GridMbAbsents.Columns["ID"] != null)
                {
                    GridMbAbsents.Columns["ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des membres absents :\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FiltrerParStatut()
        {
            string filtre = CbBoxFiltreStatut.SelectedItem?.ToString() ?? "Tous";

            try
            {
                var membres = APRep.GetTousMembres();

                if (filtre != "Tous")
                {
                    membres = membres.Where(m => m.Statut == filtre).ToList();
                }

                GridTousMembres.DataSource = null;
                GridTousMembres.DataSource = membres.Select(m => new
                {
                    ID = m.IdUser,
                    Nom = m.User?.Nom,
                    Prénom = m.User?.Prenom,
                    Email = m.User?.Email,
                    Statut = m.Statut,
                    Date_Création = m.DateCreatDossier.ToString("dd/MM/yyyy"),
                    Date_Validation = m.DateValidAdmin?.ToString("dd/MM/yyyy") ?? "-",
                    Motif_Ban = m.MotifBan ?? "-"
                }).ToList();

                if (GridTousMembres.Columns["ID"] != null)
                {
                    GridTousMembres.Columns["ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du filtrage :\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnValider_Click(object? sender, EventArgs e)
        {
            if (GridDossiersAttente.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un dossier à valider.",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int idUser = Convert.ToInt32(GridDossiersAttente.SelectedRows[0].Cells["ID"].Value);

            var result = MessageBox.Show("Voulez-vous valider ce dossier ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = APRep.ValiderDossier(idUser, _currentUser.Id);

                    if (success)
                    {
                        MessageBox.Show("Dossier validé avec succès !",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LoadDossiersEnAttente();
                        LoadTousMembres();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la validation.",
                            "Erreur",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur technique :\n{ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefuser_Click(object? sender, EventArgs e)
        {
            if (GridDossiersAttente.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un dossier à refuser.",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int idUser = Convert.ToInt32(GridDossiersAttente.SelectedRows[0].Cells["ID"].Value);

            var result = MessageBox.Show("Voulez-vous refuser ce dossier ?",
                "Confirmation", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = APRep.RefuserDossier(idUser);

                    if (success)
                    {
                        MessageBox.Show("Dossier refusé.",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LoadDossiersEnAttente();
                        LoadTousMembres();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors du refus.",
                            "Erreur",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur technique :\n{ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void BtnBannir_Click(object? sender, EventArgs e)
        {
            if (GridTousMembres.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un membre à bannir.",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int idUser = Convert.ToInt32(GridTousMembres.SelectedRows[0].Cells["ID"].Value);
            string statut = GridTousMembres.SelectedRows[0].Cells["Statut"].Value.ToString();

            if (statut == "BAN")
            {
                MessageBox.Show("Ce membre est déjà banni.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form dialogBan = new Form
            {
                Text = "Bannir le membre",
                Size = new Size(450, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblMotif = new Label
            {
                Text = "Motif du bannissement :",
                Location = new Point(20, 20),
                AutoSize = true
            };

            TextBox txtMotif = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(390, 80),
                Multiline = true,
                PlaceholderText = "Entrez le motif..."
            };

            Button btnConfirmer = new Button
            {
                Text = "Confirmer",
                Location = new Point(20, 150),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConfirmer.FlatAppearance.BorderSize = 0;

            Button btnAnnuler = new Button
            {
                Text = "Annuler",
                Location = new Point(260, 150),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAnnuler.FlatAppearance.BorderSize = 0;

            btnConfirmer.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtMotif.Text))
                {
                    MessageBox.Show("Le motif est obligatoire.",
                        "Attention",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = APRep.BannirMembre(idUser, txtMotif.Text.Trim());

                    if (success)
                    {
                        MessageBox.Show("Membre banni avec succès.",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LoadTousMembres();
                        dialogBan.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors du bannissement.",
                            "Erreur",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur technique :\n{ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            btnAnnuler.Click += (s, args) => dialogBan.Close();

            dialogBan.Controls.Add(lblMotif);
            dialogBan.Controls.Add(txtMotif);
            dialogBan.Controls.Add(btnConfirmer);
            dialogBan.Controls.Add(btnAnnuler);

            dialogBan.ShowDialog();
        }

        private void BtnBannirAbsent_Click(object? sender, EventArgs e)
        {
            if (GridMbAbsents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un membre à bannir.",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int idUser = Convert.ToInt32(GridMbAbsents.SelectedRows[0].Cells["ID"].Value);

            Form dialogBan = new Form
            {
                Text = "Bannir le membre",
                Size = new Size(450, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblMotif = new Label
            {
                Text = "Motif du bannissement (absentéisme) :",
                Location = new Point(20, 20),
                AutoSize = true
            };

            TextBox txtMotif = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(390, 80),
                Multiline = true,
                Text = "Absentéisme répété (≥3 absences)",
                PlaceholderText = "Entrez le motif..."
            };

            Button btnConfirmer = new Button
            {
                Text = "Confirmer",
                Location = new Point(20, 150),
                Size = new Size(150, 35),
            };

            Button btnAnnuler = new Button
            {
                Text = "Annuler",
                Location = new Point(260, 150),
                Size = new Size(150, 35),
            };

            btnConfirmer.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtMotif.Text))
                {
                    MessageBox.Show("Le motif est obligatoire.",
                        "Attention",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = APRep.BannirMembre(idUser, txtMotif.Text.Trim());

                    if (success)
                    {
                        MessageBox.Show("Membre banni avec succès.",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LoadMembresAbsents();
                        LoadTousMembres();
                        dialogBan.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors du bannissement.",
                            "Erreur",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur technique :\n{ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            btnAnnuler.Click += (s, args) => dialogBan.Close();

            dialogBan.Controls.Add(lblMotif);
            dialogBan.Controls.Add(txtMotif);
            dialogBan.Controls.Add(btnConfirmer);
            dialogBan.Controls.Add(btnAnnuler);

            dialogBan.ShowDialog();
        }

        private void BtnDeconnexion_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Voulez-vous vraiment vous déconnecter ?",
                "Déconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
                loginForm.FormClosed += (s, args) => this.Close();
            }
            else
            {
                return;
            }
        }
    }
}