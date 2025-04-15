using Tennis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Cart.Components.OderDetail;

public class OrderDetailComponent : ViewComponent
{
	public IViewComponentResult Invoke(IEnumerable<OrderDetail> obj)
	{
		return View(obj);
	}
}