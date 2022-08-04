using BookShop.DataAccess.Repository.IRepository;
using BookShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopWeb.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _uniOfWork;

		public ShoppingCartViewComponent(IUnitOfWork uniOfWork)
		{
			_uniOfWork = uniOfWork;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			if(claim != null)
			{
				if(HttpContext.Session.GetInt32(StaticDetails.SessionCart)!= null)
				{
					return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
				}
				else
				{
					HttpContext.Session.SetInt32(StaticDetails.SessionCart,
						_uniOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
					return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
				}
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
	}
}
