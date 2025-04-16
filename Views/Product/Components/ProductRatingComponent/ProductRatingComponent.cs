namespace Tennis.Views.Product.Components.ProductRatingComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class ProductRatingComponent : ViewComponent
{
    private readonly IProductRatingRepository _repo;

    public ProductRatingComponent(IProductRatingRepository repo)
    {
        _repo = repo;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var obj = await _repo.GetListAsync(x => x.ProductId == id, includeProperties: "Customer");
        return View("ProductRatingComponent", obj);
    }
}
