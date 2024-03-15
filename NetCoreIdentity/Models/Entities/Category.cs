﻿namespace NetCoreIdentity.Models.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        public string Desription { get; set; }

        // Relaitonal Properties

        public virtual ICollection<Product> Products { get; set; }
    }
}
