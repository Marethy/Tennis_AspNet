using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Shared.Components.HeaderCategoryComponent;

public class HeaderCategoryComponent : ViewComponent
{
	private readonly IProductRepository _productRepository;
	private readonly ICategoryRepository _repoCategory;

	public HeaderCategoryComponent(ICategoryRepository repoCategory, IProductRepository productRepository)
	{
		_repoCategory = repoCategory;
		_productRepository = productRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repoCategory.GetListAsync();
		//var obj = await _productRepository.GetListAsync();
		return View(obj);
	}
}