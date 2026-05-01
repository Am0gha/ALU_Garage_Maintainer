using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ALU_DAL.Models;
using ALU_DAL;
using ALU_API.Models;
using System;
namespace ALU_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly ALU_DAL_Repository _repo;

        public CarsController(ALU_DAL_Repository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet]
        public JsonResult GetAllCars()
        {
            List<Car> cars = new List<Car>();
            try
            {
                cars = _repo.fetchCars();
            }
            catch (Exception ex)
            {
                cars = null;
                return Json(ex.Message);
            }
            // return empty list if repository returned null to avoid null JSON
            return new JsonResult(cars ?? new List<Car>());
        }

        [HttpGet]
        public JsonResult GetCarCountForClass(string carClass)
        {
            int? carCount = 0;
            try
            {
                carCount = _repo.fetchCarCountForClass(carClass);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(carCount);

        }

        [HttpGet]
        public JsonResult GetCarOfClass(string carClass)
        {
            List<Car> cars = new List<Car>();
            try
            {
                cars = _repo.fetchCarFromClass(carClass);
            }
            catch (Exception ex)
            {
                cars = null;
                return Json(ex.Message);
            }
            return Json(cars);
        }

        [HttpPost]
        public JsonResult AddCar(Car car)
        {
            int? status = null;
            try
            {
                status = _repo.addCarToDb(car);
            }
            catch (Exception ex)
            {
                status = null;
            }

            return Json(status);
            
        }

        [HttpGet]
        public JsonResult GetCarOfClassRarity(string carClass, string rarity)
        {
            List<Car>? cars = new List<Car>();
            try
            {
                cars = _repo.FetchCarFromClassRarity(carClass, rarity);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
            if (cars.Count==0 || cars is [])
            {
                return Json("Please check the class of the car as well as the rarity of the car you want to search for.");
            }
            return Json(cars);
        }

    }
}
