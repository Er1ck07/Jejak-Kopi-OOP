using System;
using System.Collections.Generic;
using System.Linq;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  ENCAPSULATION: Status pesanan hanya bisa berubah lewat metode
    //  RELATIONSHIP — Aggregation: Pesanan punya daftar ItemPesanan.
    //  ItemPesanan bisa tetap ada meski Pesanannya dihapus (berbeda
    //  dengan Composition).
    // =========================================================

    public enum StatusPesanan
    {
        Diproses,
        Dikirim,
        Selesai,
        Dibatalkan
    }

    public class ItemPesanan
    {
        public int IdKatalog     { get; }
        public string NamaBiji   { get; }
        public int HargaSatuan   { get; }
        public int Qty           { get; }
        public decimal Subtotal  => HargaSatuan * (decimal)Qty;

        public ItemPesanan(int idKatalog, string namaBiji, int harga, int qty)
        {
            if (qty <= 0) throw new ArgumentException("Qty harus positif.");
            IdKatalog   = idKatalog;
            NamaBiji    = namaBiji;
            HargaSatuan = harga;
            Qty         = qty;
        }

        public override string ToString() =>
            $"{NamaBiji} x{Qty} = Rp {Subtotal:N0}";
    }

    public class PesananModel
    {
        // --- private backing field untuk status (ENCAPSULATION) ---
        private StatusPesanan _status;

        public int IdPesanan           { get; }
        public int IdPengguna          { get; }
        public DateTime TanggalPesanan { get; }
        public string MetodePembayaran { get; }

        // RELATIONSHIP — Aggregation: daftar item dalam pesanan
        private readonly List<ItemPesanan> _items;
        public IReadOnlyList<ItemPesanan> Items => _items.AsReadOnly();

        public StatusPesanan Status
        {
            get => _status;
            private set => _status = value;
        }

        // Total dihitung dari item, tidak bisa diset manual
        public decimal TotalHarga => _items.Sum(i => i.Subtotal);

        public PesananModel(int idPesanan, int idPengguna, string metodePembayaran,
                            List<ItemPesanan> items)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("Pesanan harus memiliki minimal 1 item.");

            IdPesanan       = idPesanan;
            IdPengguna      = idPengguna;
            TanggalPesanan  = DateTime.Now;
            MetodePembayaran = metodePembayaran;
            _items          = new List<ItemPesanan>(items);
            _status         = StatusPesanan.Diproses;
        }

        // --- Transisi status (ENCAPSULATION: logika transisi terpusat) ---
        public bool UbahStatus(StatusPesanan statusBaru)
        {
            // Aturan bisnis: pesanan yang sudah selesai/dibatalkan tidak bisa diubah
            if (_status == StatusPesanan.Selesai || _status == StatusPesanan.Dibatalkan)
                return false;

            // Aturan bisnis: tidak bisa mundur status
            if (statusBaru < _status)
                return false;

            _status = statusBaru;
            return true;
        }

        public bool Batalkan()
        {
            if (_status != StatusPesanan.Diproses) return false;
            _status = StatusPesanan.Dibatalkan;
            return true;
        }

        public string GetStatusLabel() => _status switch
        {
            StatusPesanan.Diproses   => "Sedang Diproses",
            StatusPesanan.Dikirim    => "Sedang Dikirim",
            StatusPesanan.Selesai    => "Selesai",
            StatusPesanan.Dibatalkan => "Dibatalkan",
            _                        => "Tidak Diketahui"
        };

        public override string ToString() =>
            $"Pesanan #{IdPesanan} | {TanggalPesanan:dd/MM/yyyy} | {GetStatusLabel()} | " +
            $"Total Rp {TotalHarga:N0}";
    }
}
