using System;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  ENCAPSULATION + INHERITANCE (base class)
    //  Semua field privat, diakses hanya lewat property dengan validasi.
    //  Admin dan Pelanggan mewarisi kelas ini.
    // =========================================================
    public abstract class UserBase
    {
        // --- private backing fields (ENCAPSULATION) ---
        private int    _id;
        private string _nama;
        private string _username;
        private string _password;
        private string _noTelepon;
        private string _email;
        private bool   _isDeleted;

        // --- public properties dengan validasi ---
        public int Id
        {
            get => _id;
            protected set
            {
                if (value < 0) throw new ArgumentException("Id tidak boleh negatif.");
                _id = value;
            }
        }

        public string Nama
        {
            get => _nama;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nama tidak boleh kosong.");
                _nama = value.Trim();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username tidak boleh kosong.");
                _username = value.Trim();
            }
        }

        // Password tidak memiliki public getter — hanya bisa diverifikasi via VerifikasiPassword()
        public string Password
        {
            protected get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password tidak boleh kosong.");
                _password = value;
            }
        }

        public string NoTelepon
        {
            get => _noTelepon;
            set => _noTelepon = value?.Trim() ?? string.Empty;
        }

        public string Email
        {
            get => _email;
            set => _email = value?.Trim() ?? string.Empty;
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            protected set => _isDeleted = value;
        }

        // --- Constructor ---
        protected UserBase(int id, string nama, string username, string password,
                           string noTelepon, string email, bool isDeleted = false)
        {
            Id        = id;
            Nama      = nama;
            Username  = username;
            Password  = password;
            NoTelepon = noTelepon;
            Email     = email;
            IsDeleted = isDeleted;
        }

        protected UserBase() { }

        // --- Metode abstrak yang wajib di-override oleh subclass (POLYMORPHISM) ---
        public abstract string GetRole();
        public abstract string GetDashboardTitle();
        public abstract string[] GetMenuItems();

        // --- Metode konkret yang bisa di-override ---
        public virtual string GetInfo()
        {
            return $"[{GetRole()}] {Nama} (@{Username}) | {Email}";
        }

        // Verifikasi password tanpa mengekspos nilainya ke luar
        public bool VerifikasiPassword(string inputPassword)
        {
            return _password == inputPassword;
        }

        public void SoftDelete()
        {
            _isDeleted = true;
        }

        public override string ToString() => GetInfo();
    }
}
