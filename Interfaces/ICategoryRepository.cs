﻿using Tennis.Models;

namespace Tennis.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
	//public Task<List<Category>> Get();
	//public Task<Category> GetById(int id);
	public Table[] GetRevenueStructure(int year);
}