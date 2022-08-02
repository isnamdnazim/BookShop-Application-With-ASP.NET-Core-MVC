using BookShop.DataAccess;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        
        //Get
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            

            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }

            
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company Created Sucessfully";
                }

                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company Updated Sucessfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //Get
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //var categoryFromDb = _db.Categories.Find(id);
        //    var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
        //    //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c=>c.Id==id);

        //    if (CoverTypeFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(CoverTypeFromDbFirst);
        //}

        ////Post
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST(int? id)
        //{
        //    var obj = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.CoverType.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "CoverType Deleted Sucessfully";
        //    return RedirectToAction("Index");



        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });

        }

        //Post
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return Json(new {success = false, message = "Ërror while deleting"});
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successful" });
            



        }
        #endregion
    }


}
