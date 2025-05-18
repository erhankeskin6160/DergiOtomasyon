namespace DergiOtomasyon.Models
{
    public class Rating
    {
        public int Id { get; set; }

         
        public int UserId { get; set; }

        public int MagazineInfoId { get; set; }

        public int Point { get; set; } // 1-5 arası puan

        public DateTime RatedDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }

        public virtual MagazineInfo MagazineInfo { get; set; }
    }
}
