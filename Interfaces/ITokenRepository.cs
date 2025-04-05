namespace Tennis.Interfaces;

public interface ITokenRepository
{
	public bool CheckToken(string userName, string token);
}