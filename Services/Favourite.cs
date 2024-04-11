using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ResponseCaching;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class Favourite
    {
        dbServices db = new dbServices();

        public async Task<responseData> addTofavourite(requestData req)
        {
            responseData resData = new responseData();
            resData.rData ["rCode"] = 0;

            try
            {
                MySqlParameter[] sql = new MySqlParameter[]
                {   new MySqlParameter("@CustomerId",req.addInfo["CustomerId"].ToString()),
                };
                var result = new ArrayList();
                var fav = @"select * FROM pc_student.ShoeFavourite where CustomerId=@CustomerId";
                var add = db.ExecuteSQLName(fav,sql);

                if(add == null)
                {
                    resData.rData["rCode"]=1;
                    resData.rData["rMessage"]="No Items in Favourite";
                }
                else
                {

                    var list = new List<Dictionary<string, object>>();
                    for (var i=0;i<add.Count();i++)
                    {
                        foreach (var row in add[i])
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>();

                            foreach (var field in row.Keys)
                            {
                                dict[field]=row[field].ToString();

                            }
                            result.Add(dict);
                        }
                    }
                    resData.rData["rData"]=result;
                    resData.rData["rcode"] =0;
                    resData.rData["rMessage"] ="Favourite Items";
                    // resData.rData["PImage"]= add[0][0]["PImage"];
                    // resData.rData["Product"]= add[0][0]["Product"];
                    // resData.rData["Price"]= add[0][0]["Price"];
                }

            }
            catch(Exception ex)
            {

                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
            return resData;
        }


        public async Task<responseData> Liked(requestData req)
        {
            responseData resData = new responseData();
            resData.rData ["rCode"] = 0;

            try
            {
                // string inputID=req.addInfo["CustomerId"].ToString();

                MySqlParameter[] sql = new MySqlParameter[]
                {   
                    // new MySqlParameter("CustomerId",inputID),
                    // new MySqlParameter("status",2),

                    new MySqlParameter("@CustomerId",req.addInfo["CustomerId"].ToString()),
                    new MySqlParameter("@PImage",req.addInfo["PImage"]),
                    new MySqlParameter("@Product",req.addInfo["Product"].ToString()),
                    new MySqlParameter("@Price",req.addInfo["Price"].ToString()),
                
                };
                var fav = @"INSERT INTO pc_student.ShoeFavourite(CustomerId,PImage,Product,Price) VALUES(@CustomerId,@PImage,@Product,@Price)";
                var add = db.executeSQL(fav,sql);

                if(add[0].Count()!=0)
                {
                    resData.rData["rCode"]=1;
                    resData.rData["rMessage"]="Add items into Favourite ";
                }
                else
                {
                    resData.rData["rMessage"] =" iTEMS ADDED INTO FAVOURITE  SUCCESSFULLY ";
                    // resData.rData["PImage"]= add[0][0]["PImage"];
                    // resData.rData["Product"]= add[0][0]["Product"];
                    // resData.rData["Price "]= add[0][0]["Price"];
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