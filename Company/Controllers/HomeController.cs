using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Controllers
{
    /*
     * EMPLOYEE
     * Id (PK, integer)
     * Name (character varying)
     * Dept (character varying)
     * CreatedDateTime (timestamp with zone)
     * IsDeleted (boolean)
     * */
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Employees()
        {
            List<Employee> employees = new List<Employee>();
            using (var dbContext = new CompanyDbEntities())
            {
                employees = dbContext.Employees.Where(obj => obj.IsDeleted == false).ToList();
                ViewBag.Message = "OK";
            }
            return View(employees);
        }

        public ActionResult NewEmployee()
        {
            return View();
        }

        public ActionResult InsertEmployee(Employee employee)
        {
            if (employee.Name == "")
                return RedirectToAction("NewEmployee");
            if (employee.Dept == "")
                return RedirectToAction("NewEmployee");
            try
            {
                using (var dbContext = new CompanyDbEntities())
                {
                    int maximumId = 0;
                    maximumId = dbContext.Employees.Max(obj => obj.Id);
                    employee.Id = maximumId + 1;
                    employee.CreatedDateTime = DateTime.Now;
                    employee.IsDeleted = false;
                    dbContext.Employees.Add(employee);
                    dbContext.SaveChanges();
                }
            }
            catch
            {

            }

            return RedirectToAction("Employees");
        }

        public ActionResult EditEmployee(int Id)
        {
            Employee employee;
            using (var dbContext = new CompanyDbEntities())
            {
                employee = dbContext.Employees.Where(obj => obj.Id == Id).FirstOrDefault();
            }
            return View(employee);
        }

        public ActionResult UpdateEmployee(string Name, String Dept)
        {
            if (Name == "")
                return RedirectToAction("EditEmployee");
            if (Dept == "")
                return RedirectToAction("EditEmployee");
            try
            {
                using (var dbContext = new CompanyDbEntities())
                {
                    Employee employee = dbContext.Employees.Where(obj => obj.Name == Name).FirstOrDefault();
                    employee.Dept = Dept;
                    dbContext.Entry(employee).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Employees");
        }

        public ActionResult DeleteEmployee(int Id)
        {
            try
            {
                using (var dbContext = new CompanyDbEntities())
                {
                    Employee employee = dbContext.Employees.Where(obj => obj.Id == Id).FirstOrDefault();
                    employee.IsDeleted = true;
                    dbContext.Entry(employee).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Employees");
        }

    }
}