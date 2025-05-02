namespace DergiOtomasyon.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MagazineInfoId { get; set; }
        public DateTime Favorite_date { get; set; } = DateTime.Now; //Favori TArihi
        public virtual User User { get; set; }
        public virtual MagazineInfo MagazineInfo { get; set; }
    }
}
