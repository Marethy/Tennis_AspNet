using Microsoft.AspNetCore.Mvc;
using Tennis.Models;

namespace Tennis.Views.Cart.Components.CartComponent;

public class CartComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<Item> list)
    {
        return View(list);
    }
}
