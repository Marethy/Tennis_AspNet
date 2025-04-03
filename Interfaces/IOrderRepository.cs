using FoodWebMVC.Models;

namespace FoodWebMVC.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
	public Task UpdatePaymentState(int orderId);
}