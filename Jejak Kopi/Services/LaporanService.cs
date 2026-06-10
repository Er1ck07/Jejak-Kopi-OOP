using Jejak_Kopi.Interfaces;
using Jejak_Kopi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jejak_Kopi.Services
{
    // =========================================================
    //  POLYMORPHISM via Interface (IReportable)
    //  RELATIONSHIP — Dependency: LaporanService bergantung pada
    //  daftar PesananModel yang diinjeksi dari luar (tidak membuat
    //  sendiri), sehingga mudah diuji dan diganti.
    // =========================================================
    public class LaporanService
    {
        private readonly List<PesananModel> _semuaPesanan;

        public LaporanService(List<PesananModel> pesanan)
        {
            _semuaPesanan = pesanan ?? new List<PesananModel>();
        }

        // Polimorfisme: method ini bisa menerima objek apapun yang
        // mengimplementasi IReportable — Admin, Pelanggan, BijiKopi, dsb.
        public void CetakLaporan(IReportable sumber)
        {
            Console.WriteLine("=== LAPORAN ===");
            Console.WriteLine(sumber.GetRingkasanLaporan());
            foreach (var baris in sumber.GetDetailLaporan())
                Console.WriteLine(" • " + baris);
            Console.WriteLine("===============");
        }

        public decimal GetTotalPendapatanSelesai()
        {
            return _semuaPesanan
                .Where(p => p.Status == StatusPesanan.Selesai)
                .Sum(p => p.TotalHarga);
        }

        public int GetJumlahPesananPerStatus(StatusPesanan status)
        {
            return _semuaPesanan.Count(p => p.Status == status);
        }

        public PesananModel GetPesananTerbesar()
        {
            return _semuaPesanan
                .OrderByDescending(p => p.TotalHarga)
                .FirstOrDefault();
        }
    }
}
