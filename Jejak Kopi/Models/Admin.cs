using System;
using System.Collections.Generic;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  INHERITANCE: Admin mewarisi UserBase
    //  POLYMORPHISM: override GetRole, GetDashboardTitle, GetMenuItems, GetInfo
    // =========================================================
    public class Admin : UserBase
    {
        // Properti tambahan khusus Admin (tidak ada di Pelanggan)
        public DateTime TanggalDiangkat { get; private set; }
        public List<string> LogAktivitas { get; } = new List<string>();

        public Admin(int id, string nama, string username, string password,
                     string noTelepon, string email, bool isDeleted = false)
            : base(id, nama, username, password, noTelepon, email, isDeleted)
        {
            TanggalDiangkat = DateTime.Now;
        }

        // Parameterless constructor untuk kompatibilitas pola factory
        public Admin() : base() { }

        // --- POLYMORPHISM: implementasi metode abstrak ---
        public override string GetRole() => "Admin";

        public override string GetDashboardTitle() =>
            $"Dashboard Admin — Selamat datang, {Nama}!";

        public override string[] GetMenuItems() => new[]
        {
            "Dashboard",
            "Data Biji Kopi",
            "Daftar Pelanggan",
            "Pesanan",
            "Laporan"
        };

        // --- POLYMORPHISM: override metode virtual dari base ---
        public override string GetInfo()
        {
            return base.GetInfo() + $" | Diangkat: {TanggalDiangkat:dd MMM yyyy}";
        }

        // --- Perilaku eksklusif Admin ---
        public void TambahLog(string aktivitas)
        {
            LogAktivitas.Add($"[{DateTime.Now:HH:mm:ss}] {aktivitas}");
        }

        public string GetLogTerakhir()
        {
            return LogAktivitas.Count > 0
                ? LogAktivitas[^1]
                : "Belum ada aktivitas tercatat.";
        }
    }
}
