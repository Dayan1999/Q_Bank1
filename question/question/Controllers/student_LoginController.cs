using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using question.Models;
using System.Text;

namespace question.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class student_LoginController : ControllerBase
    {
        private string connectionString = "Data Source=LAPTOP-06GAVR62\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True";

        // POST api/login
        [HttpPost]
        public ActionResult Post(Student_loginModel model)
        {
            // Check if email exists in the database
            if (!IsEmailExists(model.Student_email))
            {
                return BadRequest("Invalid email address.");
            }

            // Retrieve the password from the database
            string passwordHash = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Password FROM UserInf WHERE Email=@Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Student_email);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        passwordHash = result.ToString();
                    }
                }
            }

            // Decrypt the password
            string password = "";
            if (!string.IsNullOrEmpty(passwordHash))
            {
                byte[] passwordBytes = Convert.FromBase64String(passwordHash);
                password = Encoding.UTF8.GetString(passwordBytes);
            }

            // Check if the password matches
            if (password != model.Student_password)
            {
                return BadRequest("Invalid password.");
            }

            return Ok("Login successful."); 
        }

        private bool IsEmailExists(string email)
        {
            // Check if email exists in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM UserInf WHERE Email=@Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
