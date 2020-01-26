using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HealthUp.Controllers.RemoteValidation
{
    [AjaxOnly]
    public class Validation_FilesController : BaseController
    {
        private readonly HealthUpContext _context;
        public Validation_FilesController(HealthUpContext context)
        {
            _context = context;
        }

        public JsonResult IsValidFotografiaDivulgacao(string FotografiaDivulgacao)
        {
            if (Path.GetExtension(FotografiaDivulgacao) != ".jpg")
            {
                return Json(new string("A fotografia tem de ser no formato .jpg"));
            }
            return Json(true);
        }

        public JsonResult IsValidVideoDivulgacao(string VideoDivulgacao)
        {
            if (Path.GetExtension(VideoDivulgacao) != ".mp4")
            {
                return Json(new string("A fotografia tem de ser no formato .mp4"));
            }
            return Json(true);
        }
    }
}