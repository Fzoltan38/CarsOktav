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
    }
}
