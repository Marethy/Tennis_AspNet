﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tennis.ViewModels;

namespace Tennis.Interfaces;

public interface IUserManager
{
	public Task SignIn(
		HttpContext httpContext,
		CookieUserItem user,
		bool isPersistent = false);

	public Task SignOut(HttpContext httpContext);
}

public class UserManager : IUserManager
{
	public async Task SignIn(HttpContext httpContext, CookieUserItem user, bool isPersistent = false)
	{
		var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

		// Generate Claims from DbEntity
		var claims = GetUserClaims(user);

		var claimsIdentity = new ClaimsIdentity(
			claims, authenticationScheme);

		var claimsPrincipal = new ClaimsPrincipal(
			claimsIdentity);

		var authProperties = new AuthenticationProperties
		{
			AllowRefresh = true,
			// Refreshing the authentication session should be allowed.
			ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),
			// The time at which the authentication ticket expires. A 
			// value set here overrides the ExpireTimeSpan option of 
			// CookieAuthenticationOptions set with AddCookie.
			IsPersistent = isPersistent
			// Whether the authentication session is persisted across 
			// multiple requests. Required when setting the 
			// ExpireTimeSpan option of CookieAuthenticationOptions 
			// set with AddCookie. Also required when setting 
			// ExpiresUtc.
			// IssuedUtc = <DateTimeOffset>,
			// The time at which the authentication ticket was issued.
			// RedirectUri = "~/Account/Index"
			// The full path or absolute URI to be used as an http 
			// redirect response value.
		};

		await httpContext.SignInAsync(
			"Signin",
			claimsPrincipal,
			authProperties
		);
	}

	public async Task SignOut(HttpContext httpContext)
	{
		await httpContext.SignOutAsync(
			"Signin");
	}

	private List<Claim> GetUserClaims(CookieUserItem user)
	{
		List<Claim> claims = new()
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role)
		};
		return claims;
	}
}