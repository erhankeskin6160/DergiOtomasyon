namespace DergiOtomasyon.Models
{
    public class Category
    {
       
        
            //Category Tablosu
            public int CategoryId { get; set; }//Kategori Id

            public string CategoryName { get; set; }//Kategori Adı
            public string CategoryImage { get; set; }  //Kategorinin Resmini Tutar

       
        public virtual  ICollection<Magazine> Magazine { get; set; }//Kitaplar   


        
    }
}
