using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Areas.Admin.Views.AdmStatistics.Components.StaticProductRevenueComponent;

public class StaticProductRevenueComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}