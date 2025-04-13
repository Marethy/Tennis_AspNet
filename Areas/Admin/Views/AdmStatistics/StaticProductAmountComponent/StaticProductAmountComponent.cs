using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Areas.Admin.Views.AdmStatistics.Components.StaticProductAmountComponent;

public class StaticProductAmountComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}