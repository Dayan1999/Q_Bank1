using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log_Reg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private string connectionString = "Data Source=LAPTOP-06GAVR62\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True";

        // POST api/register
        [HttpPost]
        public ActionResult Post(StudentModel model)
        {
            // Validate email format and domain name
            if (!IsEmailValid(model.Student_email) || !IsDomainValid(model.Student_email))
            {
                return BadRequest("Invalid email address.");
            }

            // Validate password length and confirm password
            if (model.Student_password.Length < 6 || model.Student_password != model.Confirm_Password)
            {
                return BadRequest("Password should be at least 6 characters long and match the confirm password.");
            }

            // Check if email already exists in the database
            if (IsEmailExists(model.Student_email))
            {
                return BadRequest("Email address already registered.");
            }

            // Encrypt password using Base64 string encoding
            string encryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.Student_password));

            // Insert data into database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO UserInf (StudentName, StudentEmail, StudentPassword, MobileNo, SchoolName) " +
                               "VALUES (@StudentName, @StudentEmail, @StudentPassword, @MobileNo, @SchoolName)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentName", model.Student_name);
                    command.Parameters.AddWithValue("@StudentEmail", model.Student_email);
                    command.Parameters.AddWithValue("@StudentPassword", encryptedPassword);
                    command.Parameters.AddWithValue("@MobileNo", model.mobile_no);
                    command.Parameters.AddWithValue("@SchoolName", model.school_name);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        return StatusCode(500, "An error occurred while inserting data into the database.");
                    }
                }
            }

            return Ok("Account created successfully.");
        }

        private bool IsEmailValid(string email)
        {
            // TODO: Implement email format validation
            return true;
        }

        private bool IsDomainValid(string email)
        {
            // TODO: Implement email domain validation
            return true;
        }

        private bool IsEmailExists(string email)
        {
            // Check if email exists in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM UserInf WHERE StudentEmail=@Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(string email)
        {
            try
            {
                // Check if email is provided
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required.");
                }

                // Retrieve user information from the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM UserInf WHERE StudentEmail=@Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Check if user exists
                        if (reader.HasRows)
                        {
                            // Read user data
                            reader.Read();

                            // Map the data to your user model
                            StudentModel user = new StudentModel
                            {
                                student_id = Convert.ToInt32(reader["StudentId"]),
                                Student_name = reader["StudentName"].ToString(),
                                Student_email = reader["StudentEmail"].ToString(),
                                mobile_no = Convert.ToInt32(reader["MobileNo"]),
                                school_name = Convert.ToInt32(reader["SchoolName"]),
                            };

                            // Close the reader and return the user data
                            reader.Close();
                            return Ok(user);
                        }
                        else
                        {
                            return NotFound("User not found.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        [HttpGet("GetAllUsers")]
        //077 2439 776
        public IActionResult GetAllUsers()
        {
            try
            {
                List<StudentModel> users = new List<StudentModel>();

                // Retrieve all users from the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM UserInf";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Iterate over the results and map them to user models
                        while (reader.Read())
                        {
                            StudentModel user = new StudentModel
                            {
                                student_id = Convert.ToInt32(reader["StudentId"]),
                                Student_name = reader["StudentName"].ToString(),
                                Student_email = reader["StudentEmail"].ToString(),
                                mobile_no = Convert.ToInt32(reader["MobileNo"]),
                                school_name = Convert.ToInt32(reader["SchoolName"]),
                            };

                            users.Add(user);
                        }

                        // Close the reader
                        reader.Close();
                    }
                }

                // Check if any users were found
                if (users.Count > 0)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound("No users found.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
    }
}
