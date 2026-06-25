using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealershipDomain.Model
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва бренду")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Будь ласка, оберіть країну")]
        [Display(Name = "Країна походження")]
        public string Country { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
