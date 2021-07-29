using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dev.Net.Lib;
using Dev.Net.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Dev.Net.Controllers
{
    public class UserController : Controller
    {
        private readonly CollectedModels datacontext;
        public UserController(CollectedModels _datacontext)
        {
            this.datacontext = _datacontext;
        }
        public IActionResult Index()
        {
          
            return View();
           
        }
        public IActionResult Home()
        {
            return View();
        }

        public async Task<ActionResult> Signup()
        {
            return View();
        }
        public async Task<ActionResult> PostUser(User obj)
        {
            string error = "";
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.Surname)
                || string.IsNullOrEmpty(obj.Login) || string.IsNullOrEmpty(obj.Password)||
                string.IsNullOrEmpty(obj.Email))
            {
                error = "Please fill all the fields";
            }
            else if (obj.Password.Length < 6)
            {
                error = "password is too short";
            }
            string login = obj.Login;
            foreach (User elm in datacontext.users)
            {
                if (elm.Login== login)
                {
                    error = "login exists";
                }
            }
            var alreadyexist= (from item in datacontext.users where item.Login == login select item).Count();
            if (alreadyexist > 0)
            {
                error = "user already exists";
            }
            string email = obj.Email;     
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
            if(error==null||match.Success)
            {
                        obj.Password = SecurePasswordhasher.Hash(obj.Password);
                         datacontext.users.Add(obj);
                        datacontext.SaveChanges();
                       
            }
            else
            {
                TempData["Namak"] = error;
            }
            return Redirect("/user/LoginPage");


        }

        public async Task<ActionResult> LoginPage()
        {
            return View();
        }

      
        public async Task<ActionResult> Login (User obj)
        {
            
            var checklogin = (from elm in datacontext.users
                              where elm.Login == obj.Login
                              select elm).FirstOrDefault();
            if (checklogin == null)
            {
                return NotFound();
            }
            else if (SecurePasswordhasher.Verify(obj.Password, checklogin.Password) == false)
            {
                return Ok("password is wrong");
            }
            else
            {
                HttpContext.Session.SetInt32("Session", checklogin.Id);

                return Redirect("/User/Profile"); 
            }  
           
           

        }

        public async Task<ActionResult> Profile()
        {
            int? id = HttpContext.Session.GetInt32("Session");
            var data = (from elm in datacontext.users
                        where elm.Id == id
                        select elm).FirstOrDefault();
            ViewBag.currentUser = data;
            ViewBag.currentcar = datacontext.cars;
            return View();
        }



        public async Task <ActionResult> AllUsers(User obj)
        {
            ViewBag.currentcar = datacontext.cars;
            ViewBag.allusers = datacontext.users;
            return View();
        }
        public async Task<ActionResult> UserProfile(int id)
        {
            var userprofile = (from elm in datacontext.users
                        where elm.Id == id
                        select elm).FirstOrDefault();
            ViewBag.currentuser = userprofile;
            ViewBag.currentcar = datacontext.cars;
            return View();
        }
        public async Task<ActionResult> RemoveCars(int id)
        {
            var remove = (from elm in datacontext.cars
                             where elm.Id==id
                             select elm).FirstOrDefault();

            datacontext.Remove(remove);
            datacontext.SaveChanges();
            return Redirect("/user/userprofile");
        }
        public async Task<ActionResult> DeliteUser(int id)
        {
            var delite = (from elm in datacontext.users
                          where elm.Id == id
                          select elm).FirstOrDefault();
            var f = (from elm in datacontext.cars
                     where elm.user == delite
                     select elm).FirstOrDefault();
            datacontext.cars.Remove(f);
            datacontext.users.Remove(delite);
            datacontext.SaveChanges();
            return Redirect("/user/AllUsers");

        }
       
    }
}