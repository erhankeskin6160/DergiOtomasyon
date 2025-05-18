namespace DergiOtomasyon.Models
{
    public class PenaltyLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BorrowingId { get; set; }   
        public DateTime PenaltyDate { get; set; } // Ceza kesilen tarih
        public int Amount { get; set; } // Ceza tutarı
    }
}
