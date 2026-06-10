using System;

namespace Jejak_Kopi.Models
{
    // =========================================================
    //  POLYMORPHISM via Factory Pattern
    //  Kode pemanggil cukup menerima UserBase — tidak perlu tahu
    //  apakah objeknya Admin atau Pelanggan. Perilaku yang tepat
    //  dipanggil secara otomatis (dynamic dispatch).
    // =========================================================
    public static class UserFactory
    {
        /// <summary>
        /// Membuat instance yang tepat (Admin atau Pelanggan) berdasarkan flag isAdmin.
        /// </summary>
        public static UserBase Create(int id, bool isAdmin, string nama,
                                      string username, string password,
                                      string noTelepon, string email,
                                      bool isDeleted = false)
        {
            if (isAdmin)
                return new Admin(id, nama, username, password, noTelepon, email, isDeleted);
            else
                return new Pelanggan(id, nama, username, password, noTelepon, email, isDeleted);
        }

        /// <summary>
        /// Bridge dari class User lama ke model OOP baru.
        /// Digunakan di Form Login untuk backward-compatibility.
        /// </summary>
        public static UserBase CreateFromLegacy(User legacyUser)
        {
            if (legacyUser == null) throw new ArgumentNullException(nameof(legacyUser));

            return Create(
                legacyUser.id,
                legacyUser.is_admin,
                legacyUser.nama      ?? string.Empty,
                legacyUser.username  ?? string.Empty,
                legacyUser.password  ?? string.Empty,
                legacyUser.no_telepon ?? string.Empty,
                legacyUser.email     ?? string.Empty,
                legacyUser.is_delete
            );
        }
    }
}
