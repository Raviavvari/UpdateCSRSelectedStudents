using System;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace UpdateCSRSelectedStudents
{
    public partial class UpdateSelected : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private DataTable UpdateData(DataTable dt)
        {
            string scn = ConfigurationManager.ConnectionStrings["scn"].ConnectionString;
            DataTable dtnotfound = new DataTable();
            using (SqlConnection cn = new SqlConnection(scn))
            {                
                using (SqlCommand cmd = new SqlCommand("UpdateCSRSelectedStudent", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    SqlParameter ptable = cmd.Parameters.AddWithValue("@tblStudentMobile", dt);
                    ptable.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();                    
                    dtnotfound.Columns.Add("No_Student_Found");
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mobile = dr[0].ToString();
                        DataRow dtr = dtnotfound.NewRow();
                        dtr["No_Student_Found"] = mobile;
                        dtnotfound.Rows.Add(dtr);
                    }
                    cn.Close();                    
                }
            }
            if (dtnotfound.Rows.Count > 0)
            {
                return dtnotfound;
            }
            else
            {
                return null;
            }
        }

        protected void btn_upload_Click(object sender, EventArgs e)
        {            
            if (tbFile_Upload.FileName != null)
            {
                try
                {
                    string path = string.Concat(Server.MapPath(tbFile_Upload.FileName));
                    tbFile_Upload.SaveAs(path);
                    string excelCS = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source= {0}; Extended Properties = Excel 8.0", path);
                    using(OleDbConnection olecn = new OleDbConnection(excelCS))
                    {
                        OleDbCommand olecmd = new OleDbCommand("select * from [Sheet1$]", olecn);
                        olecmd.CommandType = CommandType.TableDirect;
                        olecn.Open();
                        OleDbDataReader dr = olecmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Contact_No");
                        while (dr.Read())
                        {
                            string mobile = dr[0].ToString();
                            StringBuilder sb = new StringBuilder();
                            sb.Append(mobile);
                            sb.Remove(0, 2);
                            sb.Remove(5,1);
                            sb.Remove(10, 1);
                            DataRow dtr = dt.NewRow();
                            dtr["Contact_No"] = mobile;
                            dt.Rows.Add(dtr);
                        }
                        DataTable dtnotfound = UpdateData(dt);
                        if(dtnotfound.Rows.Count > 0)
                        {
                            DataTable dtfordownlod = new DataTable();
                            dtfordownlod = (DataTable)Session["dtnodata"];//retrieves data from the session from Home.aspx.cs page
                            if (dt != null)
                            {
                                using (XLWorkbook wb = new XLWorkbook())//using xlworkbook class from ClosedXML.Excel namespace we can download the required data table
                                {
                                    wb.Worksheets.Add(dt, "Sheet1");
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment; filename=Not Registered Students.xlsx");
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        wb.SaveAs(ms);
                                        ms.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }            
        }
    }
}