namespace Tennis.Views.Product.Components.SortComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class SortComponent : ViewComponent
{
    private readonly IProductRepository _repo;

    public SortComponent(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
