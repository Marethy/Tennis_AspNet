using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Views.User.Components.EditProfileComponent;

public class EditProfileComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}