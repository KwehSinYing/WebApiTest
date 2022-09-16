
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
//using System.Web.Http;

namespace WebApiTest
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IConfiguration configuration;
        public ProductController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            using (SqlConnection conn = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("localdb").Value))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("insert into product values (@productName, @productDesc, GETDATE(), null)", conn);
                cmd.Parameters.Add(new SqlParameter("@productName", product.ProductName ));
                cmd.Parameters.Add(new SqlParameter("@productDesc", product.ProductDesc ));

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return Ok("Succeed");
                }
                else
                    return BadRequest("Failed");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get(string productName)
        {
            DataSet ds = new DataSet("Product");
            try
            {
                using (SqlConnection conn = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("localdb").Value))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SearchProduct", conn);
                    cmd.Parameters.Add(new SqlParameter("@productname", productName));
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(ds);
                    return Ok(JsonConvert.SerializeObject(ds));
                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(string productName, string productDesc)
        {
            using (SqlConnection conn = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("localdb").Value))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("update product set productDesc=@productDesc where productName = @productName", conn);
                cmd.Parameters.Add(new SqlParameter("@productDesc", productDesc));
                cmd.Parameters.Add(new SqlParameter("@productName", productName));
                cmd.CommandType = CommandType.Text;

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return Ok("Successfully updated "+ result + " row(s).");
                }
                else
                    return BadRequest("Operation failed");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string productName)
        {
            using (SqlConnection conn = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("localdb").Value))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("delete from product where productName = @productName", conn);
                cmd.Parameters.Add(new SqlParameter("@productName", productName));
                cmd.CommandType = CommandType.Text;

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return Ok("Successfully deleted " + result + " row(s).");
                }
                else
                    return BadRequest("Operation failed");
            }
        }
    }
}
