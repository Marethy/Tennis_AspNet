using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

namespace Tennis.Views.Home.Components.NewDealComponent;

public class NewDealComponent(IProductRepository repo) : ViewComponent
{
    private readonly IProductRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync(
            x => x.ProductDateCreated.Month == DateTime.Now.Month);
        return View("NewDealComponent", obj);
    }
}
