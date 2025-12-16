using Salle_Sport.Data.Repositories;
using Salle_Sport.Models;

namespace Salle_Sport.Forms
{
    public class RegisterForm : Form
    {
        private TableLayoutPanel? MainLayout;
        private Label? lblTitre;
        private Label? lblEmail, lblPwd, lblNom, lblPrenom;
        private TextBox? txtEmail, txtPwd, txtNom, txtPrenom;
         private Label? lblStatus;
        private Button? btnRegister;
        private LinkLabel? lnkLogin;
        private AuthRepository _authRepository;

        public RegisterForm()
        {
            _authRepository = new AuthRepository();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Inscription à la meilleure Salle de Sport de France !! Bienvenue !";
            this.Size = new Size(600,400);
            this.StartPosition = FormStartPosition.CenterScreen;

            MainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(20),
                AutoSize = true
            };

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            for(int i = 0 ; i < MainLayout.RowCount ; i++)
            {
                MainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            lblTitre = new Label
            {
                Text = "Créer un compte :",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 10, 0, 30)
            };

            lblEmail = new Label
            {
                Text = "Email :",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 8, 10, 5)
            };

            txtEmail = new TextBox
            {
                Width = 300,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "exemple@mail.com",
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 5, 0, 5)
            };

            lblPwd = new Label
            {
                Text = "Mot de passe",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 8, 10, 5)
            };

            txtPwd = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "Min. 10 caractère dont 1 caractère spéciale",
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 5, 0, 5)
            };

            lblNom = new Label
            {
                Text = "Nom :",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 8, 10, 5)
            };

            txtNom = new TextBox
            {
                Width = 280,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "Votre nom",
                Font = new Font ("Segoe UI", 10),
                Margin = new Padding(0, 5, 0, 5)
            };

            lblPrenom = new Label
            {
                Text = "Prénom :",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 8, 10, 5)
            };

            txtPrenom = new TextBox
            {
                Width = 280,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "Votre prénom",
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 5, 0, 5)
            };

            btnRegister = new Button
            {
                Text = "S'inscrire",
                Width = 280,
                Height = 40,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Margin = new Padding(0, 15, 0, 10)
            };
            btnRegister.Click += BtnRegister_Click;

            lblStatus = new Label
            {
                AutoSize = true,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None,
                Font = new Font("Segoe UI", 9),
                Margin = new Padding(0, 5, 0, 5)
            };
            
            lnkLogin = new LinkLabel
            {
                Text = "Vous avez déjà un compte ? Connectez-vous içi.",
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Font = new Font("Segoe UI", 9),
                Margin = new Padding(0, 10, 0, 0)
            };
            lnkLogin.Click += LnkLogin_Click;

            MainLayout.Controls.Add(lblTitre, 0, 0);
            MainLayout.SetColumnSpan(lblTitre, 2); // Titre sur 2 colonnes
            MainLayout.Controls.Add(lblEmail, 0, 1);
            MainLayout.Controls.Add(txtEmail, 1, 1);
            MainLayout.Controls.Add(lblPwd, 0, 2);
            MainLayout.Controls.Add(txtPwd, 1, 2);
            MainLayout.Controls.Add(lblNom, 0, 3);
            MainLayout.Controls.Add(txtNom, 1, 3);
            MainLayout.Controls.Add(lblPrenom, 0, 4);
            MainLayout.Controls.Add(txtPrenom, 1, 4);
            MainLayout.Controls.Add(btnRegister, 1, 5);
            MainLayout.Controls.Add(lblStatus, 0, 6);
            MainLayout.SetColumnSpan(lblStatus, 2);
            MainLayout.Controls.Add(lnkLogin, 0, 7);
            MainLayout.SetColumnSpan(lnkLogin, 2);

            this.Controls.Add(MainLayout);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            lblStatus?.Text = "";

            if(string.IsNullOrWhiteSpace(txtEmail?.Text))
            {
                lblStatus?.Text = "L'email est obligatoire.";
                lblStatus?.ForeColor = Color.Red;
                txtEmail?.Focus();
                return;
            }

            if(!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                lblStatus?.Text = "Format d'email invalide.";
                lblStatus?.ForeColor = Color.Red;
                txtEmail?.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtPwd?.Text))
            {
                lblStatus?.Text = "Le mot de passe est obligatoire.";
                lblStatus?.ForeColor = Color.Red;
                txtPwd?.Focus();
                return;
            }

            if(txtPwd.Text.Length < 10 || !txtPwd.Text.Any(char.IsDigit))
            {
                lblStatus?.Text = "Le mot de passe doit contenir au moins 10 caractères dont 1 caractère spéciale.";
                lblStatus?.ForeColor = Color.Red;
                txtPwd?.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtNom?.Text))
            {
                lblStatus?.Text = "Le nom est obligatoire.";
                lblStatus?.ForeColor = Color.Red;
                txtNom?.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtPrenom?.Text))
            {
                lblStatus?.Text = "Le prénom est obligatoire. ";
                lblStatus?.ForeColor = Color.Red;
                txtPrenom?.Focus();
                return;
            };

            User newUser = new User
            {
                Email = txtEmail.Text.Trim(),
                Pwd = txtPwd.Text,
                Nom = txtNom.Text.Trim(),
                Prenom = txtPrenom.Text.Trim(),
                Role = "Mb"
            };

            btnRegister?.Enabled = false;
            btnRegister?.Text = "Inscription en cours...";

            try
            {
                bool success = _authRepository.Register(newUser);

                if(success)
                {
                    lblStatus?.Text = "Inscription Réussie !";
                    lblStatus?.ForeColor = Color.Green;

                    MessageBox.Show(
                        "Votre compte a été créé avec succès !\n\n" +
                        "Statut : EN ATTENTE\n" +
                        "Un administrateur doit valider votre compte avant que vous puissiez accéder aux séances.",
                        "Inscription réussie",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    RetourLoginForm();
                }
            }
            catch(Exception ex)
            {
                lblStatus?.Text = "Erreur technique";
                lblStatus?.ForeColor = Color.Red;
                btnRegister?.Enabled = true;
                btnRegister?.Text = "S'inscrire";

                MessageBox.Show(
                    $"Un erreur est survenue : \n\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void LnkLogin_Click(object? sender, EventArgs e)
        {
            RetourLoginForm();
        }

        private void RetourLoginForm()
        {
            LoginForm Form = new LoginForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }
    }
}