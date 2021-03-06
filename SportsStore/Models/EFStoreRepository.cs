﻿using System.Linq;

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly SportsStoreDbContext context;

        public EFStoreRepository(SportsStoreDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Products => context.Products;
    }
}