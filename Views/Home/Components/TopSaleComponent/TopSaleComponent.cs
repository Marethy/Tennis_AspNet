namespace Tennis.Views.Home.Components.TopSaleComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class TopSaleComponent(IProductRepository repo) : ViewComponent
{
    private readonly IProductRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync(
            x => x.ProductDiscount > 0, x => x.OrderByDescending(x => x.ProductId), take: 8);
        return View("TopSaleComponent", obj);
    }
}