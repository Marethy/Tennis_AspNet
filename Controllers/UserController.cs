using System.Security.Claims;
using Tennis.Interfaces;
using Tennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Controllers;

[AllowAnonymous]
public class UserController : Controller
{
	private readonly IMailService _mailService;
	private readonly IUserRepository _repo;
	private readonly ITokenRepository _tokenRepository;
	private readonly IUserManager _userManager;

	public UserController(IUserRepository repo, IUserManager userManager, IMailService mailService,
		ITokenRepository tokenRepository)
	{
		_repo = repo;
		_userManager = userManager;
		_mailService = mailService;
		_tokenRepository = tokenRepository;
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
	{
		if (!ModelState.IsValid)
			return View(model);

		var user = _repo.Register(model);

		// SignIn user after registration
		await _userManager.SignIn(HttpContext, user);

		return LocalRedirect("~/Home/Index");
	}

	[HttpGet]
	public async Task<IActionResult> Login()
	{
		if (User.Identity.IsAuthenticated)
			await _userManager.SignOut(HttpContext);
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> LoginAsync(LoginViewModel model)
	{
		if (!ModelState.IsValid)
			return RedirectToAction("Index", "User");

		var user = _repo.Validate(model);

		if (user == null)
			return View(model);

		// SignIn user after validation
		await _userManager.SignIn(HttpContext, user, model.RememberLogin);

		return RedirectToAction("Index", "Home");
	}

	public async Task<IActionResult> Logout(string returnUrl)
	{
		if (!User.Identity.IsAuthenticated || User.IsInRole("Admin"))
			return RedirectToAction("Index", "Home");
		await _userManager.SignOut(HttpContext);

		return RedirectToAction("Index", "Home");
	}

	[HttpGet]
	public IActionResult ForgotPassword()
	{
		return View();
	}


}