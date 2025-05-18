using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DergiOtomasyon.Models
{
    public class MagazineInfo
    {

        public int Id { get; set; }

        public string? TopicName { get; set; }
        public int IssueNumber { get; set; }//dergini oanki sayı durumu

        public int PageCount { get; set; } //derginin oanki sayısı

        public string Authors { get; set; }

        public int ViewCount = 0;
        public int Point = 0;

        public string? Keyword { get; set; }//Amaç derginin konuları listelemek örnek  bilgisayarla ilgili konusu olan dergileri getircek kullanıcı
        public DateTime PublicationDate { get; set; }

        public string MagazineImg {  get; set; }    //derginin  sayıdaki resmi
        public int? Stok { get; set; } = 0;

        //Forgein key
        public int MagazineId { get; set; }

        public string? MagazineDescrition { get; set; }
        public virtual Magazine Magazine { get; set; }
        public virtual ICollection<Borrowing> Borrowings { get; set; } //navgation property


        public virtual ICollection<Rating> Ratings { get; set; }

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Ratings != null && Ratings.Any())
                    return Ratings.Average(r => r.Point);
                return 0;
            }
        }

        // Foreign Key

    }
}
