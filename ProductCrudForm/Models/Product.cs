using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductCrudForm.Models
{
    [Serializable]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string CreatedAt { get; set; }
    }
}