using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductService.Dto
{
    public class ProductWithSpecsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string ProductBrandName { get; set; }
        public int? Storage { get; set; }
        public int? RAM { get; set; }
        public string? CPU { get; set; }
        public string? GPU { get; set; }
        public string? Screen { get; set; }
        public string? Color { get; set; }
        public string? Keyboard { get; set; }
        public string? Warranty { get; set; }
        public string? Panel { get; set; }
        public bool? Touchscreen { get; set; }
        public int Quantity { get; set; }
    }
}
