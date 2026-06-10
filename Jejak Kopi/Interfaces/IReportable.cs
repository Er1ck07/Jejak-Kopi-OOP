using System.Collections.Generic;

namespace Jejak_Kopi.Interfaces
{
    // =========================================================
    //  POLYMORPHISM via Interface
    //  Setiap kelas yang mengimplementasi interface ini dapat
    //  diperlakukan secara seragam oleh LaporanService,
    //  meski implementasinya berbeda-beda.
    // =========================================================

    /// <summary>
    /// Kontrak untuk objek yang bisa menghasilkan laporan ringkasan.
    /// </summary>
    public interface IReportable
    {
        string GetRingkasanLaporan();
        List<string> GetDetailLaporan();
    }

    /// <summary>
    /// Kontrak untuk objek yang bisa divalidasi sebelum diproses.
    /// </summary>
    public interface IValidatable
    {
        bool IsValid();
        string GetPesanValidasi();
    }

    /// <summary>
    /// Kontrak untuk entitas yang dapat di-soft-delete.
    /// </summary>
    public interface ISoftDeletable
    {
        bool IsDeleted { get; }
        void SoftDelete();
    }
}
