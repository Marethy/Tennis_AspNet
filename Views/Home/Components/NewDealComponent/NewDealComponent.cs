using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Home.Components.NewDealComponent;

public class NewDealComponent : ViewComponent
{
	private readonly IProductRepository _repo;

	public NewDealComponent(IProductRepository repo)
	{
		_repo = repo;
	}

	//public async Task<IViewComponentResult> InvokeAsync()
	//{
	//	var obj = await _repo.GetListAsync(
	//		x => x.ProductDateCreated.Month == DateTime.Now.Month);
	//	return View("NewDealComponent", obj);
	//}

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var allProducts = await _repo.GetListAsync(); // Lấy toàn bộ sản phẩm

        // Tìm ngày tạo gần nhất
        var latestDate = allProducts.Max(x => x.ProductDateCreated.Date);

        // Lọc sản phẩm có ngày tạo trùng với ngày gần nhất
        var latestProducts = allProducts
            .Where(x => x.ProductDateCreated.Date == latestDate)
            .ToList();

        return View("NewDealComponent", latestProducts);
    }

}