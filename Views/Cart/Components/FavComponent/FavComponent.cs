using Microsoft.AspNetCore.Mvc;
using Tennis.Models;

namespace Tennis.Views.Cart.Components.FavComponent;

public class FavComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Favorite> fav)
    {
        return View("FavComponent", fav);
    }
}
