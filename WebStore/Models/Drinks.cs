using System.ComponentModel;

namespace WebStore.Models
{
	public class Drinks
	{
        [DisplayName("Код")]
        public int Id { get; set; }

        [DisplayName("Напиток")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }

        [DisplayName("Цена(руб)")]
        public int Price { get; set; }
	}
}
