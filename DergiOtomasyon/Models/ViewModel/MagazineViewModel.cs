namespace DergiOtomasyon.Models.ViewModel
{
    public class MagazineViewModel
    {
        public List<MagazineInfo> magazineInfo { get; set; }
        public int     CurrentPage{ get; set; }    
        public int TotalPages { get; set; }  
    }
}
