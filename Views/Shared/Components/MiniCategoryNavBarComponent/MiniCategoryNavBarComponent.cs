﻿using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Shared.Components.MiniCategoryNavBarComponent;

public class MiniCategoryNavBarComponent : ViewComponent
{
	private readonly ICategoryRepository _repo;

	public MiniCategoryNavBarComponent(ICategoryRepository repo)
	{
		_repo = repo;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repo.GetListAsync();
		return View(obj);
	}
}