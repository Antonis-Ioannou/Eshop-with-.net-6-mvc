using CirceBook.DataAccess.Repository;
using CirceBook.DataAccess.Repository.IRepository;
using CirceBook.Models;
using CirceBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Utilities;
using PagedList;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CirceBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchBy, string search)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

            if (searchBy == "Title" && search != null)
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType").Where(x => x.Title.ToUpper().Contains(search.ToUpper()) || search == null);

			}
			if (searchBy == "Author" && search != null)
			{
				productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType").Where(x => x.Author.ToUpper().Contains(search.ToUpper()) || search == null);

			}
			if (searchBy == "Category" && search != null )
			{
				productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType").Where(x => x.Category.Name.ToUpper().Contains(search.ToUpper()) || search == null);

			}

			return View(productList);
        }

        public IActionResult Details(int productId)
		{
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId= productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, includeProperties: "Category,CoverType")
            };

			return View(cartObj);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                x => x.ApplicationUserId == claim.Value && x.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //--new code--//
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        #endregion
    }
}