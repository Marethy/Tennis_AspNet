using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tennis.Models;

public class Blog
{
	[Key]
	[DisplayName("Mã bài viết")]
	public int BlogId { get; set; }

	[Required]
	[DisplayName("Tên bài viết")]
	public string? BlogName { get; set; }

	[DisplayName("Nội dung")]
	public string? BlogContent { get; set; }
		
	[DisplayName("Hình ảnh")]
	public string? BlogImage { get; set; }

	[DisplayName("Ngày thêm")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	public DateTime BlogDateCreated { get; set; } = DateTime.Now;

	// Computed property để hiển thị nội dung giới hạn
	public string ShortContent =>
		!string.IsNullOrEmpty(BlogContent) && BlogContent.Length > 500
			? BlogContent.Substring(0, 500) + "..."
			: BlogContent ?? string.Empty;
}