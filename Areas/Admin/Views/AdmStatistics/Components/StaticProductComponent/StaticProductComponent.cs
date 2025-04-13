using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Areas.Admin.Views.AdmStatistics.Components.StaticProductComponent;

public class StaticProductComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}