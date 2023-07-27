using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public Employeeinfo employee = new Employeeinfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            employee.name = Request.Form["name"];
            employee.email = Request.Form["email"];
            employee.phone = Request.Form["phone"];
            employee.address = Request.Form["address"];

            if(employee.name.Length == 0 || employee.email.Length == 0 ||
                employee.phone.Length==0 || employee.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "insert into tbl_Employee"+
                    "(name,email,phone,address) values (@name,@email,@phone,@address)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", employee.name);
                    command.Parameters.AddWithValue("@email", employee.email);
                    command.Parameters.AddWithValue("@phone", employee.phone);
                    command.Parameters.AddWithValue("@address", employee.address);

                    command.ExecuteNonQuery();
                }
            }

            employee.name = ""; employee.email = ""; employee.phone = ""; employee.address = "";
            successMessage = "New Employee Added Successfully";
            Response.Redirect("/Employees/Index");

        }
    }
}
