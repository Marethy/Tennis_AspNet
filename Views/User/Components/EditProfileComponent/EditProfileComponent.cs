using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.User.Components.EditProfileComponent;

public class EditProfileComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}