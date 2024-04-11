using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class EditProfile
    {
         dbServices ds = new dbServices();

        private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _service_config = new Dictionary<string, string>();
         public async Task<responseData> UpdateProfile(requestData req)
        {
            responseData resData= new responseData();
            resData.rData["rCode"]=0;
            
            try
            {
                MySqlParameter[] myParams = new MySqlParameter[] {
                new MySqlParameter("@CustomerId",req.addInfo["CustomerId"]),
                new MySqlParameter("@Username", req.addInfo["Username"].ToString()),
                new MySqlParameter("@Email", req.addInfo["Email"].ToString()),
                new MySqlParameter("@Mobile", req.addInfo["Mobile"].ToString()),

                }; 


                var sq = $"update pc_student.Shoehub_Customer set Username=@Username,Email=@Email,Mobile=@Mobile where CustomerId=@CustomerId";
                var data = ds.executeSQL(sq, myParams);
                
                if (data==null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Invalid Credentials";
                }
                else
                {
                    resData.eventID = req.eventID;
                    resData.rData["rMessage"] = "User Details Update Successfully";

                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
            return resData;
        }

        public async Task<responseData> updatePass(requestData req)
        {
            responseData resData= new responseData();
            resData.rData["rCode"]=0;

            try
            {
                MySqlParameter[] para = new MySqlParameter[] {
                new MySqlParameter("@CustomerId", req.addInfo["CustomerId"]),
                new MySqlParameter("@Mobile",req.addInfo["Mobile"].ToString()),
                new MySqlParameter("@Password",req.addInfo["Password"].ToString()),
                new MySqlParameter("@New_Password",req.addInfo["New_Password"].ToString())

                };
                var sql=$"select * from pc_student.Shoehub_Customer where Mobile=@Mobile and Password=@Password";
                var check = ds.executeSQL(sql, para);

                if(check !=null  && check[0].Count()>0)
                {
                    var query1=$"update pc_student.Shoehub_Customer set Password=@New_Password where CustomerId=@CustomerId " ;
                    var update = ds.executeSQL(query1, para);


                    if(update!=null)
                    {
                        resData.eventID = req.eventID;
                        resData.rData["rMessage"] = "Password Updated Successfully";
                    }
                    else
                    {

                        resData.rData["rCode"] = 1;
                        resData.rData["rMessage"] = "Invalid Credentials";

                    }
                }
                else
                {
                    resData.rData["rMessage"]="Kindly Check Your Old Password and MobileNo";
                    resData.rData["rcode"] = 1;
                }
                
            }
            catch (Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
            return resData;
        }
        
    }
}