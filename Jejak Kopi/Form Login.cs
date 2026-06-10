using Jejak_Kopi.Database;
using Jejak_Kopi.Models;

namespace Jejak_Kopi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = Login1;
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }

        private void _PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
                e.IsInputKey = true;
        }

        private void Pindah(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                SelectNextControl((Control)sender, true, true, true, true);
            if (e.KeyCode == Keys.Up)
                SelectNextControl((Control)sender, false, true, true, true);
        }

        private void Login1_Click(object sender, EventArgs e)
        {
            string inusern = Username.Text.Trim();
            string inpass  = Password.Text;

            if (string.IsNullOrEmpty(inusern) || string.IsNullOrEmpty(inpass))
            {
                MessageBox.Show("Username dan password harus diisi.", "Peringatan");
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();

            // Ambil user dari database menggunakan AuthenticateUser yang sudah ada
            User legacyUser = dbHelper.AuthenticateUser(inusern, inpass);

            if (legacyUser == null)
            {
                MessageBox.Show("Username atau Password Salah!!", "Login gagal");
                return;
            }

            // ---------------------------------------------------------------
            //  POLYMORPHISM via Factory:
            //  UserFactory menghasilkan UserBase — Admin atau Pelanggan —
            //  tanpa pemanggil perlu tahu tipe konkretnya.
            // ---------------------------------------------------------------
            UserBase currentUser = UserFactory.CreateFromLegacy(legacyUser);

            MessageBox.Show(
                $"Berhasil masuk sebagai {currentUser.GetRole()}!\n{currentUser.GetDashboardTitle()}",
                "Login Sukses"
            );

            // POLYMORPHISM: GetRole() menentukan alur — bukan pemeriksaan is_admin mentah
            switch (currentUser.GetRole())
            {
                case "Admin":
                    FormUtamaAdmin formAdmin = new FormUtamaAdmin(currentUser.Username);
                    formAdmin.Show();
                    this.Hide();
                    break;

                case "Pelanggan":
                    FormUtama formUser = new FormUtama(currentUser.Username);
                    formUser.Show();
                    this.Hide();
                    break;

                default:
                    MessageBox.Show("Role tidak dikenali.", "Error");
                    break;
            }
        }

        private void Register1_Click(object sender, EventArgs e)
        {
            using (Form2 regist = new Form2())
            {
                this.Hide();
                regist.ShowDialog();
            }
        }
    }
}
