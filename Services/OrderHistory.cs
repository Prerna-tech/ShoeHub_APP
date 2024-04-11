using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace E_commerce.Services
{
    public class OrderHistory
    {
        dbServices db = new dbServices();
        public async Task<responseData> history(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rcode"] = 0;
            MySqlParameter[] order = new MySqlParameter[]
            {new MySqlParameter("@CustomerId",req.addInfo["CustomerId"]),};
        
        try
        {
            var res = new ArrayList();
            var sq = $"SELECT * FROM pc_student.ShoeCart WHERE CustomerId=@CustomerId ";
            var run = db.ExecuteSQLName(sq, order);

            if (run == null )
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = "No Order History";
            }
            else
            {
                var list = new List<Dictionary<string,object>>();
                for(var i =0;i<run.Count();i++)
                {
                    foreach (var row in run[i])
                    {
                        Dictionary<string,object> orderDetails = new Dictionary<string, object>();
                        foreach (var field in row.Keys)
                        {
                            orderDetails[field] = row[field].ToString();
                        }
                        res.Add(orderDetails);
                    }
                }

                resData.rData["rData"] = res;
                resData.rData["rCode"]=0;
                resData.rData["rMessage"]= "Order History";
            }

            // resData.rData["CustomerId"] =run[0][0]["CustomerId"];
            // resData.rData["rMessage"] = "displayed data";
        }
        catch (Exception ex)
        {
            resData.rData["rCode"] = 1;
            resData.rData["rMessage"] = ex.Message;
        }

        return resData;
        }
    }
}
                                                                                                  