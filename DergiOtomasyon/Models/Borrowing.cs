namespace DergiOtomasyon.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        public int UserId { get; set; }    //Dergiyi Alan Kullanıcı
        public int MagazineInfoId { get; set; }//Hangi Dergiyi Aldı

       
        public DateTime BorrowingDate { get; set; } = DateTime.Now;//ödünç aldığı tarih

        public DateTime DeliveryDate => BorrowingDate.AddDays(15);//iade edilmesi gereken Tarih

        public DateTime? ReturnDate { get; set; }  //iade edilen Tarih

        public bool? ısReturned{ get; set; }//iade edildimi

        public bool? IsOverdue { get; set; }
         

        public double PenaltyAmount
        {
            get
            {
                if (IsOverdue == false)
                {
                    User.Balance = 10;
                }
                return 10;
            }
        }
       


        public virtual User User { get; set; } //navgation property


        
        public virtual MagazineInfo MagazineInfo { get; set; }
    }
}