using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DergiOtomasyon.Models
{
    public class User 
    {
        public int Id { get; set; }
      
        public string Name { get; set; }
    
        public string LastName {  get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Balance { get; set; } = 0;  //Bakiye sistemi koyduk çünkü kullanıcı iade etmesse dergiye borçlanacak

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool Subscription { get; set; } = true;//abonelik durumu
    
        public string? UserImg {  get; set; }    
     

    }
}