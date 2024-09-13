using System.Collections;

namespace Core.Entities
{
    public class SubCategory : BaseEntity 
    {
        public string Name { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

    }
}
