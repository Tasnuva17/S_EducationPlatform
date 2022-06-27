using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationPlatform.Models;


namespace EducationPlatform.Controllers
{
    //EducationPlatformEntities db = new EducationPlatformEntities();
    public class S_StudentController : Controller
    {
        // GET: S_Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Student obj)
        {
           var db = new EducationPlatformEntities();
            var student = new Student()
            {
                Name = obj.Name,
                Address = obj.Address,
                Email = obj.Email,
                Phone = obj.Phone,
                Password = obj.Password,
                Gender = obj.Gender,
                Institution = obj.Institution,
                IsValid = "Yes",
                Education = obj.Education,


            };
            db.Students.Add(student);
            db.SaveChanges();

            TempData["msg"] = "Student Registration done";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Login (Student st)
        {
            var db = new EducationPlatformEntities();

            //   if (ModelState.IsValid)
            //  {
            var details = (from usaerlist in db.Students
                           where usaerlist.Email == st.Email && usaerlist.Password == st.Password

                           select usaerlist).FirstOrDefault();
                              
                if (details!= null)
                {
                    
                    Session["Id"] = details.Id;
                    Session["Name"] = details.Email;
                    return RedirectToAction("Index");
                }
            TempData["msg"] = "incorrect pass or email";
                return View();
            }
           
        

    }
}