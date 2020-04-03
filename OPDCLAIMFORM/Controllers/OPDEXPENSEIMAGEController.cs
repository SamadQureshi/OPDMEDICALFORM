using OPDCLAIMFORM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPDCLAIMFORM.Controllers
{
    public class OPDEXPENSEIMAGEController : Controller
    {
        private readonly MedicalInfoEntities db = new MedicalInfoEntities();
        // GET: OPDEXPENSEIMAGE
        public ActionResult Index(int id, String opdType)
        {
            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                ViewData["OPDEXPENSE_ID"] = id;
                ViewData["OPDTYPE"] = opdType;
                ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<OPDEXPENSE_IMAGEOBJ>() };

                // Settings.
                model.ImgLst = this.db.GET_OPDEXPENSE_IMAGE().Select(p => new OPDEXPENSE_IMAGEOBJ
                {
                    FileId = p.IMAGE_ID,
                    FileName = p.IMAGE_NAME,
                    FileContentType = p.IMAGE_EXT,
                    ExpenseAmount = p.EXPENSE_AMOUNT,
                    ExpenseName = p.NAME_EXPENSES,
                    OPDExpense_id = p.OPDEXPENSE_ID,
                    OPDType = p.OPDTYPE
                }
                ).Where(e => e.OPDExpense_id == id).ToList();

                return this.View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

        }




        #region POST: /Img/Index

        /// <summary>
        /// POST: /Img/Index
        /// </summary>
        /// <param name="model">Model parameter</param>
        /// <returns>Return - Response information</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ImgViewModel model)
        {
            // Initialization.
            string fileContent = string.Empty;
            string fileContentType = string.Empty;

            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                model.OPDExpense_ID = Convert.ToInt32(Request.Url.Segments[3].ToString());

                if (ModelState.IsValid)
                {
                    // Converting to bytes.
                    byte[] uploadedFile = new byte[model.FileAttach.InputStream.Length];
                    model.FileAttach.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                    // Initialization.
                    fileContent = Convert.ToBase64String(uploadedFile);
                    fileContentType = model.FileAttach.ContentType;

                    // Saving info.
                    this.db.ADD_OPDEXPENSE_IMAGE(model.OPDExpense_ID, model.FileAttach.FileName, fileContentType, fileContent, model.ExpenseName, model.ExpenseAmount);
                }

                // Settings.
                model.ImgLst = this.db.GET_OPDEXPENSE_IMAGE().Select(p => new OPDEXPENSE_IMAGEOBJ
                {
                    FileId = p.IMAGE_ID,
                    FileName = p.IMAGE_NAME,
                    FileContentType = p.IMAGE_EXT,
                    ExpenseAmount = p.EXPENSE_AMOUNT,
                    ExpenseName = p.NAME_EXPENSES,
                    OPDExpense_id = p.OPDEXPENSE_ID,
                    OPDType = p.OPDTYPE

                }).Where(e => e.OPDExpense_id == model.OPDExpense_ID).ToList();

                // Info
                return this.View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        #endregion


        #region Download file methods

        #region GET: /Img/DownloadFile

        /// <summary>
        /// GET: /Img/DownloadFile
        /// </summary>
        /// <param name="fileId">File Id parameter</param>
        /// <returns>Return download file</returns>
        public ActionResult DownloadFile(int fileId)
        {
            // Model binding.
            ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<OPDEXPENSE_IMAGEOBJ>() };

           // try
           // {
                // Loading dile info.
                var fileInfo = this.db.GET_OPDEXPENSE_IMAGE_DETAILS(fileId).First();

                // Info.
                return this.GetFile(fileInfo.IMAGE_BASE64, fileInfo.IMAGE_EXT);
            //}
            //catch (Exception ex)
            //{
                // Info
                //Console.Write(ex);
            //}

            // Info.
            //return this.View(model);
        }


        // POST: OPDEXPENSEIMAGE/Delete/5
        public ActionResult Delete(int id , int opdexpenseid)
        {

            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                // Model binding.
                ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<OPDEXPENSE_IMAGEOBJ>() };

                var fileInfo = this.db.DELETE_OPDEXPENSE_IMAGE(id);

                // Info.
                return RedirectToAction("Index", "OPDEXPENSEIMAGE", new { id = opdexpenseid });
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        [HttpPost]
        public ActionResult DeleteOPDEXPENSEIMAGE(int id)
        {
           try
            {

                // Loading dile info.
                var fileInfo = this.db.DELETE_OPDEXPENSE_IMAGE(id);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return new EmptyResult();
        }


        #endregion

        #endregion

        #region Helpers

        #region Get file method.

        /// <summary>
        /// Get file method.
        /// </summary>
        /// <param name="fileContent">File content parameter.</param>
        /// <param name="fileContentType">File content type parameter</param>
        /// <returns>Returns - File.</returns>
        private FileResult GetFile(string fileContent, string fileContentType)
        {
            // Initialization.
            FileResult file;
            try
            {
                // Get file.
                byte[] byteContent = Convert.FromBase64String(fileContent);
                file = this.File(byteContent, fileContentType);
            }
            catch (Exception ex)
            {
                // Info.
                throw ex;
            }

            // info.
            return file;
        }

        #endregion

        #endregion
        private void AuthenticateUser()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();

            ViewBag.RollType = managerController.AuthenticateUser();

            ViewBag.UserName = managerController.GetName();

        }



    }
}