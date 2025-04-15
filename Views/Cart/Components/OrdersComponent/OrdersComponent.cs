using Microsoft.AspNetCore.Mvc;
using Tennis.Models;

namespace Tennis.Views.Cart.Components.OrdersComponent;

public class OrdersComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<Order> obj)
    {
        return View(obj);
    }
}
