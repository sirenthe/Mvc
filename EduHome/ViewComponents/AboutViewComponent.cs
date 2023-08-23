using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents
{
	public class AboutViewComponent :ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
