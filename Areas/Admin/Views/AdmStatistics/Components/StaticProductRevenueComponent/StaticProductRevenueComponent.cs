using Microsoft.AspNetCore.Mvc;

namespace Tennis.Areas.Admin.Views.AdmStatistics.Components.StaticProductRevenueComponent;

public class StaticProductRevenueComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}