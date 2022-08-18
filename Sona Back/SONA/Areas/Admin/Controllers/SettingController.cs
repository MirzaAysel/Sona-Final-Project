using System;
using System.Threading.Tasks;
using Business.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace SONA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingRepository;

        public SettingController(ISettingService settingRepository)
        {
            _settingRepository = settingRepository;
        }
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _settingRepository.GetAll();
            return View(data.ToPagedList(page,3));
        }
        
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Setting setting;
            try
            {
                setting = await _settingRepository.Get(id);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = e.Message
                });
            }
            
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            Setting settingExist;
            try
            {
                settingExist = await _settingRepository.Get(setting.Id);
                setting.Key = settingExist.Key;
                await _settingRepository.Update(setting);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = e.Message
                });
            }
            return RedirectToAction("Index");
        }
    }
}