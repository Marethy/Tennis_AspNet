using Tennis.Models;

namespace Tennis.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
	public Task UpdatePaymentState(int orderId);
}