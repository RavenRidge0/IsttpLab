using System.ComponentModel.DataAnnotations;

namespace DealershipDomain.Model
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Модель автомобіля")]
        public string ModelName { get; set; }

        [Display(Name = "Рік випуску")]
        [Range(1886, int.MaxValue, ErrorMessage = "Рік випуску не може бути меншим за 1886")]
        [CurrentYearMax(ErrorMessage = "Рік випуску не може бути в майбутньому")]
        public int Year { get; set; }

        [Display(Name = "Ціна ($)")]
        public decimal Price { get; set; }

        public int BrandId { get; set; }

        [Display(Name = "Бренд")]
        public virtual Brand? Brand { get; set; }
    }
}
