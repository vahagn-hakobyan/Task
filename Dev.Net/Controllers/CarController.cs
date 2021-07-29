using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dev.Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Net.Controllers
{
    public class CarController : Controller
    {
        private readonly CollectedModels datacontext;
        public CarController(CollectedModels _datacontext)
        {
            this.datacontext = _datacontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task <ActionResult> AllCars(Car obj)
        {
            ViewBag.currentcar = datacontext.cars;
            return View();
        }
        public async Task<ActionResult> AddCar(Car obj)
        {
            return View();
        }
        public async Task <ActionResult> AddNewCar(Car obj)
        {
            

            datacontext.cars.Add(obj);
            datacontext.SaveChanges();
            return Redirect("/Car/AllCars");
        }
    }
}