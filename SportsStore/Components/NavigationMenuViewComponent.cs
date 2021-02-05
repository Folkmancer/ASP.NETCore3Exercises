﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            return View(repository.Products
                    .Select(product => product.Category)
                    .Distinct()
                    .OrderBy(category => category)
                );
        }
    }
}