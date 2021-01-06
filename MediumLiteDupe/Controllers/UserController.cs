using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repository.IRepository;
using EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediumLiteDupe.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(SignInManager<AppUser> signInManager,ILogger<UserController> logger,UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: User
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return Redirect("/Identity/Account/Login");
        }

        public ActionResult EditProfile()
        {
            AppUser curUser = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
            if (curUser.ImageId.HasValue)
               curUser.UserImage = _unitOfWork.UserImage.Get(curUser.ImageId.Value);
            return View(curUser);
        }
        [HttpPost]
        public ActionResult EditProfile(string field, string val)
        {
            string msg = "Error while updating profile" ;
            bool isSuccess = false;
            if (field == "Name" && string.IsNullOrWhiteSpace(val))
            {
                msg = "Please enter user name";
            }
            else
            {
                AppUser curUser = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
                //string UserId = _userManager.GetUserId(HttpContext.User);
                //var user = _userManager.FindByIdAsync(UserId);
                if (field == "Name")
                    curUser.Name = val;
                else if (field == "Bio")
                    curUser.BioDescription = val;
                var result = _userManager.UpdateAsync(curUser).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    if (field == "Name")
                    {
                        Claim claim = User.FindFirst("FullName");
                        Claim newClaim = new Claim("FullName", val);
                        _userManager.RemoveClaimAsync(curUser, claim).Wait();
                        _userManager.AddClaimAsync(curUser, newClaim).Wait();
                    }
                    isSuccess = true;
                    msg = $"{field} is updated successfully";
                }
            }
            return Json(new { success = isSuccess, message = msg });
        }

        [HttpPost]
        public ActionResult SaveProfilePic()
        {
            string msg = "Error while updating profile picture";
            string profileImgSrc = "";
            bool isSuccess = false;
            var files = HttpContext.Request.Form.Files;
            UserImage image = new UserImage();
            
            if (files != null && files.Count == 1)
            {
                var imageFile = files[0];
                if (imageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        imageFile.CopyTo(ms);
                        image.Data = ms.ToArray();
                    }
                    image.Name = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    image.ExtensionType = Path.GetExtension(imageFile.FileName).Remove(0,1);
                    try
                    {
                        AppUser curUser = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
                        if (curUser.ImageId.HasValue && curUser.ImageId.Value > 0)
                        {
                            image.Id = curUser.ImageId.Value;
                            _unitOfWork.UserImage.Update(image);
                            _unitOfWork.Save();
                            msg = "Your profile picture is update successfully";
                            isSuccess = true;
                        }
                        else
                        {
                            _unitOfWork.UserImage.Add(image);
                            _unitOfWork.Save();
                            curUser.ImageId = image.Id;
                            _userManager.UpdateAsync(curUser);
                            var result = _userManager.UpdateAsync(curUser).GetAwaiter().GetResult();
                            if (result.Succeeded)
                            {
                                msg = "Your profile picture is update successfully";
                                isSuccess = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                                                
                    }                    
                }
                
                if (isSuccess)
                {
                    var base64 = Convert.ToBase64String(image.Data);
                    profileImgSrc = String.Format("data:image/{0};base64,{1}", image.ExtensionType, base64);
                }

            }
            return Json(new { success = isSuccess, message = msg, imageSrc = profileImgSrc });
        }

        public ActionResult Stories()
        {
            int UserProfileId = Convert.ToInt32(User.FindFirst("ProfileId").Value);
            List<Story> stories = _unitOfWork.Story.GetAll(x => x.Status == 1 && x.UserProfileId == UserProfileId, orderBy: x => x.OrderByDescending(y => y.CreatedOn)).ToList();
            ViewBag.PublishCount = _unitOfWork.Story.GetAll(x => x.Status == 2 && x.UserProfileId == UserProfileId).Count();
            return View(stories);
        }
        public ActionResult WriteStory(int? id)
        {
            int UserProfileId = Convert.ToInt32(User.FindFirst("ProfileId").Value);
            Story model = new Story();
            if (id.HasValue && id.Value > 0)
            {
                model = _unitOfWork.Story.GetAll(x => x.Id == id && x.UserProfileId == UserProfileId, includProperties: "StoryData").FirstOrDefault();
                //byte array to string
                model.StoryContent = Encoding.ASCII.GetString(model.StoryData.Data);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult WriteStory(Story model)
        {            
            model.CreatedOn = DateTime.Now;
            model.ModifiedOn = DateTime.Now;
            
            //string to byte array            
            byte[] bytes = Encoding.ASCII.GetBytes(model.StoryContent ?? "");
            if (model.Id > 0)
            {
                model.StoryData = new StoryData { Data = bytes };
                _unitOfWork.Story.Update(model);
            }
            else
            {
                model.UserProfileId = Convert.ToInt32(User.FindFirst("ProfileId").Value);
                model.StoryData = new StoryData { ContentType = "Text", Data = bytes };
                _unitOfWork.Story.Add(model);
            }
            _unitOfWork.Save();
            
            return RedirectToAction("Stories");
        }

        public ActionResult ViewStory(int id)
        {
            var model = _unitOfWork.Story.GetAll(x => x.Id == id, includProperties: "StoryData").FirstOrDefault();
            //byte array to string
            model.StoryContent = Encoding.ASCII.GetString(model.StoryData.Data);
            return View(model);
        }
        public ActionResult DeleteStory(int id)
        {
            _unitOfWork.Story.Remove(id);
            _unitOfWork.Save();
            return RedirectToAction("Stories");
        }

        public PartialViewResult GetStories(string id)
        {
            int UserProfileId = Convert.ToInt32(User.FindFirst("ProfileId").Value);
            string _view = "_DraftStories";
            List<Story> stories = null;
            if (id == "P")
            {
                stories = _unitOfWork.Story.GetAll(x => x.Status == 2 && x.UserProfileId == UserProfileId, orderBy: x => x.OrderByDescending(y => y.CreatedOn)).ToList();
                _view = "_PublishedStories";
            }
            else
            {
                stories = _unitOfWork.Story.GetAll(x => x.Status == 1 && x.UserProfileId == UserProfileId, orderBy: x => x.OrderByDescending(y => y.CreatedOn)).ToList();            
            }
            return PartialView(_view, stories);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}