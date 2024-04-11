using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class Order
    {
        dbServices db = new dbServices();

        public async Task<responseData> ordering(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rcode"] = 0;

            try{
                string inputID=req.addInfo["CustomerId"].ToString();

                MySqlParameter[] sql = new MySqlParameter[]
                {   
                    new MySqlParameter("CustomerId",inputID),
                    new MySqlParameter("@Status",1),
                    new MySqlParameter("@Time",req.addInfo["Time"].ToString()),
                };

                var change = $"UPDATE pc_student.ShoeCart SET Status=@Status, Time=@Time WHERE CustomerId =@CustomerId";
                var res = db.executeSQL(change,sql);
                if(res==null)
                {
                    resData.rData["rcode"] = 1;
                    resData.rData["rMessage"]="Kindly Placed your Order";

                }
                else
                {
                    resData.rData["rcode"] = 0;
                    resData.rData["rMessage"]="Order Placed succesfully";

                }
                
            }
            catch(Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMesage"] = ex.Message;
            }
            return resData;
        }
    }
}