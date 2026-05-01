using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ALU_DAL.Models;
using ALU_DAL;
using System;
using ALU_API.Models;
namespace ALU_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassController : Controller
    {
        private readonly ALU_DAL_Repository _repo;
        public ClassController(ALU_DAL_Repository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public JsonResult GetStarsForClassRarity(string cls, string rarity)
        {
            List<int>? stars = new List<int>();
            try
            {
                stars = _repo.returnValidStarsForClassRarity(cls, rarity);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }

            if (stars is null || stars.Count==0) 
            {
                return Json("Something went wrong in backend(DAL), report this to the site admin.");
            }
            else if (stars[0] == -1)
            {
                return Json("Invalid Rarity value for the given Class! Try again.");
            }
            return Json(stars);
            
        }

        [HttpGet]
        public JsonResult GetFuelEip(string cls, string rarity, int star)
        {

            FuelEip res = new FuelEip();
            try
            {
                res = _repo.returnValidFuelForClassRarityStar(cls, rarity, star);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
            //if (fuel > 0)
            return Json(res);
            
        }

        [HttpGet]
        public JsonResult GetClassFuelRange(string cls)
        {
            List<int>? fuelRange = new List<int>();

            try
            {
                fuelRange = _repo.returnMinMaxFuel(cls);
                if (fuelRange.Count == 0 || fuelRange is [])
                {
                    return Json("Check the class entered.");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(fuelRange);
        }

        [HttpGet]
        public JsonResult GetAllClasses()
        {
            FetchClasses carClasses = new FetchClasses();
            try
            {
                carClasses.classes = _repo.returnAllCarClasses();
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            if (carClasses.classes.Count == 0 || carClasses.classes is [])
            {
                return Json("Something went wrong in the DAL layer, report this issue to the admin.");
            }
            return Json(carClasses.classes);
        }

        [HttpGet]
        public JsonResult GetValidRarityForClass(string carClass)
        {
            List<string> rarity = new List<string>();
            try
            {
                rarity = _repo.returnValidRarityList(carClass);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            if (rarity.Count == 0 || rarity is [])
            {
                return Json("Please check the class of the car entered.");
            }
            return Json(rarity);
        }

        
    }
}
