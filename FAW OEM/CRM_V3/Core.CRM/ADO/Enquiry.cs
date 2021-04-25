using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Core.CRM.ADO
{
    public class Enquiry
    {
        private void GetChartData()
        {
            string cs = ConfigurationManager.ConnectionStrings["BMS0517ConnectionString"].ConnectionString;
            //now lets create sql connection object
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("Select Count(*) from CRM_EnquiryMaster",con);
                
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                    {
                }
            }
        }
    }
   
}
