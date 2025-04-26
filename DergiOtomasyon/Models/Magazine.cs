using System.ComponentModel.DataAnnotations.Schema;

namespace DergiOtomasyon.Models
{
    public class Magazine
    {
        public int Id { get; set; }


        public string MagazineName { get; set; }//dergi ismi


        public string Editor { get; set; }//editor


        public int MagazineIssue { get; set; }//dergi sayısı

        public DateTime PublisDate { get; set; }

        public bool ısPuslihed { get; set; } //aktiflik durumu

        public string Img { get; set; }//dergi kapak

        public virtual ICollection<MagazineInfo> MagazineInfo { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; } //kategori




    }
}