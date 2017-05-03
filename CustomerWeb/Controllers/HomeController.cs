
using CustomerWeb.Helper;
using CustomerWeb.Models;
using CustomerWeb.Models.JsonModel;
using CustomerWeb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CustomerWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var serverFileName = Guid.NewGuid() + ".csv";

                    var path = Path.Combine(Server.MapPath("~/CSVFiles/"), serverFileName);

                    file.SaveAs(path);


                    //save FileName To DB
                    using (var db = new CustomerDB())
                    {
                        db.UploadedFiles.Add(new UploadedFile { FileName = fileName, ServerFileName = serverFileName });
                        db.SaveChanges();
                    }

                    TempData["SuccessUpload"] = true;
                    TempData["ServerFileName"] = serverFileName;
                    TempData["FileName"] = fileName;

                    return RedirectToAction("SendToAPIAsync");

                }
                else
                {
                    return RedirectToAction("Index");

                }

            }

            else
            {
                return RedirectToAction("Index");


            }

        }

        public async Task<ActionResult> SendToAPIAsync()
        {
            var db = new CustomerDB();

            ViewBag.SuccessUpload = TempData["SuccessUpload"];
            string serverFileName = TempData["ServerFileName"].ToString();
            string fileName = TempData["FileName"].ToString();

            string filePath = Server.MapPath(Url.Content("~/CSVFiles/" + serverFileName));

            var records = System.IO.File.ReadAllLines(filePath);

            var h = new HttpHandler();
            var hashs = new List<string>();
            UploadResults model = new UploadResults { RecordsCount = 0, FaildCount = 0, FileName = fileName, SuccessCount = 0 };

            #region SendData

            foreach (var item in records)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    model.RecordsCount++;

                    //for inCorrect Data Format
                    var detail = new Details();
                    try
                    {
                        var dt = item.Split(',');
                        detail = new Details { customer = dt[0].Trim(), value = int.Parse(dt[1].Trim()), file = fileName, action = "order created", property = "Mohammad" };
                    }
                    catch
                    {
                        continue;
                    }

                    //Send Data
                    var responce = await h.SendDetailsToAPIAsync(detail);


                    if (responce.hash.Length > 50)
                    {
                        responce.hash = "NOT Valid Hash";
                        responce.added = false;
                    }

                    db.CustomersInfoes.Add(new CustomersInfo { customer = detail.customer, value = detail.value, file = detail.file, added = responce.added, errors = string.Join(",", responce.errors.ToArray()), action = detail.action, hash = responce.hash, property = detail.property });
                    db.SaveChanges();

                    hashs.Add(responce.hash);
                }
            }
            #endregion SendData

            hashs = db.CustomersInfoes.Select(o => o.hash).ToList();
            //check Customer Data
            foreach (var hash in hashs)
            {
                CustomersInfo responce;
                try
                {
                    responce = await h.CheckCustommerAsync(hash);
                }
                catch (Exception e)
                {
                    try
                    {
                        var notfounItem = db.CustomersInfoes.FirstOrDefault(o => o.hash == hash);
                        notfounItem.errors = "Hash not found";
                        notfounItem.added = false;
                        db.SaveChanges();
                    }
                    catch
                    {
                        continue;
                    }
                    model.FaildCount++;

                    continue;
                }

                model.SuccessCount++;

            }


            return View(model);
        }


        public ActionResult Search(string q = "")
        {
            if (!string.IsNullOrEmpty(q))
            {
                var db = new CustomerDB();

                var model = db.CustomersInfoes.Where(o => o.customer.Contains(q));
                return View(model);

            }

            return View();
        }
    }
}