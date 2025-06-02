using Microsoft.AspNetCore.SignalR;

namespace Tennis.Hubs;

public class CustomerHub : Hub
{
	public static int _customerCounter;

	public static int CustomerCount
	{
		get => _customerCounter;
		set => _customerCounter = value;
	}

	public override Task OnConnectedAsync()
	{
		_customerCounter++;
		Clients.All.SendAsync("updateCustomerCounter", _customerCounter).GetAwaiter().GetResult();
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		_customerCounter--;
		Clients.All.SendAsync("updateCustomerCounter", _customerCounter).GetAwaiter().GetResult();
		return base.OnDisconnectedAsync(exception);
	}
}