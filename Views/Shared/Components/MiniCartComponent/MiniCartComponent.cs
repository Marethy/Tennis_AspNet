using System.Globalization;
using Tennis.Interfaces;
using Tennis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Cart.Components.MiniCartComponent;

public class MiniCartComponent : ViewComponent
{
	private readonly ICartRepository _cartRepo;
	private readonly CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

	public MiniCartComponent(ICartRepository cartRepo)
	{
		_cartRepo = cartRepo;
	}

	public decimal TotalMoney()
	{
		decimal totalMoney = 0;
		List<Item> listCart = _cartRepo.Get(HttpContext.Session);
		if (listCart != null)
			totalMoney = listCart.Sum(x =>
				x.Quantity * (x.Product.ProductPrice - x.Product.ProductPrice * x.Product.ProductDiscount / 100));
		return totalMoney;
	}

	public IViewComponentResult Invoke()
	{
		var cart = _cartRepo.Get(HttpContext.Session);
		ViewData["TotalMoney"] = TotalMoney().ToString("#,###", cul.NumberFormat);
		ViewData["Count"] = cart.Count();
		return View(cart);
	}
}