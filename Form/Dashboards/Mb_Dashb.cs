using Salle_Sport.Models;
using Salle_Sport.Data.Repositories;

namespace Salle_Sport.Forms.Dashboards
{
    public class Mb_Dashb : Form
    {
        private User _currentUser;
        private Mb_Rep MbRep;
        
        private Label? lblWelcome, lblStatus;
        private TableLayoutPanel? MainLayout;
        
        /// <summary>Contrôle à onglets permettant de naviguer entre les séances disponibles, les inscriptions du membre et son profil</summary>
        private TabControl? tabControl;
        /// <summary>Onglets représentant respectivement : les séances disponibles à l'inscription, les inscriptions actuelles du membre, et son profil personnel</summary>
        private TabPage? tabSeanceDispo, tabMesSeances, tabMonProfil;
        /// <summary>Grilles de données affichant respectivement : les séances disponibles et les inscriptions du membre</summary>
        private DataGridView? GridSeanceDispo, GridMesInscri;
        
        private Button? btnInscrire, btnActualiserDispo, btnMeDesinscrire, btnActualiserMesInscri, btnDeconnexion;
        public Mb_Dashb(User user)
        {
            _currentUser = user;
            MbRep = new Mb_Rep();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = $"Espace Membre - {_currentUser.Prenom} {_currentUser.Nom}.";
            this.Size = new Size(1000,700);
            this.StartPosition = FormStartPosition.CenterScreen;

            MainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 2,
                Padding = new Padding(15)
            };

            MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            lblWelcome = new Label
            {
                Text = $"Bienvenue, {_currentUser.Prenom} {_currentUser.Nom}!",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblStatus = new Label
            {
                Text = $"Statut : {GetStatutDossier()}",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleRight
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

            tabSeanceDispo = new TabPage("Séances Disponibles");
            tabMesSeances = new TabPage("Mes Inscriptions");
            tabMonProfil = new TabPage("Mon Profil");

            tabControl.TabPages.Add(tabSeanceDispo);
            tabControl.TabPages.Add(tabMesSeances);
            tabControl.TabPages.Add(tabMonProfil);

            MainLayout.Controls.Add(lblWelcome, 0, 0);
            MainLayout.Controls.Add(btnDeconnexion, 1, 0);
            MainLayout.Controls.Add(lblStatus, 1, 1);
            MainLayout.Controls.Add(tabControl, 0, 1);
            MainLayout.SetColumnSpan(tabControl, 2);

            InitializeTabSeancesDispo();
            InitializeTabMesInscriptions();
            InitializeTabMonProfil();

            this.Controls.Add(MainLayout);
        }

        private void InitializeTabSeancesDispo()
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

            GridSeanceDispo = new DataGridView
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

            btnInscrire = new Button
            {
                Text = "S'inscrire ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnInscrire.Click += BtnInscrire_Click;

            btnActualiserDispo = new Button
            {
                Text = "Actualiser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActualiserDispo.Click += (s, e) => LoadSeancesDispo();

            flowButtons.Controls.Add(btnInscrire);
            flowButtons.Controls.Add(btnActualiserDispo);

            layout.Controls.Add(GridSeanceDispo, 0, 0);
            layout.Controls.Add(flowButtons, 0, 1);

            tabSeanceDispo?.Controls.Add(layout);
        }

        private void InitializeTabMesInscriptions()
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

            GridMesInscri = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.None
            };

            FlowLayoutPanel flowButton = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnMeDesinscrire = new Button
            {
                Text = "Se désinscrire ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnMeDesinscrire.Click += BtnMeDesinscrire_Click;

            btnActualiserMesInscri = new Button
            {
                Text = "Actualiser ?",
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActualiserMesInscri.Click += (s, e) => LoadMesInscriptions();

            flowButton.Controls.Add(btnMeDesinscrire);
            flowButton.Controls.Add(btnActualiserMesInscri);

            layout.Controls.Add(GridMesInscri, 0, 0);
            layout.Controls.Add(flowButton, 0, 1);

            tabMesSeances?.Controls.Add(layout);            
        }

        private void InitializeTabMonProfil()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 8,
                ColumnCount = 2,
                Padding = new Padding(30)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            Label lblNom = new Label { Text = "Nom :", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtNom = new TextBox { Dock = DockStyle.Fill, Text = _currentUser.Nom, Font = new Font("Segoe UI", 10F) };

            Label lblPrenom = new Label { Text = "Prénom :", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtPrenom = new TextBox { Dock = DockStyle.Fill, Text = _currentUser.Prenom, Font = new Font("Segoe UI", 10F) };

            Label lblEmail = new Label { Text = "Email :", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtEmail = new TextBox { Dock = DockStyle.Fill, Text = _currentUser.Email, Font = new Font("Segoe UI", 10F) };

            Label lblMdp = new Label { Text = "Mot de passe :", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            TextBox txtMdp = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 10F) };

            Button btnModifier = new Button
            {
                Text = "Modifier mes informations",
                Dock = DockStyle.Fill,
                Height = 40,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnModifier.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) || 
                    string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtMdp.Text))
                {
                    MessageBox.Show("Tous les champs sont obligatoires.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Voulez-vous vraiment modifier vos informations ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bool success = MbRep.ModifierProfil(_currentUser.Id, txtNom.Text, txtPrenom.Text, txtEmail.Text, txtMdp.Text);
                        if (success)
                        {
                            _currentUser.Nom = txtNom.Text;
                            _currentUser.Prenom = txtPrenom.Text;
                            _currentUser.Email = txtEmail.Text;
                            _currentUser.Pwd = txtMdp.Text;
                            lblWelcome?.Text = $"Bienvenue, {_currentUser.Prenom} {_currentUser.Nom}!";
                            this.Text = $"Espace Membre - {_currentUser.Prenom} {_currentUser.Nom}.";
                            MessageBox.Show("Vos informations ont été modifiées avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMdp.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur technique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            Panel separator = new Panel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.Fixed3D, Height = 2 };

            Button btnQuitter = new Button
            {
                Text = "Je quitte la salle",
                Width = 300,
                Height = 50,
                Anchor = AnchorStyles.None,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White
            };
            btnQuitter.Click += (s, e) =>
            {
                Form dialogConfirm = new Form
                {
                    Text = "Confirmation",
                    Size = new Size(500, 250),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                TableLayoutPanel dialogLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 3,
                    ColumnCount = 1,
                    Padding = new Padding(15)
                };
                dialogLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
                dialogLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                dialogLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                Label lblWarning = new Label
                {
                    Text = "ATTENTION",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.Red,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label lblMessage = new Label
                {
                    Text = "Êtes-vous sûr de vouloir quitter la salle ?\n\nCette action est irréversible.\n\n" +
                           "Votre dossier sera marqué comme 'QUIT' et vos inscriptions futures seront supprimées.",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10F),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                FlowLayoutPanel panelButtons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(5)
                };

                Button btnConfirmer = new Button
                {
                    Text = "Confirmer",
                    Width = 120,
                    Height = 40,
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Width = 120,
                    Height = 40,
                    Font = new Font("Segoe UI", 10F)
                };

                btnConfirmer.Click += (sender, args) =>
                {
                    try
                    {
                        bool success = MbRep.JeQuitteLaSalle(_currentUser.Id);
                        if (success)
                        {
                            MessageBox.Show("Vous avez quitté la salle. Votre dossier a été mis à jour.\n\nVous allez être déconnecté.", 
                                          "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dialogConfirm.Close();
                            BtnDeconnexion_Click(null, EventArgs.Empty);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la procédure.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur technique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnAnnuler.Click += (sender, args) => dialogConfirm.Close();

                panelButtons.Controls.Add(btnConfirmer);
                panelButtons.Controls.Add(btnAnnuler);

                dialogLayout.Controls.Add(lblWarning, 0, 0);
                dialogLayout.Controls.Add(lblMessage, 0, 1);
                dialogLayout.Controls.Add(panelButtons, 0, 2);

                dialogConfirm.Controls.Add(dialogLayout);
                dialogConfirm.ShowDialog(this);
            };

            layout.Controls.Add(lblNom, 0, 0);
            layout.Controls.Add(txtNom, 1, 0);
            layout.Controls.Add(lblPrenom, 0, 1);
            layout.Controls.Add(txtPrenom, 1, 1);
            layout.Controls.Add(lblEmail, 0, 2);
            layout.Controls.Add(txtEmail, 1, 2);
            layout.Controls.Add(lblMdp, 0, 3);
            layout.Controls.Add(txtMdp, 1, 3);
            layout.SetColumnSpan(btnModifier, 2);
            layout.Controls.Add(btnModifier, 0, 4);
            // Row 5 = espace vide (Percent 100F)
            layout.SetColumnSpan(separator, 2);
            layout.Controls.Add(separator, 0, 6);
            layout.SetColumnSpan(btnQuitter, 2);
            layout.Controls.Add(btnQuitter, 0, 7);

            tabMonProfil?.Controls.Add(layout);
        }

        private void LoadData()
        {
            LoadSeancesDispo();
            LoadMesInscriptions();
        }

        private void LoadSeancesDispo()
        {
            try
            {
                var seances = MbRep.GetSeanceDispo();

                GridSeanceDispo?.DataSource = null;
                GridSeanceDispo?.DataSource = seances.Select( s => new
                {
                    Id = s.Id,
                    Date = s.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                    Ade = s.NomAde,
                    Coach = s.NomCoach,
                    Durée = $"{s.Duree} min",
                    Places = s.Cap_Max
                }).ToList();

                if(GridSeanceDispo?.Columns["Id"] != null)
                {
                    GridSeanceDispo?.Columns["Id"]?.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show ($"Erreur lors du chargement des séances disponibles : {ex.Message}",
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
            }
        }

        private void LoadMesInscriptions()
        {
            try
            {
                var inscription = MbRep.GetMyInscri_Seances(_currentUser.Id);

                GridMesInscri?.DataSource = null;
                GridMesInscri?.DataSource = inscription.Select( i => new
                {
                    Id_Seance = i.IdSeance,
                    Date = i.Seance?.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                    Ade = i.Seance?.NomAde,
                    Coach = i.Seance?.NomCoach,
                    Durée =$"{i.Seance?.Duree} min",
                    Inscrit_le = i.DateInsc.ToString("dd/MM/yyyy HH:mm"),
                    Présent = i.Present ? "Oui" : "Non"
                }).ToList();

                if(GridMesInscri?.Columns["Id_Seance"] != null)
                {
                    GridMesInscri?.Columns["Id_Seance"]?.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show ($"Erreur lors du chargement de vos inscriptions : {ex.Message}",
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
            }
        }

        private void BtnInscrire_Click(object? sender, EventArgs e)
        {
            if(GridSeanceDispo?.SelectedRows.Count == 0)
            {
                MessageBox.Show ("Veuillez sélectionner une séance pour vous y inscrire.",
                "Attention",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            int idSeance = Convert.ToInt32(GridSeanceDispo?.SelectedRows[0].Cells["Id"].Value);

            var result = MessageBox.Show ($"Veuillez confirmer votre inscription à la séance sélectionnée.",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                try
                {
                    bool success = MbRep.Inscri_Seance(_currentUser.Id, idSeance);
                    
                    if(success)
                    {
                        MessageBox.Show ("Inscription réussie !",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMesInscriptions();
                    }
                    else
                    {
                        MessageBox.Show ("Erreur lors de l'inscription. Vérifier que :\n\n Votre compte est ACTIF\nVous n'êtes pas déjà inscrit à la séance",
                            "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show ($"Erreur technique : {ex.Message}",
                        "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnMeDesinscrire_Click(object? sender, EventArgs e)
        {
            if(GridMesInscri?.SelectedRows.Count == 0)
            {
                MessageBox.Show ("Veuillez sélectionner une séance pour vous être désinscrit.",
                "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            int idSeance = Convert.ToInt32(GridMesInscri?.SelectedRows[0].Cells["Id_Seance"].Value);

            var result = MessageBox.Show ($"Veuillez confirmer votre désinscription à la séance sélectionnée.",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                try
                {
                    bool success = MbRep.Desinscrire_Seance(_currentUser.Id, idSeance);
            
                    if(success)
                    {
                        MessageBox.Show("Désinscription réussie !",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMesInscriptions();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la désinscription.",
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Erreur technique : {ex.Message}",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeconnexion_Click(object? sender, EventArgs e)
        {
            // Retour au formulaire de connexion
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
            loginForm.FormClosed += (s, args) => this.Close();
        }

        private string GetStatutDossier()
        {
            return MbRep.GetStatutDossier(_currentUser.Id);
        }
    }
}