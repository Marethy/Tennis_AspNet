﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tennis.Models;

public class Admin
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int AdminId { get; set; }

	[Required]
	[StringLength(50)]
	[DisplayName("Tài khoản")]
	public string AdminUserName { get; set; }

	[Required]
	[StringLength(50)]
	[DisplayName("Mật khẩu")]
	public string AdminPassword { get; set; }

	[DisplayName("Email")] public string? AdminEmail { get; set; }

	[DisplayName("Hình đại diện")] public string? AdminImage { get; set; }

	[DisplayName("Ngày khởi tạo")] public DateTime AdminDateCreated { get; set; } = DateTime.Now;
}