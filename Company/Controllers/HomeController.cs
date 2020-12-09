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
            List<Employee> employees = new List<Employee>();
            using (var dbContext = new CompanyDbEntities())
            {
                employees = dbContext.Employees.Where(obj => obj.IsDeleted == false).ToList();
                ViewBag.Message = "OK";
            }
            return View(employees);
        }

        public ActionResult New()
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

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            Employee employee;
            using (var dbContext = new CompanyDbEntities())
            {
                employee = dbContext.Employees.Where(obj => obj.Id == Id).FirstOrDefault();
            }
            return View(employee);
        }

        public ActionResult UpdateEmployee(Employee criteria)
        {
            if (criteria.Name == "")
                return RedirectToAction("Edit");
            if (criteria.Dept == "")
                return RedirectToAction("Edit");
            try
            {
                using (var dbContext = new CompanyDbEntities())
                {
                    Employee employee = dbContext.Employees.Where(obj => obj.Id == criteria.Id).FirstOrDefault();
                    employee.Name = criteria.Name;
                    employee.Dept = criteria.Dept;
                    dbContext.Entry(employee).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteEmployee(int Id)
        {
            try
            {
                using (var dbContext = new CompanyDbEntities())
                {
                    Employee employee = dbContext.Employees.Where(obj => obj.Id == Id).FirstOrDefault();
                    dbContext.Employees.Remove(employee);
                    dbContext.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

    }
}