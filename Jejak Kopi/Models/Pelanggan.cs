using System;
using System.Collections.Generic;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  INHERITANCE: Pelanggan mewarisi UserBase
    //  POLYMORPHISM: override GetRole, GetDashboardTitle, GetMenuItems
    //  RELATIONSHIP (Composition): Pelanggan memiliki Keranjang
    // =========================================================
    public class Pelanggan : UserBase
    {
        // --- Properti eksklusif Pelanggan ---
        public string Alamat { get; private set; }

        // RELATIONSHIP — Composition: Pelanggan punya tepat satu Keranjang
        // (Keranjang tidak bisa hidup tanpa Pelanggan)
        public KeranjangModel Keranjang { get; private set; }

        // RELATIONSHIP — Aggregation: Pelanggan bisa punya banyak Pesanan
        private readonly List<PesananModel> _riwayatPesanan = new();
        public IReadOnlyList<PesananModel> RiwayatPesanan => _riwayatPesanan.AsReadOnly();

        public Pelanggan(int id, string nama, string username, string password,
                         string noTelepon, string email, bool isDeleted = false)
            : base(id, nama, username, password, noTelepon, email, isDeleted)
        {
            // Keranjang dibuat otomatis bersama Pelanggan (Composition)
            Keranjang = new KeranjangModel(this);
        }

        public Pelanggan() : base() { }

        // --- POLYMORPHISM: implementasi metode abstrak ---
        public override string GetRole() => "Pelanggan";

        public override string GetDashboardTitle() =>
            $"Selamat datang kembali, {Nama}! ☕";

        public override string[] GetMenuItems() => new[]
        {
            "Dashboard",
            "Katalog Kopi",
            "Keranjang",
            "Riwayat Transaksi"
        };

        // --- POLYMORPHISM: override metode virtual ---
        public override string GetInfo()
        {
            return base.GetInfo() + $" | Pesanan: {_riwayatPesanan.Count}";
        }

        // --- Perilaku eksklusif Pelanggan ---
        public void SetAlamat(string noJalan, string jalan, string kecamatan, string kabupaten)
        {
            if (string.IsNullOrWhiteSpace(jalan))
                throw new ArgumentException("Nama jalan tidak boleh kosong.");
            Alamat = $"{noJalan}, {jalan}, {kecamatan}, {kabupaten}";
        }

        public void TambahRiwayatPesanan(PesananModel pesanan)
        {
            if (pesanan == null) throw new ArgumentNullException(nameof(pesanan));
            _riwayatPesanan.Add(pesanan);
        }

        public decimal GetTotalBelanja()
        {
            decimal total = 0;
            foreach (var p in _riwayatPesanan)
                total += p.TotalHarga;
            return total;
        }
    }
}
