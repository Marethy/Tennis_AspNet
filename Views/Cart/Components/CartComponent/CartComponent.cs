using Tennis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Cart.Components.CartComponent;

public class CartComponent : ViewComponent
{
	public IViewComponentResult Invoke(IEnumerable<Item> list)
	{
		return View(list);
	}
}