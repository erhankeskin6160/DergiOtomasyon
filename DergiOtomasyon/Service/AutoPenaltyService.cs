using DergiOtomasyon.Models;
using System.Linq;

namespace DergiOtomasyon.Service
{
    public class AutoPenaltyService
    {
        private readonly MagazineDbContext _context;

        public AutoPenaltyService(MagazineDbContext context)
        {
            _context = context;
        }

        public void AutoPenalty()
        {
            int penalty = 10;

            // Gecikmiş ödünç alınan dergileri al
            var overdueBorrowings = _context.Borrowings
                .Where(x => x.ReturnDate == null && DateTime.Now > x.DeliveryDate)
                .ToList();

            foreach (var borrowing in overdueBorrowings)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == borrowing.UserId);
                if (user == null) continue;

                // Bu ödünç alma için daha önce kesilmiş ceza günlerini al
                var penaltyDays = _context.PenaltyLogs
                    .Where(x => x.BorrowingId == borrowing.Id)
                    .Select(x => x.PenaltyDate.Date) // sadece tarih kısmını al
                    .ToList();

                // Ceza uygulamaya başlama tarihi
                var penaltyStartDate = borrowing.DeliveryDate.AddDays(1);
                var today = DateTime.Now.Date; // sadece tarihi al, saat kısmını dikkate alma

                // Teslim tarihinden bugüne kadar olan günleri kontrol et
                for (var date = penaltyStartDate; date <= today; date = date.AddDays(1))
                {
                    if (!penaltyDays.Contains(date)) // Ceza daha önce uygulanmamışsa
                    {
                        user.Balance -= penalty; // Ceza kes
                        _context.PenaltyLogs.Add(new PenaltyLog
                        {
                            UserId = user.Id,
                            BorrowingId = borrowing.Id,
                            Amount = penalty,
                            PenaltyDate = date
                        });
                    }
                }
            }

            // Değişiklikleri veritabanına kaydet
            _context.SaveChanges();
        }
    }
}
