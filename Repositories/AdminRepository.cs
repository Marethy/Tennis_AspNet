namespace Tennis.Repositories;

using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.ViewModels;

public class AdminRepository(TennisWebMVCContext context, IConfiguration configuration)
    : IAdminRepository
{
    public CookieUserItem Validate(LoginViewModel model)
    {
        var userRecords = context.Admins.Where(x => x.AdminUserName == model.UserName);

        var results = userRecords.AsEnumerable()
            .Where(m => m.AdminPassword == Encode(model.Password))
            .Select(m => new CookieUserItem
            {
                Id = m.AdminId,
                Email = m.AdminEmail,
                UserName = m.AdminUserName,
                Role = "Admin"
            });

        return results.FirstOrDefault();
    }

    public string CreateResetPasswordLink(string adminUserName)
    {
        var token = ReturnToken(64);
        var toKen = new Token(adminUserName, token, DateTime.Now.AddMinutes(2));
        context.Tokens.Add(toKen);
        context.SaveChanges();

        var baseUrl = configuration["AppSettings:BaseUrl"]; // Lấy BaseUrl từ appsettings.json
        return $"{baseUrl}/Admin/AdmAccount/ResetPassword?user={adminUserName}&token={token}";
    }

    public async Task<bool> HaveAccount(ForgotViewModel model)
    {
        return await context.Admins.AnyAsync(x => x.AdminUserName == model.UserName && x.AdminEmail == model.Email);
    }

    public async Task<bool> HaveAccount(string userName, string password)
    {
        return await context.Admins.AnyAsync(_context =>
            _context.AdminUserName == userName && _context.AdminPassword == Encode(password));
    }

    public async Task ResetPassWord(ResetViewModel model)
    {
        var Admin = GetAdmin(model.UserName);
        Admin.AdminPassword = Encode(model.Password);
        await context.SaveChangesAsync();
    }

    public async Task ChangePasswordUser(ChangePasswordViewModel model, int id)
    {
        var user = GetAdmin(id);
        user.AdminPassword = Encode(model.NewPassword);
        await context.SaveChangesAsync();
    }

    private string Encode(string originalPassword)
    {
        using var md5 = MD5.Create();
        var originalBytes = Encoding.Default.GetBytes(originalPassword);
        var encodedBytes = md5.ComputeHash(originalBytes);

        return BitConverter.ToString(encodedBytes);
    }

    private string ReturnToken(int length,
        string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
        if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

        const int byteSize = 0x100;
        var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
        if (byteSize < allowedCharSet.Length)
            throw new ArgumentException(
                string.Format("allowedChars may contain no more than {0} characters.", byteSize));

        // Guid.NewGuid and System.Random are not particularly random. By using a
        // cryptographically-secure random number generator, the caller is always
        // protected, regardless of use.
        using (var rng = RandomNumberGenerator.Create())
        {
            var result = new StringBuilder();
            var buf = new byte[128];
            while (result.Length < length)
            {
                rng.GetBytes(buf);
                for (var i = 0; i < buf.Length && result.Length < length; ++i)
                {
                    // Divide the byte into allowedCharSet-sized groups. If the
                    // random value falls into the last group and the last group is
                    // too small to choose from the entire allowedCharSet, ignore
                    // the value in order to avoid biasing the result.
                    var outOfRangeStart = byteSize - byteSize % allowedCharSet.Length;
                    if (outOfRangeStart <= buf[i]) continue;
                    result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                }
            }

            return result.ToString();
        }
    }

    private Admin GetAdmin(int id)
    {
        return context.Admins.Find(id);
    }

    private Admin GetAdmin(string userName)
    {
        return context.Admins.FirstOrDefault(user => user.AdminUserName == userName);
    }

    public CookieUserItem Register(RegisterViewModel model)
    {
        var user = new Admin
        {
            AdminUserName = model.UserName,
            AdminPassword = Encode(model.Password),
            AdminEmail = model.Email,
            AdminImage = "avatar.jpg",
            AdminDateCreated = DateTime.Now
        };
        context.Admins.Add(user);
        context.SaveChanges();

        return new CookieUserItem
        {
            Id = user.AdminId,
            UserName = user.AdminUserName,
            Email = user.AdminEmail,
            Role = "Admin"
        };
    }
}
