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

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return Redirect("Login");
        }

        public ActionResult Transaction()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Transaction(Transaction obj)
        {
            var db = new EducationPlatformEntities();
            var crseId = obj.CourseId;

            var a=(from b in db.Courses where b.Id==crseId
                   select b).FirstOrDefault();
            var instituteId = a.InstitutionId;

            var coursePrice =a.Price.ToString();
            var coursePriceInt = Int32.Parse(coursePrice);
          //  var date = DateTime.Now.ToString("MM/dd/yyyy");


            var trans = new Transaction()
            {
                CourseId = obj.CourseId,
                StudentId = obj.StudentId,
                InstitutionId=instituteId,
                CoursePrice = coursePriceInt,
                CreditedAmount=obj.CreditedAmount,
               // Date=date,

                BkashTransactionNumber=obj.BkashTransactionNumber,


            };
            db.Transactions.Add(trans);
            db.SaveChanges();

            
            return RedirectToAction("Index","Home");
        }

        public ActionResult PaymentHistory()
        {
            var sid= Int32.Parse(Session["Id"].ToString());

            var db = new EducationPlatformEntities();

            var trans = (from a in db.Transactions where a.StudentId == sid select a).ToList();

            return View(trans);
        }

        public ActionResult EnrollCourses()
        {
            var sid = Int32.Parse(Session["Id"].ToString());
            var db = new EducationPlatformEntities();

            var crs = (from a in db.ValidStudents where a.StudentId == sid select a).ToList();

            return View(crs);
        }

        public ActionResult CourseDetail(int id)
        {
            //var sid = Int32.Parse(Session["Id"].ToString());

            var db = new EducationPlatformEntities();

            var coursename=(from a in db.Courses where a.Id == id select a).FirstOrDefault();
            ViewBag.Crseid = coursename.Id;
            ViewBag.CrseName = coursename.Name;
            ViewBag.Detail = coursename.Details;
            ViewBag.insId = coursename.InstitutionId;
            return View();
        }

        public ActionResult CourseModule(int id)
        {
            var db = new EducationPlatformEntities();

            var coursedetails = (from a in db.CourseDetails where a.CourseId == id select a).ToList();
            return View(coursedetails);
        }

        public ActionResult SeeAssignment(int id)
        {
            var db = new EducationPlatformEntities();

            var asgnmntDetail = (from a in db.Assignments where a.CourseId == id select a).ToList();
            return View(asgnmntDetail);

        }

        public ActionResult UploadAssignment(int id)
        {
            Session["assignmentId"]=id;
            return View();
        }
        [HttpPost]
        public ActionResult UploadAssignment(AnswerScript answ)
        {
            var db = new EducationPlatformEntities();
            //var crseId = obj.CourseId;
            var assignmentId =Int32.Parse(Session["assignmentId"].ToString());
            var sid = Int32.Parse(Session["Id"].ToString());
            var date = DateTime.Now;


            var ans = new AnswerScript()
            {
                AssignmentId= assignmentId,

                Answer= answ.Answer,
                StudentId= sid,
                Date=date,




            };
            db.AnswerScripts.Add(ans);
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult VideoConference(int id)
        {
            var db = new EducationPlatformEntities();

            var x = (from a in db.Counselings where a.CourseId == id select a).ToList().LastOrDefault();
            
            return View(x);
        }

        public ActionResult ApplyingCertificate(int id)
        {
            var db = new EducationPlatformEntities();

            //var db = new EducationPlatformEntities();
            //var crseId = obj.CourseId;
            //var assignmentId = Int32.Parse(Session["assignmentId"].ToString());
            var sid = Int32.Parse(Session["Id"].ToString());
            var date = DateTime.Now;

            var x=(from a in db.Certificates where a.ApplierId==sid && a.CourseId == id select a).FirstOrDefault();
            if (x == null)
            {
                var cer = new Certificate()
                {
                    ApplierId = sid,

                    CourseId = id,
                    Date = date,




                };
                db.Certificates.Add(cer);
                db.SaveChanges();

                return View("index");
            }
            TempData["CerMsg"] = "already  applied for certificate";
            return View("Index");

        }



    }
}