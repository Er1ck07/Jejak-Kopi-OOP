// ===========================================================================
//  OOPDemo.cs — Demonstrasi lengkap 4 pilar OOP pada proyek Jejak Kopi
//
//  Cara pakai: panggil OOPDemo.RunAll() dari tombol di form manapun.
//  Contoh di Dashboard Admin:
//    private void btnDemo_Click(object sender, EventArgs e) => OOPDemo.RunAll();
// ===========================================================================
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Jejak_Kopi.Models;
using Jejak_Kopi.Services;

namespace Jejak_Kopi
{
    public static class OOPDemo
    {
        public static void RunAll()
        {
            string output = "";
            output += "╔══════════════════════════════════════════════════╗\n";
            output += "║       JEJAK KOPI — DEMONSTRASI OOP LENGKAP      ║\n";
            output += "╚══════════════════════════════════════════════════╝\n\n";

            output += Demo_Encapsulation();
            output += Demo_Inheritance();
            output += Demo_Polymorphism();
            output += Demo_Relationship();

            MessageBox.Show(output, "OOP Demo — Jejak Kopi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // -----------------------------------------------------------------------
        //  1. ENCAPSULATION
        //  Field privat, validasi pada setter, password tidak bisa dibaca langsung
        // -----------------------------------------------------------------------
        static string Demo_Encapsulation()
        {
            string s = "━━━ 1. ENCAPSULATION ━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            // Pelanggan tidak bisa dibuat dengan nama kosong (validasi di setter)
            try
            {
                var bad = new Pelanggan(0, "", "usr", "pass123", "08123", "e@mail.com");
                s += "  ✗ Seharusnya error karena nama kosong\n";
            }
            catch (ArgumentException ex)
            {
                s += $"  ✓ Validasi nama berjalan: {ex.Message}\n";
            }

            var pelanggan = new Pelanggan(1, "Budi Santoso", "budi", "rahasia123",
                                          "081234567890", "budi@mail.com");

            // Password tidak bisa dibaca dari luar — hanya bisa diverifikasi
            s += $"  ✓ VerifikasiPassword('salah')     : {pelanggan.VerifikasiPassword("salah")}\n";
            s += $"  ✓ VerifikasiPassword('rahasia123'): {pelanggan.VerifikasiPassword("rahasia123")}\n";

            // Stok BijiKopi tidak bisa dikurangi melebihi stok
            var kopi = new KopiArabika(1, "Gayo", 5.0m, 80_000, "Aceh");
            s += $"  ✓ KurangiStok(10 kg) dari stok 5 kg: {kopi.KurangiStok(10)}\n";
            s += $"  ✓ KurangiStok(3 kg)  dari stok 5 kg: {kopi.KurangiStok(3)}\n";
            s += $"  ✓ Sisa stok: {kopi.Stok} kg\n\n";
            return s;
        }

        // -----------------------------------------------------------------------
        //  2. INHERITANCE
        // -----------------------------------------------------------------------
        static string Demo_Inheritance()
        {
            string s = "━━━ 2. INHERITANCE ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            var admin     = new Admin(10, "Siti Admin", "siti_adm", "admin123", "082", "siti@jejakkopi.id");
            var pelanggan = new Pelanggan(20, "Reza Pembeli", "reza", "pel99", "089", "reza@mail.com");

            s += $"  ✓ Admin mewarisi UserBase — VerifikasiPassword: {admin.VerifikasiPassword("admin123")}\n";
            pelanggan.SoftDelete();
            s += $"  ✓ Pelanggan mewarisi SoftDelete()  — IsDeleted: {pelanggan.IsDeleted}\n";

            var arabika = new KopiArabika(1, "Toraja", 10m, 90_000, "Sulawesi");
            var robusta  = new KopiRobusta(2, "Lampung", 15m, 55_000, kadarKafein: 27);

            s += $"  ✓ {arabika.GetRingkasan()}\n";
            s += $"  ✓ {robusta.GetRingkasan()}\n\n";
            return s;
        }

        // -----------------------------------------------------------------------
        //  3. POLYMORPHISM
        // -----------------------------------------------------------------------
        static string Demo_Polymorphism()
        {
            string s = "━━━ 3. POLYMORPHISM ━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            // Daftar bertipe UserBase — isinya campuran Admin dan Pelanggan
            var users = new List<UserBase>
            {
                new Admin(1, "Admin Utama", "admin", "adm123", "081", "a@b.com"),
                new Pelanggan(2, "Pelanggan A", "user_a", "usr123", "082", "a@c.com"),
                new Pelanggan(3, "Pelanggan B", "user_b", "usr456", "083", "b@c.com"),
            };

            s += "  Satu loop, method berbeda dipanggil otomatis (dynamic dispatch):\n";
            foreach (UserBase u in users)
                s += $"   [{u.GetRole(),9}] {u.Nama,-18} → {u.GetDashboardTitle()}\n";

            // Polimorfisme pada BijiKopi
            var kopiList = new List<BijiKopi>
            {
                new KopiArabika(1, "Gayo", 8m, 85_000, "Aceh"),
                new KopiRobusta(2, "Lampung", 20m, 50_000, 28),
            };
            s += "\n  GetDeskripsi() berbeda per subclass BijiKopi:\n";
            foreach (BijiKopi k in kopiList)
                s += $"   [{k.GetKategori()}] {k.Nama}: {k.GetDeskripsi()}\n";

            // UserFactory — factory polymorphism
            UserBase u1 = UserFactory.Create(5, true,  "New Admin", "na", "pass99", "085", "na@mail.com");
            UserBase u2 = UserFactory.Create(6, false, "New User",  "nu", "pass88", "086", "nu@mail.com");
            s += $"\n  UserFactory.Create:\n";
            s += $"   u1 → {u1.GetRole()}: {u1.GetDashboardTitle()}\n";
            s += $"   u2 → {u2.GetRole()}: {u2.GetDashboardTitle()}\n\n";
            return s;
        }

        // -----------------------------------------------------------------------
        //  4. RELATIONSHIP
        // -----------------------------------------------------------------------
        static string Demo_Relationship()
        {
            string s = "━━━ 4. RELATIONSHIP ━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            // COMPOSITION: Keranjang lahir dan mati bersama Pelanggan
            var andi = new Pelanggan(7, "Andi Wijaya", "andi", "andi123", "087", "andi@mail.com");
            andi.Keranjang.TambahItem(new ItemKeranjang(1, 101, "Gayo",    "Arabika", 85_000, 2));
            andi.Keranjang.TambahItem(new ItemKeranjang(2, 102, "Lampung", "Robusta", 50_000, 3));
            s += $"  ✓ Composition — {andi.Keranjang}\n";

            // AGGREGATION: Pesanan → ItemPesanan
            var items = new List<ItemPesanan>
            {
                new ItemPesanan(101, "Gayo",    85_000, 2),
                new ItemPesanan(102, "Lampung", 50_000, 3),
            };
            var pesanan = new PesananModel(1001, andi.Id, "Transfer Bank", items);
            s += $"  ✓ Aggregation — {pesanan}\n";
            s += $"    Status awal      : {pesanan.GetStatusLabel()}\n";
            pesanan.UbahStatus(StatusPesanan.Dikirim);
            s += $"    Setelah dikirim  : {pesanan.GetStatusLabel()}\n";
            pesanan.UbahStatus(StatusPesanan.Selesai);
            s += $"    Setelah selesai  : {pesanan.GetStatusLabel()}\n";
            bool cobaUbah = pesanan.UbahStatus(StatusPesanan.Dibatalkan);
            s += $"    Coba batalkan    : {cobaUbah} (seharusnya false — sudah selesai)\n";

            andi.TambahRiwayatPesanan(pesanan);
            s += $"    Total belanja Andi: Rp {andi.GetTotalBelanja():N0}\n";

            // DEPENDENCY: LaporanService bergantung pada List<PesananModel> yang diinjeksi
            var laporan = new LaporanService(new List<PesananModel> { pesanan });
            s += $"\n  ✓ Dependency — LaporanService\n";
            s += $"    Pendapatan selesai : Rp {laporan.GetTotalPendapatanSelesai():N0}\n";
            s += $"    Jumlah selesai     : {laporan.GetJumlahPesananPerStatus(StatusPesanan.Selesai)}\n";
            s += $"    Pesanan terbesar   : {laporan.GetPesananTerbesar()}\n\n";

            s += "╔══════════════════════════════════════════════════╗\n";
            s += "║  DEMO SELESAI — Semua pilar OOP terdemonstrasikan║\n";
            s += "╚══════════════════════════════════════════════════╝";
            return s;
        }
    }
}
