using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Areas.Admin.Views.AdmStatistics.Components.RevenueStructureComponent;

public class RevenueStructureComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}