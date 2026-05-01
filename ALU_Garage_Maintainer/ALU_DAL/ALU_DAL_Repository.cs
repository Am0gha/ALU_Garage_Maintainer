using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALU_DAL;
using ALU_DAL.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Primitives;
namespace ALU_DAL
{
    public class ALU_DAL_Repository
    {
        static AluContext _context;

        public ALU_DAL_Repository(AluContext context)
        {
            _context = context;
        }

        //Validates the given car details to be inserted into DB.
        public int? validateCarDetails(Car car)
        {
            int? minStar = 3;
            int? maxStar = 6;
            int? carId = 0;
            //Condition to check if the class of the given car is valid
            try
            {
                if (!_context.Classes.Select(c => c.Class1).ToList().Contains(car.Class))
                    return -1;

                else        //Valid class of the car
                {
                    minStar = _context.Classes.Where(p => p.Class1 == car.Class).Select(c => c.MinStars).FirstOrDefault();
                    maxStar = _context.Classes.Where(p => p.Class1 == car.Class).Select(c => c.MaxStars).FirstOrDefault();
                }

                //Out of bounds condition check for the car stars.
                if (car.MaxStars < minStar || car.MaxStars > maxStar)
                    return -2;

                //Checks if the car doesn't require key then the BPS required to unlock it shouldn't be null and vice verse.
                else if ((car.RequiresKey == false && car.Bp1sCount is null) || (car.RequiresKey == true && car.Bp1sCount is not null))
                    return -3;

                //Checks if the car requires EIP then it shouldn't have have NoEips field as null and vice verse.
                else if ((car.HasEips == true && car.NoEips is null) || (car.HasEips == false && car.NoEips is not null))
                    return -4;

                carId = _context.Cars.OrderBy(c => c.Id).Select(x=>x.Id).LastOrDefault();

            }
            catch(Exception)
            {
                return null;
            }

            return carId;

        }

        //Returns the valid star range given the class.
        public List<int>? fetchValidStarRangeForClass(string cls)
        {
            try
            {
                var res = _context.Classes.Where(c => c.Class1 == cls).Select(m => new { m.MinStars, m.MaxStars}).FirstOrDefault();
                return ((List<int>)([res.MinStars, res.MaxStars]));
            }
            catch(Exception)
            {
                return null;
            }

        }

        //Returns all the cars present in the database
        public List<Car> fetchCars()
        {
            List<Car> cars = new List<Car>();
            try
            {
                cars = _context.Cars.ToList();
            }
            catch (Exception)
            {
                cars = null;
            }
            return cars;
        }

        //Returns car belonging to aforementioned class.
        public List<Car> fetchCarFromClass(string carClass)
        {
            List<Car> cars = new List<Car>();
            try
            {
                //This is to check if the user input for the class from which the cars should be fetched is valid or not.
                if (_context.Classes.Select(c => c.Class1).ToList().Contains(carClass))
                {
                    cars = _context.Cars.Where(car => car.Class == carClass).ToList();
                }
            }
            catch (Exception)
            {
                cars = null;
            }
            //cars can also be empty list as well, which might suggest there are no cars present in the given class.
            return cars;
        }

        //Returns the car count for a given class.
        public int? fetchCarCountForClass(string carClass)
        {
            int? carCount = 0;
            try
            {
                if (returnCarClasses().Contains(carClass))
                {
                    carCount = _context.Cars.Where(c => c.Class == carClass).Count();
                }
                else return -1;
            }
            catch(Exception)
            {
                return null;
            }

            return carCount;
        }

        //This requires a new table altogether for rarity.
        public List<Car> FetchCarFromClassRarity(string carClass, string rarity)
        {
            List<Car> cars = new List<Car>();
            //This is a small thing that I believe doesnt require a table but will decide in future
            List<string> carRarity = ["COMM", "RARE", "EPIC"];
            try
            {
                if (_context.Classes.Find(carClass) != null && carRarity.Contains(rarity))
                {
                    cars = _context.Cars.Where(c => (c.Class == carClass && c.Rarity == rarity)).ToList();
                }
            }
            catch(Exception)
            {
                cars = null;
            }
            return cars;
            
        }

        //Adds car to DB.
        public int addCarToDb(Car newCar)
        {
            int? valid = validateCarDetails(newCar);
            if (valid != null && valid > 0)
            {
                try
                {
                    newCar.Id = (int)valid+1;
                    newCar.Name = newCar.Name.ToUpper();
                    _context.Cars.Add(newCar);
                    _context.SaveChanges();
                }
                catch (Exception)
                { return (int)valid; }
            }
            else return (int)valid;

            return (int)valid;
        }

        public List<string>? returnCarClasses()
        {
            List<string>? carClasses = [];
            try
            {
                carClasses = _context.Classes.Select(i => i.Class1).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return carClasses;
        }       //-> Returns all the valid classes

        public List<int>? returnMinMaxFuel(string cls)
        {
            List<int>? fuelRange = new List<int>();
            int minFuel = 0;
            int maxFuel = 99;

            try
            {
                if (_context.Classes.Find(cls) != null)
                {
                    minFuel = _context.Classes.Find(cls).MinFuel;
                    fuelRange.Add(minFuel);
                    maxFuel = _context.Classes.Find(cls).MaxFuel;
                    fuelRange.Add(maxFuel);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return fuelRange;
        }

        //Part 1 -> classes, rarities, stars and fuel system (solved)
        public List<string>? returnAllCarClasses()
        {
            FetchClasses carClasses = new FetchClasses();
            try
            {
                carClasses.classes = _context.Classes.Select(c => c.Class1).ToList();
            }
            catch(Exception)
            {
                return null;
            }
            return carClasses.classes;
        }

        public List<string>? returnValidRarityList(string cls)
        {
            int? rarityVal = 0;
            List<string> validRarityList = new List<string>();

            try
            {
                if (returnAllCarClasses().Contains(cls))
                {
                    //rarityVal = _context.Classes.Where(c => c.Class1 == cls).Select(i => i.ValidRarityForEip).FirstOrDefault();
                    rarityVal = _context.Classes.Find(cls).ValidRarity;
                    validRarityList = returnValidRarityForClass((int)rarityVal);
                }
                else
                    rarityVal = -1;     //-> Invalid Class Input
            }
            catch(Exception)
            {
                return null;
            }
            return validRarityList;

        }

        public List<string>? returnValidRarityForClass(int clsRarityVal)
        {
            List<string> rarity = new List<string>();
            List<Rarity> r_check = new List<Rarity>();
            try
            {
                r_check = _context.Rarities.OrderByDescending(v=>v.Value).ToList();
                //if (_context.Classes.Select(c => c.Class1).Contains(cls))
                
                //First fetch the class and the valid rarity value from the classes table
                foreach(Rarity r in r_check)
                {
                    if (clsRarityVal>=r.Value)
                    {
                        rarity.Add(r.Rarity1);
                        clsRarityVal -= r.Value;
                    }
                }
                
            }
            catch(Exception)
            {
                return null;
            }
            return rarity;
        }

        public List<int>? returnValidStarsForClassRarity(string cls, string rarity)
        {
            List<int>? stars = [];
            try
            {
                if (_context.Classes.Where(c => c.Class1 == cls).Any() &&
                   _context.Rarities.Where(r => r.Rarity1 == rarity).Any())
                {
                    if (rarity == "COMM")
                    {
                        switch (cls)
                        {
                            case "D":
                            case "C":
                            case "B": stars = [3]; break;
                            default: return [-1];   //Invalid Rarity Value
                        }
                    }
                    else if (rarity == "RARE")
                    {
                        switch (cls)
                        {
                            case "D":
                            case "C":
                            case "B":
                            case "A": stars = [4]; break;
                            default: return [-1];   //Invalid Rarity Value
                        }
                    }
                    else if (rarity == "EPIC")
                    {
                        switch (cls)
                        {
                            case "D":
                            case "C": stars = [5]; break;
                            case "B":
                            case "A":
                            case "S": stars = [5, 6]; break;
                            default: return [-1];   //Invalid Rarity Value
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;    //If return value null, exception occurred in DAL
            }
            return stars;
        }

        public int? oldReturnValidFuelForClassRarityStar(string cls, string rarity,int stars)
        {
            int? fuel = 0;
            try
            {
                bool flag1 = _context.Classes.Any(c => c.Class1 == cls);
                bool flag2 = returnValidRarityList(cls).Contains(rarity);
                bool flag3 = returnValidStarsForClassRarity(cls, rarity).Contains(stars);
                
                if (flag1 && flag2 && flag3)
                {
                    List<int>? fuelRange = returnMinMaxFuel(cls);
                    List<int>? starRange = fetchValidStarRangeForClass(cls);
                    for (int i = 0; i <= (starRange[1] - starRange[0]); i++)
                    {
                        if (stars == starRange[0] + i)
                        {
                            fuel = _context.Classes.Where(c => c.Class1 == cls).Select(f => f.MaxFuel).FirstOrDefault() - i;
                            break;
                        }
                    }
                    //We figured out the fuel the car is assigned. Now figure out the eipAmt
                    int eipVal = _context.Classes.Find(cls).ValidRarityForEip;
                    List<string> valEipRar = returnValidRarityForClass(eipVal);
                    
                }

                else if (!flag1) return -1; //Invalid Class Input
                else if (!flag2) return -2; //Invalid Rarity Input for the given class
                else if (!flag3) return -3; //Invalid Stars Input for the given class and rarity
                else return -99;
            }
            catch(Exception)
            {
                return null;
            }

            return fuel;
        }

        //Part 2 -> handling eip system for each

        public FuelEip returnValidFuelForClassRarityStar(string cls, string rarity, int stars)
        {
            FuelEip res = new FuelEip();
            try
            {
                List<int>? fuelRange = returnMinMaxFuel(cls);
                List<int>? starRange = fetchValidStarRangeForClass(cls);
                for (int i = 0; i <= (starRange[1] - starRange[0]); i++)
                {
                    if (stars == starRange[0] + i)
                    {
                        res.fuel = _context.Classes.Where(c => c.Class1 == cls).Select(f => f.MaxFuel).FirstOrDefault() - i;
                        break;
                    }
                }

                //Done with the fuel part, now figure out the eip part.
                int eipVal = _context.Classes.Find(cls).ValidRarityForEip;
                List<string> valRarity = returnValidRarityForClass(eipVal);
                if (!valRarity.Contains(rarity))
                {
                    res.eipAmt = null;
                    return res;
                }
                
                if (rarity == "COMM")
                {
                    switch(cls)
                    {
                        case "C": case "B": res.eipAmt = 4; break;
                        default: res.eipAmt = null; break;
                    }
                }
                else if (rarity == "RARE")
                {
                    switch(cls)
                    {
                        case "D": case "C":res.eipAmt = 4; break;
                        case "B": case "A":res.eipAmt = 8; break;
                        default: res.eipAmt = null; break;
                    }
                }
                else if (rarity == "EPIC")
                {
                    switch (cls)
                    {
                        case "D": case "C": res.eipAmt = 8; break;
                        case "B": res.eipAmt = (stars==5)? 8:((stars==6)? 12:-2);break;
                        case "A": case "S":res.eipAmt = (stars == 5) ? 12 : ((stars == 6) ? 16 : -2); break;
                        default: res.eipAmt = null; break;
                    }
                }
            }
            catch(Exception)
            {
                return null;
            }
            return res;

        }


    }
}
