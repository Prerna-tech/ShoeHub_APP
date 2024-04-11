using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Ocsp;

namespace E_commerce.Services
{
    public class Cart
    {
        dbServices db = new dbServices();

        public async Task<responseData>addToCart (requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"]=0;
            
            try
            {

                MySqlParameter[] data = new MySqlParameter[] {
                new MySqlParameter("@CustomerId",req.addInfo["CustomerId"].ToString()),
                new MySqlParameter("@PImage", req.addInfo["PImage"].ToString()),
                new MySqlParameter("@Product", req.addInfo["Product"].ToString()),
                new MySqlParameter("@Price", req.addInfo["Price"].ToString()),
                new MySqlParameter("@Total", req.addInfo["Total"].ToString()),
                new MySqlParameter("Time",req.addInfo["Time"].ToString()),
                new MySqlParameter("@Status",0)
                };


                var q1 = @"insert into pc_student.ShoeCart(CustomerId,PImage,Product,Price,Total,Status,Time) values(@CustomerId,@PImage,@Product,@Price,@Total,@Status,@Time);";
                var cart = db.executeSQL(q1,data);
                if(cart==null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"]="Items added in your cart ";
                }
                else
                {
                    resData.rData["rMessage"]="Items Added Succesfully";
                    // var show = @"select * from pc_student.ShoeCart where CustomerId=@CustomerId";
                    // var sql = db.executeSQL(show,data);


                    // // resData.rData["Product"] = cart[0][0]["Product"];
                    // // resData.rData["Quantity"] = cart[0][0]["Quantity"];
                    // // resData.rData["Price"] = cart[0][0]["Price"];
                }
                
            }
            catch (Exception ex){

                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
            return resData;

        }
        
    }
}