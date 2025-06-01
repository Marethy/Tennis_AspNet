using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class TokenRepository(TennisWebMVCContext context)
    : ITokenRepository
{
    private readonly TennisWebMVCContext _context = context;

    public bool CheckToken(string userName, string token)
    {
        return _context.Tokens.FirstOrDefault(Token =>
            Token.CustomerUserName == userName && Token.TokenValue == token && Token.Expiry > DateTime.Now) != null;
    }
}
