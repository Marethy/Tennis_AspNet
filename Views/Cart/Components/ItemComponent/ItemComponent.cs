using Tennis.Interfaces;
using Tennis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Cart.Components.ItemComponent;

public class ItemComponent : ViewComponent
{
	private readonly IProductRepository _productRepo;

	public ItemComponent(IProductRepository productRepo)
	{
		_productRepo = productRepo;
	}

	public IViewComponentResult Invoke(Item item)
	{
		return View(item);
	}
}