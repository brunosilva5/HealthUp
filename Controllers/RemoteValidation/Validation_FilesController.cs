using HealthUp.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace HealthUp.Controllers.RemoteValidation
{
    [AjaxOnly]
    public class Validation_FilesController : BaseController
    {

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