using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class FavRemove
    {
        dbServices db= new dbServices();

        public async Task<responseData> favremove(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"]=0;

                try
                {
                    MySqlParameter[] del = new MySqlParameter[]
                    {   
                        new MySqlParameter("FavId",req.addInfo["FavId"]),
                    };

                    var rem = @"delete from pc_student.ShoeFavourite where FavId=@FavId";
                    var res = db.executeSQL(rem,del);

                    if(res ==null)
                    {
                        resData.rData["rCode"] = 1;
                        resData.rData["rMessage"] = "Kindly add Items in  Favourite";
                    }
                    else
                    {
                        resData.rData["rMessage"] = " Favourite Items Remove Succesfully";
                    }

                }
                catch(Exception ex)
                {
                    resData.rData["rCode"] =1;
                    resData.rData["rMessage"] =ex.Message;
                }
                return resData;
            }
        }
}