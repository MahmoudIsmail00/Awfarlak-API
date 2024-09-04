namespace Core.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
