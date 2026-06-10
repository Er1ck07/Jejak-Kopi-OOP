using System;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  ENCAPSULATION: BijiKopi menyembunyikan detail stok dan harga
    //  INHERITANCE: BijiKopi → KopiArabika, KopiRobusta (spesialisasi)
    //  POLYMORPHISM: override GetDeskripsi() dan GetKategori()
    // =========================================================
    public abstract class BijiKopi
    {
        private int _id;
        private string _nama;
        private decimal _stok;       // dalam kg
        private int _harga;          // per kg dalam rupiah

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
                    throw new ArgumentException("Nama biji kopi tidak boleh kosong.");
                _nama = value.Trim();
            }
        }

        public decimal Stok
        {
            get => _stok;
            private set
            {
                if (value < 0) throw new ArgumentException("Stok tidak boleh negatif.");
                _stok = value;
            }
        }

        public int Harga
        {
            get => _harga;
            set
            {
                if (value <= 0) throw new ArgumentException("Harga harus lebih dari 0.");
                _harga = value;
            }
        }

        public bool TersediaUntukDijual => _stok > 0;

        protected BijiKopi(int id, string nama, decimal stok, int harga)
        {
            Id    = id;
            Nama  = nama;
            Stok  = stok;
            Harga = harga;
        }

        // --- POLYMORPHISM: metode abstrak yang wajib diimplementasi subclass ---
        public abstract string GetKategori();
        public abstract string GetDeskripsi();

        // --- Metode konkret yang diwarisi ---
        public bool KurangiStok(decimal jumlah)
        {
            if (jumlah <= 0) throw new ArgumentException("Jumlah harus lebih dari 0.");
            if (_stok < jumlah) return false;   // stok tidak cukup
            _stok -= jumlah;
            return true;
        }

        public void TambahStok(decimal jumlah)
        {
            if (jumlah <= 0) throw new ArgumentException("Jumlah tambah stok harus positif.");
            _stok += jumlah;
        }

        public virtual string GetRingkasan() =>
            $"{Nama} [{GetKategori()}] — Stok: {Stok:N1} kg | Rp {Harga:N0}/kg";

        public override string ToString() => GetRingkasan();
    }

    // =========================================================
    //  INHERITANCE + POLYMORPHISM: Subclass spesialisasi
    // =========================================================
    public class KopiArabika : BijiKopi
    {
        public string AsalDaerah { get; set; }

        public KopiArabika(int id, string nama, decimal stok, int harga, string asal = "")
            : base(id, nama, stok, harga)
        {
            AsalDaerah = asal;
        }

        public override string GetKategori() => "Arabika";

        public override string GetDeskripsi() =>
            $"Kopi Arabika asal {(string.IsNullOrEmpty(AsalDaerah) ? "tidak diketahui" : AsalDaerah)}. " +
            "Cita rasa lembut dengan tingkat keasaman yang menyegarkan.";

        public override string GetRingkasan() =>
            base.GetRingkasan() + $" | Asal: {AsalDaerah}";
    }

    public class KopiRobusta : BijiKopi
    {
        public int KadarKafein { get; set; }   // dalam persen

        public KopiRobusta(int id, string nama, decimal stok, int harga, int kadarKafein = 0)
            : base(id, nama, stok, harga)
        {
            KadarKafein = kadarKafein;
        }

        public override string GetKategori() => "Robusta";

        public override string GetDeskripsi() =>
            $"Kopi Robusta dengan kadar kafein {KadarKafein}%. " +
            "Rasa kuat, pahit, dan cocok untuk espresso.";

        public override string GetRingkasan() =>
            base.GetRingkasan() + $" | Kafein: {KadarKafein}%";
    }
}
