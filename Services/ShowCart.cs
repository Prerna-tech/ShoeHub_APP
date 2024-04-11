using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class ShowCart
    {
        dbServices db = new dbServices();


        public async Task<responseData> showcart (requestData req)

        {
            responseData resData = new responseData();
            resData.rData["rCode"] =0;

            try
            {
                MySqlParameter[] sql = new MySqlParameter[]
                {   new MySqlParameter("CustomerId",req.addInfo["CustomerId"]),
                };

                var res = new ArrayList();
                var q2 = @"SELECT * FROM pc_student.ShoeCart where CustomerId=@CustomerId";
                var query = db.ExecuteSQLName(q2,sql);

                if(query == null)
                {
                    resData.rData["rCode"] =1;
                    resData.rData["rMessage"] = "No Items in the Cart";
                }
                else
                {
                    var list = new List<Dictionary<string,object>>();
                    for(var i =0;i<query.Count();i++)
                    {
                        foreach (var row in query[i])
                        {
                            Dictionary<string,object> showcart = new Dictionary<string, object>();
                            foreach (var field in row.Keys)
                            {
                                showcart[field] = row[field].ToString();
                            }
                            res.Add(showcart);
                        }
                    }
                    resData.rData["rCode"]=0;
                    resData.rData["rData"] = res;
                    resData.rData["rMessage"]="Product Details";
                // resData.rData["PImage"] = query[0][0]["PImage"];
                // resData.rData["Product"] = query[0][0]["Product"];
                // resData.rData["Price"] = query[0][0]["Price"];
                // resData.rData["Total"] = query[0][0]["Total"];
                }
            }
            catch(Exception ex)
            {
                resData.rData["rcode"]=1;
                resData.rData["rMessage"] = ex.Message;
            }
            return resData;
        }
    }
}