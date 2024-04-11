using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class RemoveItem
    {
        dbServices db= new dbServices();

        public async Task<responseData> remove(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"]=0;

            try
            {
                 MySqlParameter[] del = new MySqlParameter[]
                {   
                    new MySqlParameter("CartID",req.addInfo["CartID"]),
                };

                var rem = @"delete from pc_student.ShoeCart where CartID=@CartID";
                var res = db.executeSQL(rem,del);

                if(res ==null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Kindly add Items in your cart";
                }
                else
                {
                    resData.rData["rMessage"] = " Items Remove Succesfully";
                }

            }
            catch(Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"] = ex.Message;
            }

            return resData;
        }

        public async Task<responseData> removeAll(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"]=0;

            try
            {
                 MySqlParameter[] del = new MySqlParameter[]
                {   
                    new MySqlParameter("CustomerId",req.addInfo["CustomerId"]),
                    new MySqlParameter("Status",2)
                };

                var rem = @"update pc_student.ShoeCart set Status=@Status where CustomerId = @CustomerId";
                var res = db.executeSQL(rem,del);

                if(res ==null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "No Items in your cart";
                }
                else
                {
                    resData.rData["rMessage"] = " All Items Remove Succesfully";
                }

            }
            catch(Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"] = ex.Message;
            }

            return resData;
        }
    }
}