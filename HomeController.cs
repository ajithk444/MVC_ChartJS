using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LineChart()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult PieChart()
        {
            return PartialView("_PieChart");
        }

        [HttpPost]
        public JsonResult AjaxMethod() //google method
        {
            string query = @"SELECT R.Value as UserType, COUNT(U.ID) AS TotalUsers FROM [USER] U JOIN REFERENCE R ON R.ID = U.UserTypeID GROUP BY R.Value";
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                            {
                            "UserType", "TotalUsers"
                            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                            sdr["UserType"], sdr["TotalUsers"]
                            });
                        }
                    }

                    con.Close();
                }
            }

            return Json(chartData);
        }

        [HttpPost]
        public JsonResult BarChart()
        {
            DataSet users = new DataSet();
            string query = @"SELECT R.Value as UserType, COUNT(U.ID) AS TotalUsers FROM [USER] U JOIN REFERENCE R ON R.ID = U.UserTypeID GROUP BY R.Value";
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            List<object> iData = new List<object>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con)) //qlDataReader sdr = cmd.ExecuteReader())
                    {
                        adapter.Fill(users);
                    }
                    con.Close();
                }
            }
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in users.Tables[0].Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in users.Tables[0].Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult UserTrendLineChart()
        {
            DataSet users = new DataSet();
            string query = @"SELECT  CreateDate, COUNT(ID) AS UserCount, Email from [USER]  WHERE CreateDate < DATEADD(month, -12, GETDATE())  GROUP BY CreateDate , Email";
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            List<object> iData = new List<object>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con)) //qlDataReader sdr = cmd.ExecuteReader())
                    {
                        adapter.Fill(users);
                    }
                    con.Close();
                }
            }
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in users.Tables[0].Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in users.Tables[0].Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData, JsonRequestBehavior.AllowGet);


        }


    }
}