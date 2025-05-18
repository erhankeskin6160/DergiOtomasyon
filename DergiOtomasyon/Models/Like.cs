namespace DergiOtomasyon.Models
{
    public class Like 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MagazineInfoId { get; set; }
        public DateTime Lıke_date { get; set; } = DateTime.Now; //Beğeni Tarihi TArihi
        public virtual User User { get; set; }
        public virtual MagazineInfo MagazineInfo { get; set; }
    }
}
