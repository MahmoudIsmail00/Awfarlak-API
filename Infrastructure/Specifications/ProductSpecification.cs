﻿namespace Infrastructure.Specifications
{
    public class ProductSpecification
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        private const int MAXPAGESIZE = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 12;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value;
        }
        //private string _search;

        //public string Search
        //{
        //    get => _search;
        //    set => _search = value.Trim().ToLower();
        //}



    }
}
