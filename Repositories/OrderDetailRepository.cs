using FoodWebMVC.Interfaces;
using FoodWebMVC.Models;

namespace FoodWebMVC.Repositories;

public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
{
	public OrderDetailRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}