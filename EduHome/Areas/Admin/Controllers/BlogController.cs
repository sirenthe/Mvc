using EduHome.Areas.Admin.ViewModels.BlogViewModel;
using EduHome.Areas.Admin.ViewModels.SliderViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduHome.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]


    public class BlogController : Controller


    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {

            var blog = await _context.Blogs.ToListAsync();

            return View(blog);
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create()

        {

            return View();
        }
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBlogViewModel createblogViewModel)
        {


            if (!ModelState.IsValid)
                return View();
            if (!createblogViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Img", "sekil daxil edin");
                return View();
            }
            if (!createblogViewModel.Image.CheckFileSize(2))
            {
                ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
                    ; return View();
            }
            string fileName = $"{Guid.NewGuid()}-{createblogViewModel.Image.FileName}";

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await createblogViewModel.Image.CopyToAsync(fileStream);
            }







            Blog blog = new Blog
            {
                Title = createblogViewModel.Title,
                Image = fileName,
                Description = createblogViewModel.Description,
                Author = createblogViewModel.Author,
                Time = createblogViewModel.Time,
                CommmentCount = createblogViewModel.CommmentCount,
            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == id);
            if (blog is null)
            {
                return NotFound();
            }
            UpdateBlogViewModel updateBlogViewModel = new UpdateBlogViewModel
            {
                Title = blog.Title,
                Author = blog.Author,
                Time = blog.Time,
                CommmentCount = blog.CommmentCount,
                Description = blog.Description,

                Id = blog.Id,

            };
            return View(updateBlogViewModel);
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateBlogViewModel updateBlogViewModel, int id)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
			if (blog is null)
			{
				return NotFound();
			}

			if (updateBlogViewModel.Image is not null)
			{
				if (!updateBlogViewModel.Image.CheckFileType("image/"))
				{
					ModelState.AddModelError("Img", "sekil daxil edin");
					return View();
				}
				if (!updateBlogViewModel.Image.CheckFileSize(2))
				{
					ModelState.AddModelError("Img", "seklin olcusu uygun deyil")
						; return View();
				}
				string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", blog.Image);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);

					string fileName = $"{Guid.NewGuid()}-{updateBlogViewModel.Image.FileName}";
					string resultpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", fileName);
					using (FileStream stream = new FileStream(resultpath, FileMode.Create))
					{
						await updateBlogViewModel.Image.CopyToAsync(stream);
					}
					blog.Image = fileName;
				}
			}

			blog.Title = updateBlogViewModel.Title;
			blog.Author = updateBlogViewModel.Author;
			blog.Time = updateBlogViewModel.Time;
			blog.Description = updateBlogViewModel.Description;
			blog.CommmentCount = updateBlogViewModel.CommmentCount;

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Delete(int Id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == Id);
            if (blog is null)
            {
                return NotFound();
            }
            DeleteBlogViewModel deleteBlogViewModel = new DeleteBlogViewModel
            {
                Id = blog.Id,
                Image = blog.Image,
                Title = blog.Title,


            };
            return View(deleteBlogViewModel);

        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog is null)
                return NotFound();


            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", blog.Image);

            if (System.IO.File.Exists(path))
            {

                System.IO.File.Delete(path);
            }



            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog is null)
            {
                return NotFound();
            }
            return View(blog);
        }



    }
















}

