using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CarsApi.Controllers
{
    [Route("cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        public const string ConnectionString = "Server=localhost;Database=trader;Uid=root; Password=;SslMode=None";
        [HttpPost]
        public object AddNewCar(Car car)
        {
            try
            {
                string sql = "INSERT INTO `cars`(`Brand`, `Type`, `Color`, `Year`) VALUES (@brand,@type,@color,@year)";

                using (var connect = new MySqlConnection(ConnectionString))
                {
                    connect.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connect);

                    cmd.Parameters.AddWithValue("@brand", car.Brand);
                    cmd.Parameters.AddWithValue("@type", car.Type);
                    cmd.Parameters.AddWithValue("@color", car.Color);
                    cmd.Parameters.AddWithValue("@year", car.Year);

                    cmd.ExecuteNonQuery();
                    connect.Close();
                }

                return new { message = "Sikres felvétel.", result = car };
            }
            catch (Exception ex)
            {
                return new { message = "Sikretelen felvétel.", result = ex.Message };
            }
          
        }

        [HttpGet]
        public object GetCar()
        {
            try
            {
                List<Car> cars = new List<Car>();
                string sql = "SELECT * FROM `cars`";

                using (var connect = new MySqlConnection(ConnectionString))
                {
                    connect.Open();

                    MySqlCommand cmd = new MySqlCommand(sql,connect);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var car = new Car 
                        { 
                            Id = reader.GetInt32(0),
                            Brand = reader.GetString(1),
                            Type = reader.GetString(2),
                            Color = reader.GetString(3),
                            Year = reader.GetInt32(4)
                        };

                        cars.Add(car);
                    }
                    
                    connect.Close();
                    return new { message = "Sikeres lekérdezés", result = cars};
                }
            }
            catch (Exception ex)
            {
                return new { message = ex.Message, result = "" };
            }
        }

        [HttpGet("byid")]
        public object GetCarById(int id)
        {
            try
            {
                string sql = "SELECT * FROM `cars` WHERE Id = @id";

                using (var connect = new MySqlConnection(ConnectionString))
                {
                    connect.Open();

                    MySqlCommand cmd = new MySqlCommand(sql,connect);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader(); 

                    if(reader.Read()==true)
                    {
                        var car = new Car
                        {
                            Id = reader.GetInt32(0),
                            Brand = reader.GetString(1),
                            Type = reader.GetString(2),
                            Color = reader.GetString(3),
                            Year = reader.GetInt32(4)
                        };

                        return Ok(new { message = "Sikeres találat", result = car });
                    }

                    connect.Close();

                    return NotFound(new { message = "Sikertelen találat", result = "" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message, result = "" });
            }
        }

        [HttpDelete]
        public object DeleteCarById(int id) 
        {
            try
            {
                string sql = "DELETE FROM `cars` WHERE Id = @id";

                using (var connect = new MySqlConnection(ConnectionString))
                {
                    connect.Open();

                    MySqlCommand cmd = new MySqlCommand(sql,connect);

                    cmd.Parameters.AddWithValue("@id", id);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return Ok(new { message = "Sikeres törlés.", result = "" });
                    }

                    connect.Close();

                    return NotFound(new { message = "Nincs ilyen autó.", result = "" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, result = "" });
            }
        }
    }
}
