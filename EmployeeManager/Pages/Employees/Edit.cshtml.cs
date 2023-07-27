using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Pages.Employees
{
    public class EditModel : PageModel
    {
        public Employeeinfo employee = new Employeeinfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from tbl_Employee where id=@id";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@id",id);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employee.id = "" + reader.GetInt32(0);
                                employee.name =reader.GetString(1);
                                employee.email =reader.GetString(2);
                                employee.phone = reader.GetString(3);
                                employee.address =reader.GetString(4);

                                
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public void OnPost()
        {
            employee.name = Request.Form["name"];
            employee.email = Request.Form["email"];
            employee.phone = Request.Form["phone"];
            employee.address = Request.Form["address"];
            employee.id = Request.Form["id"];

            if (employee.name.Length == 0 || employee.email.Length == 0 ||
                employee.phone.Length == 0 || employee.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string storedProcedure = "sp_Employee";
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", employee.name);
                    command.Parameters.AddWithValue("@email", employee.email);
                    command.Parameters.AddWithValue("@phone", employee.phone);
                    command.Parameters.AddWithValue("@address", employee.address);
                    command.Parameters.AddWithValue("@id", employee.id);

                    command.ExecuteNonQuery();
                }
            }

            employee.name = ""; employee.email = ""; employee.phone = ""; employee.address = "";
            
            Response.Redirect("/Employees/Index");

        }
    }
}
