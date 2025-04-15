using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Views.Cart.Components.ItemComponent;

public class ItemComponent(IProductRepository productRepo) : ViewComponent
{
    private readonly IProductRepository _productRepo = productRepo;

    public IViewComponentResult Invoke(Item item)
    {
        return View(item);
    }
}
