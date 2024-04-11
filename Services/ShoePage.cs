using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class ShoePage
    {
        dbServices db =  new dbServices();

        public async Task<responseData> brandPage (requestData req)
        {

            responseData resData = new responseData();
            resData.rData["rcode"] =0;

            try
            {
                MySqlParameter[] show = new MySqlParameter[]{
                    new MySqlParameter("@CustomerId",req.addInfo["CustomerId"]),
                };

                var res = new ArrayList();
                var showpage = $"select * from pc_student.ShoePage where id<=6";
                var getdata = db .ExecuteSQLName(showpage,show);

                if (getdata == null || getdata[0].Count() == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Kindly Reload Page ";
                }
                else
                {
                    var list = new List<Dictionary<string,object>>();
                    for(var i =0;i<getdata.Count();i++)
                    {
                        foreach (var row in getdata[i])
                        {
                            Dictionary<string,object>brand = new Dictionary<string, object>();
                            foreach (var field in row.Keys)
                            {
                                brand[field] = row[field];
                            }
                            res.Add(brand);
                        }
                    }
                    resData.rData["rData"] = res;
                    // resData.rData["rCode"]=0;
                    // resData.rData["rMessage"]="Products Data";
                        
                }

            }
            catch(Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
            return resData;
        }
    }
}