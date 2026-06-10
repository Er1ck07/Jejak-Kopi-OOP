using System;
using System.Collections.Generic;
using System.Linq;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  ENCAPSULATION: ItemKeranjang menjaga konsistensi qty & subtotal
    // =========================================================
    public class ItemKeranjang
    {
        private int _qty;

        public int IdKeranjang { get; }
        public int IdKatalog   { get; }
        public string NamaBijiKopi { get; }
        public string TipeBiji     { get; }
        public int HargaSatuan     { get; }

        public int Qty
        {
            get => _qty;
            set
            {
                if (value <= 0) throw new ArgumentException("Jumlah item minimal 1.");
                _qty = value;
            }
        }

        // Subtotal dihitung otomatis — tidak bisa diset langsung dari luar
        public decimal Subtotal => HargaSatuan * (decimal)_qty;

        public ItemKeranjang(int idKeranjang, int idKatalog, string nama,
                             string tipe, int harga, int qty)
        {
            IdKeranjang   = idKeranjang;
            IdKatalog     = idKatalog;
            NamaBijiKopi  = nama;
            TipeBiji      = tipe;
            HargaSatuan   = harga;
            Qty           = qty;
        }

        public override string ToString() =>
            $"{NamaBijiKopi} ({TipeBiji}) x{Qty} = Rp {Subtotal:N0}";
    }

    // =========================================================
    //  RELATIONSHIP — Composition:
    //  KeranjangModel tidak bisa ada tanpa Pelanggan pemiliknya.
    //  KeranjangModel mengandung daftar ItemKeranjang (Aggregation).
    // =========================================================
    public class KeranjangModel
    {
        // Referensi ke pemilik (Composition — hidup bersama Pelanggan)
        private readonly Pelanggan _pemilik;

        private readonly List<ItemKeranjang> _items = new();

        // Baca-saja dari luar
        public IReadOnlyList<ItemKeranjang> Items => _items.AsReadOnly();

        public string UsernamePemilik => _pemilik.Username;

        public decimal TotalHarga => _items.Sum(i => i.Subtotal);

        public int JumlahItem => _items.Count;

        // Hanya bisa dibuat oleh Pelanggan (lihat Pelanggan.cs)
        internal KeranjangModel(Pelanggan pemilik)
        {
            _pemilik = pemilik ?? throw new ArgumentNullException(nameof(pemilik));
        }

        public void TambahItem(ItemKeranjang item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Jika item dengan id yang sama sudah ada, tambah qty-nya saja
            var existing = _items.FirstOrDefault(i => i.IdKatalog == item.IdKatalog);
            if (existing != null)
                existing.Qty += item.Qty;
            else
                _items.Add(item);
        }

        public bool HapusItem(int idKeranjang)
        {
            var item = _items.FirstOrDefault(i => i.IdKeranjang == idKeranjang);
            if (item == null) return false;
            _items.Remove(item);
            return true;
        }

        public List<int> GetIdItemTerpilih(IEnumerable<int> idList)
        {
            return _items
                .Where(i => idList.Contains(i.IdKeranjang))
                .Select(i => i.IdKeranjang)
                .ToList();
        }

        public void Kosongkan() => _items.Clear();

        public override string ToString() =>
            $"Keranjang @{UsernamePemilik}: {JumlahItem} item | Total Rp {TotalHarga:N0}";
    }
}
