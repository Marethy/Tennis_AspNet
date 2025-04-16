namespace Tennis.Views.Product.Components.RelatedProductComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class RelatedProductComponent : ViewComponent
{
    private readonly IProductRepository _repo;

    public RelatedProductComponent(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync();
        return View("RelatedProductComponent", obj);
    }
}
