using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Views.User.Components.ShowProfileComponent;

public class ShowProfileComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}