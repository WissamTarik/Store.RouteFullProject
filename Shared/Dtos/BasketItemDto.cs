using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Shared.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        [Range(0,double.MaxValue)]
        public double Price { get; set; }
        [Range(1,99)]
        public int Quantity {  get; set; }
    }
}
