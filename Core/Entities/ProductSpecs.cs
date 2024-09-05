﻿namespace Core.Entities
{
    public class ProductSpecs : BaseEntity
    {

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
        public Product product { get; set; }
    }
}
