using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.Service.Forms
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        string empno;
        string Dealer;
        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.QueryString["id"] != null)
            {
                empno = context.Request.QueryString["id"];
                Dealer = context.Request.QueryString["Dealer"];
            }
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "image/jpeg";
            Stream strm = ShowEmpImage(empno, Dealer);
            byte[] buffer = new byte[4096];
            int byteSeq = strm.Read(buffer, 0, 4096);

            while (byteSeq > 0)
            {
                context.Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, 4096);
            }
            //context.Response.BinaryWrite(buffer);
        }

        public Stream ShowEmpImage(string empno, string Dealer)
        {
            string conn = CConn.CConnection.GetConnectionString().ToString();
            SqlConnection connection = new SqlConnection(conn);
            string sql = "SELECT Image FROM FIRImageDetail WHERE FIRCode = @ID and DealerCode=@DealerCode";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", empno);
            cmd.Parameters.AddWithValue("@DealerCode", Dealer);
            connection.Open();
            object img = cmd.ExecuteScalar();
            try
            {
                return new MemoryStream((byte[])img);
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}