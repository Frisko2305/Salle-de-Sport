using Salle_Sport.Models;
using Salle_Sport.Data.Repositories;
using System.Data;

namespace Salle_Sport.Forms.Dashboards
{
    public class AS_Dashb : Form
    {
        private User _currentUser;
        private AsRepository ASRep;
        private Label lblSeanceSelectionnee;
        private TableLayoutPanel mainLayout;
        private TabControl tabControl;
        private DataGridView GridAde, GridCoach, GridSeance, GridInscri;
        private Button btnDeconnexion,btnAjoutAde, btModifAde, btnSuppAde;
        private Button btnAjoutCoach, btnModifCoach, btnSuppCoach;
        private Button btnAjoutSeance, btnModifSeance, btnSuppSeance, btnVoirInscri;
        private Button btnMarquerPST, btnMarquerABS, btnSuppInscri;

        public AS_Dashb(User user)
        {
            _currentUser = user;
            ASRep = new AsRepository();

            this.Text = "Admin Séances - Salle de Sport";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
            LoadData();
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
                Text = $"Espace Admin Séances - {_currentUser.Prenom}",
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
            InitializeTabActivites();
            InitializeTabCoachs();
            InitializeTabSeances();

            mainLayout.Controls.Add(lblHeader, 0, 0);
            mainLayout.Controls.Add(btnDeconnexion, 1, 0);
            mainLayout.Controls.Add(tabControl, 0, 1);
            mainLayout.SetColumnSpan(tabControl, 2);

            this.Controls.Add(mainLayout);
        }

        private void InitializeTabActivites()
        {
            TabPage tabActivites = new TabPage("Gestion Activités");
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            GridAde = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            layout.Controls.Add(GridAde, 0, 0);

            FlowLayoutPanel panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnAjoutAde = new Button { Text = "Ajouter", Width = 120, Height = 35 };
            btnAjoutAde.Click += BtnAjouterActivite_Click;

            btModifAde = new Button { Text = "Modifier", Width = 120, Height = 35 };
            btModifAde.Click += BtnModifierActivite_Click;

            btnSuppAde = new Button { Text = "Supprimer", Width = 120, Height = 35 };
            btnSuppAde.Click += BtnSupprimerActivite_Click;

            panelButtons.Controls.Add(btnAjoutAde);
            panelButtons.Controls.Add(btModifAde);
            panelButtons.Controls.Add(btnSuppAde);

            layout.Controls.Add(panelButtons, 0, 1);
            tabActivites.Controls.Add(layout);
            tabControl.TabPages.Add(tabActivites);
        }

        private void InitializeTabCoachs()
        {
            TabPage tabCoachs = new TabPage("Gestion Coachs");
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            GridCoach = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            FlowLayoutPanel panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5)
            };

            btnAjoutCoach = new Button { Text = "Ajouter", Width = 120, Height = 35 };
            btnAjoutCoach.Click += BtnAjouterCoach_Click;

            btnModifCoach = new Button { Text = "Modifier", Width = 120, Height = 35 };
            btnModifCoach.Click += BtnModifierCoach_Click;

            btnSuppCoach = new Button { Text = "Supprimer", Width = 120, Height = 35 };
            btnSuppCoach.Click += BtnSupprimerCoach_Click;

            panelButtons.Controls.Add(btnAjoutCoach);
            panelButtons.Controls.Add(btnModifCoach);
            panelButtons.Controls.Add(btnSuppCoach);

            layout.Controls.Add(GridCoach, 0, 0);
            layout.Controls.Add(panelButtons, 0, 1);

            tabCoachs.Controls.Add(layout);

            tabControl.TabPages.Add(tabCoachs);
        }

        private void InitializeTabSeances()
        {
            TabPage tabSeances = new TabPage("Gestion Séances");
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            TableLayoutPanel topSection = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            topSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            topSection.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            GridSeance = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            GridSeance.SelectionChanged += DgvSeances_SelectionChanged;
            topSection.Controls.Add(GridSeance, 0, 0);

            FlowLayoutPanel panelButtonsSeances = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnAjoutSeance = new Button { Text = "Ajouter", Width = 120, Height = 35 };
            btnAjoutSeance.Click += BtnAjouterSeance_Click;

            btnModifSeance = new Button { Text = "Modifier", Width = 120, Height = 35 };
            btnModifSeance.Click += BtnModifierSeance_Click;

            btnSuppSeance = new Button { Text = "Supprimer", Width = 120, Height = 35 };
            btnSuppSeance.Click += BtnSupprimerSeance_Click;

            btnVoirInscri = new Button { Text = "Voir inscriptions", Width = 140, Height = 35 };
            btnVoirInscri.Click += BtnVoirInscriptions_Click;

            panelButtonsSeances.Controls.Add(btnAjoutSeance);
            panelButtonsSeances.Controls.Add(btnModifSeance);
            panelButtonsSeances.Controls.Add(btnSuppSeance);
            panelButtonsSeances.Controls.Add(btnVoirInscri);
            
            topSection.Controls.Add(panelButtonsSeances, 0, 1);

            layout.Controls.Add(topSection, 0, 0);

            TableLayoutPanel bottomSection = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            bottomSection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            bottomSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomSection.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            lblSeanceSelectionnee = new Label
            {
                Text = "Sélectionnez une séance pour voir les inscriptions",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            GridInscri = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            FlowLayoutPanel panelButtonsInscriptions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            btnMarquerPST = new Button { Text = "Marquer Présent", Width = 140, Height = 35 };
            btnMarquerPST.Click += BtnMarquerPresent_Click;

            btnMarquerABS = new Button { Text = "Marquer Absent", Width = 140, Height = 35 };
            btnMarquerABS.Click += BtnMarquerAbsent_Click;

            btnSuppInscri = new Button { Text = "Supprimer inscription", Width = 160, Height = 35 };
            btnSuppInscri.Click += BtnSupprimerInscription_Click;

            bottomSection.Controls.Add(lblSeanceSelectionnee, 0, 0);
            bottomSection.Controls.Add(GridInscri, 0, 1);

            panelButtonsInscriptions.Controls.Add(btnMarquerPST);
            panelButtonsInscriptions.Controls.Add(btnMarquerABS);
            panelButtonsInscriptions.Controls.Add(btnSuppInscri);

            bottomSection.Controls.Add(panelButtonsInscriptions, 0, 2);

            layout.Controls.Add(bottomSection, 0, 1);

            tabSeances.Controls.Add(layout);
            tabControl.TabPages.Add(tabSeances);
        }

        private void LoadData()
        {
            LoadActivites();
            LoadCoachs();
            LoadSeances();
        }

        private void LoadActivites()
        {
            var activites = ASRep.GetToutesActivites();
            GridAde.DataSource = activites.Select(a => new
            {
                ID = a.Id,
                Nom = a.NomAde,
                Description = a.Descri
            }).ToList();
        }

        private void LoadCoachs()
        {
            var coachs = ASRep.GetTousCoachs();
            GridCoach.DataSource = coachs.Select(c => new
            {
                ID = c.Id,
                Nom = c.Nom,
                Prénom = c.Prenom,
                Spécialité = c.Specialite
            }).ToList();
        }

        private void LoadSeances()
        {
            var seances = ASRep.GetToutesSeances();
            GridSeance.DataSource = seances.Select(s => new
            {
                ID = s.Id,
                Date = s.DateHDebut.ToString("dd/MM/yyyy HH:mm"),
                Durée = $"{s.Duree} min",
                Capacité = s.Cap_Max,
                Coach = s.NomCoach,
                Activité = s.NomAde
            }).ToList();
        }

        private void LoadInscriptions(int idSeance)
        {
            var inscriptions = ASRep.GetInscriptionsPourSeance(idSeance);
            GridInscri.DataSource = inscriptions.Select(i => new
            {
                IdUser = i.IdUser,
                IdSeance = i.IdSeance,
                Nom = i.User?.Nom,
                Prénom = i.User?.Prenom,
                Email = i.User?.Email,
                Inscription = i.DateInsc.ToString("dd/MM/yyyy HH:mm"),
                Présent = i.Present ? "Oui" : "Non"
            }).ToList();

            lblSeanceSelectionnee.Text = $"Inscriptions pour la séance sélectionnée ({inscriptions.Count} inscrits)";
        }

        private void BtnAjouterActivite_Click(object? sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Ajouter une activité";
                dialog.Size = new Size(400, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 4, ColumnCount = 2 };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                Label lblNom = new Label { Text = "Nom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtNom = new TextBox { Dock = DockStyle.Fill };

                Label lblDescri = new Label { Text = "Description:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft };
                TextBox txtDescri = new TextBox { Dock = DockStyle.Fill, Multiline = true };

                Button btnOk = new Button { Text = "Ajouter", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text))
                    {
                        MessageBox.Show("Le nom est obligatoire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var activite = new Activite { NomAde = txtNom.Text, Descri = txtDescri.Text };
                    if (ASRep.CreerActivite(activite))
                    {
                        MessageBox.Show("Activité ajoutée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadActivites();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblNom, 0, 0);
                layout.Controls.Add(txtNom, 1, 0);
                layout.Controls.Add(lblDescri, 0, 1);
                layout.Controls.Add(txtDescri, 1, 1);
                layout.Controls.Add(btnOk, 1, 2);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnModifierActivite_Click(object? sender, EventArgs e)
        {
            if (GridAde.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une activité.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridAde.SelectedRows[0].Cells["ID"].Value;
            var activite = ASRep.GetToutesActivites().FirstOrDefault(a => a.Id == id);
            if (activite == null) return;

            using (Form dialog = new Form())
            {
                dialog.Text = "Modifier une activité";
                dialog.Size = new Size(400, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 4, ColumnCount = 2 };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                Label lblNom = new Label { Text = "Nom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtNom = new TextBox { Dock = DockStyle.Fill, Text = activite.NomAde };

                Label lblDescri = new Label { Text = "Description:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft };
                TextBox txtDescri = new TextBox { Dock = DockStyle.Fill, Multiline = true, Text = activite.Descri };

                Button btnOk = new Button { Text = "Modifier", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text))
                    {
                        MessageBox.Show("Le nom est obligatoire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    activite.NomAde = txtNom.Text;
                    activite.Descri = txtDescri.Text;
                    if (ASRep.ModifierActivite(activite))
                    {
                        MessageBox.Show("Activité modifiée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadActivites();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblNom, 0, 0);
                layout.Controls.Add(txtNom, 1, 0);
                layout.Controls.Add(lblDescri, 0, 1);
                layout.Controls.Add(txtDescri, 1, 1);
                layout.Controls.Add(btnOk, 1, 2);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnSupprimerActivite_Click(object? sender, EventArgs e)
        {
            if (GridAde.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une activité.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridAde.SelectedRows[0].Cells["ID"].Value;
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette activité ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (ASRep.SupprimerActivite(id))
                {
                    MessageBox.Show("Activité supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadActivites();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression. L'activité est peut-être utilisée par des séances.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAjouterCoach_Click(object? sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Ajouter un coach";
                dialog.Size = new Size(400, 220);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 4, ColumnCount = 2 };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                Label lblNom = new Label { Text = "Nom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtNom = new TextBox { Dock = DockStyle.Fill };

                Label lblPrenom = new Label { Text = "Prénom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtPrenom = new TextBox { Dock = DockStyle.Fill };

                Label lblSpec = new Label { Text = "Spécialité:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtSpec = new TextBox { Dock = DockStyle.Fill };

                Button btnOk = new Button { Text = "Ajouter", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
                    {
                        MessageBox.Show("Nom et prénom sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var coach = new Coach { Nom = txtNom.Text, Prenom = txtPrenom.Text, Specialite = txtSpec.Text };
                    if (ASRep.CreerCoach(coach))
                    {
                        MessageBox.Show("Coach ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCoachs();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblNom, 0, 0);
                layout.Controls.Add(txtNom, 1, 0);
                layout.Controls.Add(lblPrenom, 0, 1);
                layout.Controls.Add(txtPrenom, 1, 1);
                layout.Controls.Add(lblSpec, 0, 2);
                layout.Controls.Add(txtSpec, 1, 2);
                layout.Controls.Add(btnOk, 1, 3);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnModifierCoach_Click(object? sender, EventArgs e)
        {
            if (GridCoach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un coach.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridCoach.SelectedRows[0].Cells["ID"].Value;
            var coach = ASRep.GetTousCoachs().FirstOrDefault(c => c.Id == id);
            if (coach == null) return;

            using (Form dialog = new Form())
            {
                dialog.Text = "Modifier un coach";
                dialog.Size = new Size(400, 220);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 4, ColumnCount = 2 };
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                Label lblNom = new Label { Text = "Nom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtNom = new TextBox { Dock = DockStyle.Fill, Text = coach.Nom };
                layout.Controls.Add(lblNom, 0, 0);
                layout.Controls.Add(txtNom, 1, 0);

                Label lblPrenom = new Label { Text = "Prénom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtPrenom = new TextBox { Dock = DockStyle.Fill, Text = coach.Prenom };

                Label lblSpec = new Label { Text = "Spécialité:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                TextBox txtSpec = new TextBox { Dock = DockStyle.Fill, Text = coach.Specialite };

                Button btnOk = new Button { Text = "Modifier", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
                    {
                        MessageBox.Show("Nom et prénom sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    coach.Nom = txtNom.Text;
                    coach.Prenom = txtPrenom.Text;
                    coach.Specialite = txtSpec.Text;
                    if (ASRep.ModifierCoach(coach))
                    {
                        MessageBox.Show("Coach modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCoachs();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblPrenom, 0, 1);
                layout.Controls.Add(txtPrenom, 1, 1);
                layout.Controls.Add(lblSpec, 0, 2);
                layout.Controls.Add(txtSpec, 1, 2);
                layout.Controls.Add(btnOk, 1, 3);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnSupprimerCoach_Click(object? sender, EventArgs e)
        {
            if (GridCoach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un coach.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridCoach.SelectedRows[0].Cells["ID"].Value;
            string nom = GridCoach.SelectedRows[0].Cells["Nom"].Value?.ToString() ?? "";
            string prenom = GridCoach.SelectedRows[0].Cells["Prénom"].Value?.ToString() ?? "";

            var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le coach {prenom} {nom} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (ASRep.SupprimerCoach(id))
                {
                    MessageBox.Show("Coach supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCoachs();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression. Le coach est peut-être affecté à des séances.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAjouterSeance_Click(object? sender, EventArgs e)
        {
            var activites = ASRep.GetToutesActivites();
            var coachs = ASRep.GetTousCoachs();

            if (activites.Count == 0 || coachs.Count == 0)
            {
                MessageBox.Show("Vous devez d'abord créer au moins une activité et un coach.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form dialog = new Form())
            {
                dialog.Text = "Ajouter une séance";
                dialog.Size = new Size(450, 300);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 6, ColumnCount = 2 };
                for (int i = 0; i < 6; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

                Label lblDate = new Label { Text = "Date/Heure:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                DateTimePicker dtpDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };

                Label lblDuree = new Label { Text = "Durée (min):", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                NumericUpDown nudDuree = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 15, Maximum = 240, Value = 60, Increment = 15 };

                Label lblCap = new Label { Text = "Capacité max:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                NumericUpDown nudCap = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 100, Value = 20 };

                Label lblCoach = new Label { Text = "Coach:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                ComboBox cboCoach = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                cboCoach.DataSource = coachs;
                cboCoach.DisplayMember = "Nom";
                cboCoach.ValueMember = "Id";
                layout.Controls.Add(lblCoach, 0, 3);
                layout.Controls.Add(cboCoach, 1, 3);

                Label lblActivite = new Label { Text = "Activité:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                ComboBox cboActivite = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                cboActivite.DataSource = activites;
                cboActivite.DisplayMember = "NomAde";
                cboActivite.ValueMember = "Id";


                Button btnOk = new Button { Text = "Ajouter", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    var seance = new Seance
                    {
                        DateHDebut = dtpDate.Value,
                        Duree = (int)nudDuree.Value,
                        Cap_Max = (int)nudCap.Value,
                        IdCoach = (int)cboCoach.SelectedValue,
                        IdAde = (int)cboActivite.SelectedValue
                    };

                    if (ASRep.CreerSeance(seance))
                    {
                        MessageBox.Show("Séance ajoutée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSeances();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblDate, 0, 0);
                layout.Controls.Add(dtpDate, 1, 0);
                layout.Controls.Add(lblDuree, 0, 1);
                layout.Controls.Add(nudDuree, 1, 1);
                layout.Controls.Add(lblCap, 0, 2);
                layout.Controls.Add(nudCap, 1, 2);
                layout.Controls.Add(lblActivite, 0, 4);
                layout.Controls.Add(cboActivite, 1, 4);
                layout.Controls.Add(btnOk, 1, 5);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnModifierSeance_Click(object? sender, EventArgs e)
        {
            if (GridSeance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une séance.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridSeance.SelectedRows[0].Cells["ID"].Value;
            var seance = ASRep.GetToutesSeances().FirstOrDefault(s => s.Id == id);
            if (seance == null) return;

            var activites = ASRep.GetToutesActivites();
            var coachs = ASRep.GetTousCoachs();

            using (Form dialog = new Form())
            {
                dialog.Text = "Modifier une séance";
                dialog.Size = new Size(450, 300);
                dialog.StartPosition = FormStartPosition.CenterParent;

                TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 6, ColumnCount = 2 };
                for (int i = 0; i < 6; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

                Label lblDate = new Label { Text = "Date/Heure:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                DateTimePicker dtpDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", Value = seance.DateHDebut };

                Label lblDuree = new Label { Text = "Durée (min):", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                NumericUpDown nudDuree = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 15, Maximum = 240, Value = seance.Duree, Increment = 15 };

                Label lblCap = new Label { Text = "Capacité max:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                NumericUpDown nudCap = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 100, Value = seance.Cap_Max };

                Label lblCoach = new Label { Text = "Coach:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                ComboBox cboCoach = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                cboCoach.DataSource = coachs;
                cboCoach.DisplayMember = "Nom";
                cboCoach.ValueMember = "Id";
                cboCoach.SelectedValue = seance.IdCoach;


                Label lblActivite = new Label { Text = "Activité:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                ComboBox cboActivite = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                cboActivite.DataSource = activites;
                cboActivite.DisplayMember = "NomAde";
                cboActivite.ValueMember = "Id";
                cboActivite.SelectedValue = seance.IdAde;


                Button btnOk = new Button { Text = "Modifier", Dock = DockStyle.Right, Width = 80 };
                btnOk.Click += (s, ev) =>
                {
                    seance.DateHDebut = dtpDate.Value;
                    seance.Duree = (int)nudDuree.Value;
                    seance.Cap_Max = (int)nudCap.Value;
                    seance.IdCoach = (int)cboCoach.SelectedValue;
                    seance.IdAde = (int)cboActivite.SelectedValue;

                    if (ASRep.ModifierSeance(seance))
                    {
                        MessageBox.Show("Séance modifiée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSeances();
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                layout.Controls.Add(lblDate, 0, 0);
                layout.Controls.Add(dtpDate, 1, 0);
                layout.Controls.Add(lblDuree, 0, 1);
                layout.Controls.Add(nudDuree, 1, 1);
                layout.Controls.Add(lblCap, 0, 2);
                layout.Controls.Add(nudCap, 1, 2);
                layout.Controls.Add(lblCoach, 0, 3);
                layout.Controls.Add(cboCoach, 1, 3);
                layout.Controls.Add(lblActivite, 0, 4);
                layout.Controls.Add(cboActivite, 1, 4);
                layout.Controls.Add(btnOk, 1, 5);

                dialog.Controls.Add(layout);
                dialog.ShowDialog(this);
            }
        }

        private void BtnSupprimerSeance_Click(object? sender, EventArgs e)
        {
            if (GridSeance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une séance.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)GridSeance.SelectedRows[0].Cells["ID"].Value;
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette séance ? Toutes les inscriptions seront supprimées.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (ASRep.SupprimerSeance(id))
                {
                    MessageBox.Show("Séance supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSeances();
                    GridInscri.DataSource = null;
                    lblSeanceSelectionnee.Text = "Sélectionnez une séance pour voir les inscriptions";
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvSeances_SelectionChanged(object? sender, EventArgs e)
        {
            if (GridSeance.SelectedRows.Count > 0)
            {
                int idSeance = (int)GridSeance.SelectedRows[0].Cells["ID"].Value;
                LoadInscriptions(idSeance);
            }
        }

        private void BtnVoirInscriptions_Click(object? sender, EventArgs e)
        {
            if (GridSeance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une séance.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idSeance = (int)GridSeance.SelectedRows[0].Cells["ID"].Value;
            LoadInscriptions(idSeance);
        }

        private void BtnMarquerPresent_Click(object? sender, EventArgs e)
        {
            if (GridInscri.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un membre inscrit.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUser = (int)GridInscri.SelectedRows[0].Cells["IdUser"].Value;
            int idSeance = (int)GridInscri.SelectedRows[0].Cells["IdSeance"].Value;

            if (ASRep.MarquerPresence(idUser, idSeance, true))
            {
                MessageBox.Show("Présence marquée.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInscriptions(idSeance);
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarquerAbsent_Click(object? sender, EventArgs e)
        {
            if (GridInscri.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un membre inscrit.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUser = (int)GridInscri.SelectedRows[0].Cells["IdUser"].Value;
            int idSeance = (int)GridInscri.SelectedRows[0].Cells["IdSeance"].Value;

            if (ASRep.MarquerPresence(idUser, idSeance, false))
            {
                MessageBox.Show("Absence marquée.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInscriptions(idSeance);
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSupprimerInscription_Click(object? sender, EventArgs e)
        {
            if (GridInscri.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un membre inscrit.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUser = (int)GridInscri.SelectedRows[0].Cells["IdUser"].Value;
            int idSeance = (int)GridInscri.SelectedRows[0].Cells["IdSeance"].Value;
            string nom = GridInscri.SelectedRows[0].Cells["Nom"].Value?.ToString() ?? "";

            var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer l'inscription de {nom} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (ASRep.SupprimerInscription(idUser, idSeance))
                {
                    MessageBox.Show("Inscription supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInscriptions(idSeance);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeconnexion_Click(object? sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
            loginForm.FormClosed += (s, args) => this.Close();
        }
    }
}