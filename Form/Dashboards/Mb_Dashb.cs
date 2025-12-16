using Salle_Sport.Models;
using Salle_Sport.Data.Repositories;

namespace Salle_Sport.Forms.Dashboards
{
    public class Mb_Dashb : Form
    {
        private User _currentUser;
        private Mb_Rep MbRep;
        
        private Label lblWelcome, lblStatus;
        private TableLayoutPanel MainLayout;
        
        private TabControl tabControl;
        private TabPage tabSeanceDispo, tabMesSeances;
        private DataGridView GridSeanceDispo, GridMesInscri;
        
        private Button btnInscrire, btnActualiserDispo, btnMeDesinscrire, btnActualiserMesInscri, btnDeconnexion;

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

            tabControl.TabPages.Add(tabSeanceDispo);
            tabControl.TabPages.Add(tabMesSeances);

            MainLayout.Controls.Add(lblWelcome, 0, 0);
            MainLayout.Controls.Add(btnDeconnexion, 1, 0);
            MainLayout.Controls.Add(lblStatus, 1, 1);
            MainLayout.Controls.Add(tabControl, 0, 1);
            MainLayout.SetColumnSpan(tabControl, 2);

            InitializeTabSeancesDispo();
            InitializeTabMesInscriptions();

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

            tabSeanceDispo.Controls.Add(layout);
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

            tabMesSeances.Controls.Add(layout);            
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

                GridSeanceDispo.DataSource = null;
                GridSeanceDispo.DataSource = seances.Select( s => new
                {
                    Id = s.Id,
                    Date = s.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                    Ade = s.NomAde,
                    Coach = s.NomCoach,
                    Durée = $"{s.Duree} min",
                    Places = s.Cap_Max
                }).ToList();

                if(GridSeanceDispo.Columns["Id"] != null)
                {
                    GridSeanceDispo.Columns["Id"].Visible = false;
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

                GridMesInscri.DataSource = null;
                GridMesInscri.DataSource = inscription.Select( i => new
                {
                    Id_Seance = i.IdSeance,
                    Date = i.Seance.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                    Ade = i.Seance.NomAde,
                    Coach = i.Seance.NomCoach,
                    Durée =$"{i.Seance.Duree} min",
                    Inscrit_le = i.DateInsc.ToString("dd/MM/yyyy HH:mm"),
                    Présent = i.Present ? "Oui" : "Non"
                }).ToList();

                if(GridMesInscri.Columns["Id_Seance"] != null)
                {
                    GridMesInscri.Columns["Id_Seance"].Visible = false;
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
            if(GridSeanceDispo.SelectedRows.Count == 0)
            {
                MessageBox.Show ("Veuillez sélectionner une séance pour vous y inscrire.",
                "Attention",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            int idSeance = Convert.ToInt32(GridSeanceDispo.SelectedRows[0].Cells["Id"].Value);

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
            if(GridMesInscri.SelectedRows.Count == 0)
            {
                MessageBox.Show ("Veuillez sélectionner une séance pour vous être désinscrit.",
                "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            int idSeance = Convert.ToInt32(GridMesInscri.SelectedRows[0].Cells["Id_Seance"].Value);

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