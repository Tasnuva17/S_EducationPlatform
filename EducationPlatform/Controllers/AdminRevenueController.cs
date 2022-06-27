using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationPlatform.Auth;
using EducationPlatform.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace EducationPlatform.Controllers
{
    [AdminLogged]
    public class AdminRevenueController : Controller
    {
        // GET: AdminRevenue
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Revenue()
        {
            var db=new EducationPlatformEntities();
            var sellingAmount=(from i in db.Transactions
                                    where ( i.Id>0)
                                    select i.CreditedAmount).Sum();
           
            var myEarning = 0.6 * sellingAmount;
            var varsityEarning = 0.4 * sellingAmount;
            
            ViewBag.sellingAmount = sellingAmount;
            ViewBag.myEarning = myEarning;
            ViewBag.varsityEarning = varsityEarning;


            
            var InstitutionList = db.Institutions.ToList();
            return View(InstitutionList);



        }

        public ActionResult InstitutionRevenue(int id)
        {
            ViewBag.institutionId = id;
            var db = new EducationPlatformEntities();
            var sellingAmount = (from i in db.Transactions
                                 where (i.InstitutionId ==id)
                                 select i.CreditedAmount).Sum();

            var totalSellingCourse = (from i in db.Transactions
                                 where (i.InstitutionId == id)
                                 select i.Id).Count();

            var varsityName = (from i in db.Institutions
                               where i.Id == id
                               select i.Name).FirstOrDefault();
            
            var myEarning = 0.6 * sellingAmount;
            var varsityEarning = 0.4 * sellingAmount;

            ViewBag.varsityName = varsityName;
            ViewBag.totalSellingCourse = totalSellingCourse;
            ViewBag.sellingAmount = sellingAmount;
            ViewBag.myEarning = myEarning;
            ViewBag.varsityEarning = varsityEarning;
            
            return View();
        }

        public ActionResult InstitutionRevenueReportPdf(int id)
        {
            var db = new EducationPlatformEntities();

            var sellingAmount = (from i in db.Transactions
                                 where (i.InstitutionId == id)
                                 select i.CreditedAmount).Sum();

            var totalSellingCourse = (from i in db.Transactions
                                      where (i.InstitutionId == id)
                                      select i.Id).Count();

            var varsityName = (from i in db.Institutions
                               where i.Id == id
                               select i.Name).FirstOrDefault();

            var myEarning = 0.6 * sellingAmount;
            var varsityEarning = 0.4 * sellingAmount;

            var dateime = DateTime.Now.ToString("dddd, dd MMMM yyyy").ToString();


            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Create a DataTable.
            System.Data.DataTable dataTable = new DataTable();
            //Add columns to the DataTable
            dataTable.Columns.Add("particular:");
             dataTable.Columns.Add("Details");
            // dataTable.Columns.Add("Name");
            //Add rows to the DataTable.
            dataTable.Rows.Add(new object[] { "Print Time", dateime });
            dataTable.Rows.Add(new object[] { "Varsity Id", id });
            dataTable.Rows.Add(new object[] { "Varsity Name", varsityName });
            dataTable.Rows.Add(new object[] { "Total Selling Amount", sellingAmount +" BDT" });
            dataTable.Rows.Add(new object[] { "ABC Education Earning", myEarning+ " BDT" });
            dataTable.Rows.Add(new object[] { varsityName+" Earning", varsityEarning +" BDT" });
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new PointF(10, 10));
            // Open the document in browser after saving it
           
            doc.Save(varsityName+ dateime+".pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            //close the document
            doc.Close(true);
            return View();
        }
    }
}