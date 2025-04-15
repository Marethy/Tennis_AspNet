using Microsoft.AspNetCore.Mvc;
using Tennis.Models;

namespace Tennis.Views.Cart.Components.OrderDetailComponent;

public class OrderDetailComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<OrderDetail> obj)
    {
        return View(obj);
    }
}
