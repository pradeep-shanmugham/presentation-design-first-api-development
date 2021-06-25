using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace resource_server.Controllers
{
    public class Car
    {
        public int id { get; set; }
        public string vinNo { get; set; }
        public Type type { get; set; }
        public decimal regularPrice { get; set; }
        public bool isInStock { get; set; }
        public bool isOnSale { get; set; }
        public decimal saleDiscountPercentage { get; set; }

    }

    [Route("cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetCars()
        {
            return Ok(SimulatingACallToADataSource());
        }

        IEnumerable<Car> SimulatingACallToADataSource()
        {
            List<Car> someProducts = new List<Car>();

            Random aRandomizer = new Random();

            //Simulating a call to a data store to get information
            for (int i = 1; i <= 20; i++)
            {
                var inStock = aRandomizer.Next(10) % 2 == 0 ? true : false;
                var onSale = aRandomizer.Next(10) % 2 == 0 ? true : false;

                someProducts.Add(new Car
                {
                    id = i,
                    isInStock = inStock,
                    isOnSale = onSale,
                    regularPrice = 50m * aRandomizer.Next(200),
                    saleDiscountPercentage =onSale ? aRandomizer.Next(60) : 0,
                    vinNo = "VINNO1----2313123123123",
                });
            }

            return someProducts;
        }
    }
}
