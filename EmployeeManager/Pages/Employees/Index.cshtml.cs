using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Pages.Employees
{
    public class IndexModel : PageModel
    {
        public List<Employeeinfo> employees = new List<Employeeinfo>();
        public void OnGet()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tempdb;Integrated Security=True";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select * from tbl_Employee";
                using(SqlCommand sqlCommand = new SqlCommand(sql,connection))
                {
                    using(SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Employeeinfo employeeinfo = new Employeeinfo();
                            employeeinfo.id = "" + reader.GetInt32(0);
                            employeeinfo.name =reader.GetString(1);
                            employeeinfo.email =reader.GetString(2);
                            employeeinfo.phone = reader.GetString(3);
                            employeeinfo.address = reader.GetString(4);

                            employees.Add(employeeinfo);
                        }
                    }
                }
            }
        }
    }

    public class Employeeinfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
    }
}
