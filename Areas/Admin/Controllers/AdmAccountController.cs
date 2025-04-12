using System.Security.Claims;
using FoodWebMVC.Interfaces;
using FoodWebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AdmAccountController : Controller
{
	private readonly IMailService _mailService;
	private readonly IAdminRepository _repo;
	private readonly ITokenRepository _tokenRepository;
	private readonly IUserManager _userManager;

	public AdmAccountController(IAdminRepository repo, IUserManager userManager, IMailService mailService,
		ITokenRepository tokenRepository)
	{
		_repo = repo;
		_userManager = userManager;
		_mailService = mailService;
		_tokenRepository = tokenRepository;
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
			return View(model);
		var user = _repo.Validate(model);

		if (user == null)
			return View(model);

		// SignIn the user
		await _userManager.SignIn(HttpContext, user, model.RememberLogin);
		return RedirectToAction("Index", "AdmHome");
	}

	public async Task<IActionResult> Logout()
	{
		if (!User.Identity.IsAuthenticated || User.IsInRole("Customer"))
			return RedirectToAction("Index", "AdmHomeController");
		await _userManager.SignOut(HttpContext);

		return RedirectToAction("Login");
	}

	[HttpGet]
	public IActionResult ForgotPassword()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> ForgotPasswordAsync(ForgotViewModel model)
	{
		if (ModelState.IsValid)
		{
			var user = await _repo.HaveAccount(model);
			if (!user)
			{
				ViewBag.Message = "Your username or email is wrong!";
				return View(model);
			}

			// Create reset password link and send email
			var linkResetPassword = _repo.CreateResetPasswordLink(model.UserName);
			var mailRequest = new MailRequest(
				toEmail: model.Email,
				subject: "Reset Your Password",
				body: $"<p>Dear {model.UserName},</p>" +
					  $"<p>Click the link below to reset your password:</p>" +
					  $"<a href='{linkResetPassword}'>Reset Password</a><br>" +
					  $"<p>If you did not request this, please ignore this email.</p>");
			await _mailService.SendEmailAsync(mailRequest);

			try
			{
				// Send the email
				await _mailService.SendEmailAsync(mailRequest);
				return RedirectToAction("ShowMessage");
			}
			catch (Exception ex)
			{
				ViewBag.Message = $"An error occurred while sending the email: {ex.Message}";
				return View(model);
			}
		}

		ViewBag.Message = "Please fill out all information before submitting!";
		return View(model);
	}

	[HttpGet]
	public IActionResult ShowMessage()
	{
		ViewBag.Message = "Password reset link has been sent to your email. Check your mailbox.";
		return View();
	}

	[HttpGet]
	public IActionResult ResetPassword(string user, string token)
	{
		ViewBag.Message = null;
		var checkedToken = _tokenRepository.CheckToken(user, token);
		if (checkedToken)
		{
			ViewBag.UserName = user;
			ViewBag.Token = token;
			return View();
		}

		return RedirectToAction("Login");
	}

	[HttpPost]
	public async Task<IActionResult> ResetPasswordAsync(ResetViewModel model)
	{
		if (ModelState.IsValid)
		{
			var checkedToken = _tokenRepository.CheckToken(model.UserName, model.Token);
			if (checkedToken)
			{
				await _repo.ResetPassWord(model);
				return RedirectToAction("Login");
			}

			return RedirectToAction("Login");
		}

		ViewBag.UserName = model.UserName;
		ViewBag.Token = model.Token;
		return View(model);
	}

	[HttpGet]
	public IActionResult ChangePass()
	{
		if (!User.Identity.IsAuthenticated || User.IsInRole("Customer"))
			return RedirectToAction("Login");

		return View();
	}

	[HttpPost]
	public async Task<IActionResult> ChangePassAsync(ChangePasswordViewModel model)
	{
		if (!User.Identity.IsAuthenticated || User.IsInRole("Customer"))
			return RedirectToAction("Login");

		if (!ModelState.IsValid) return View(model);

		var id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
		var user = User.FindFirstValue(ClaimTypes.Name);

		if (await _repo.HaveAccount(user, model.OldPassword))
		{
			await _repo.ChangePasswordUser(model, id);
			return RedirectToAction("Index", "AdmHome");
		}

		ViewBag.Message = "Your password is wrong! Please try again.";
		return View();
	}
}