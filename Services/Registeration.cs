using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class Registeration
    {
        dbServices ds = new dbServices();
        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public async Task<responseData> registration(requestData req)
        {
            responseData resData= new responseData();
            resData.rData["rCode"]=0;
            
            try
            { 
                string Email= req.addInfo["Email"].ToString();
                string Mobile = req.addInfo["Mobile"].ToString();

                bool validEmail=IsValidEmail(Email);
                bool validMobile = IsValidMobileNumber(Mobile);

                if(validEmail && validMobile)
                {
                    string checkemail = "Email";
                    string checkPhone_no ="Mobile";

                MySqlParameter[] para = new MySqlParameter[] {
                new MySqlParameter("@Username", req.addInfo["Username"].ToString()),
                new MySqlParameter("@Email", Email),
                new MySqlParameter("@Mobile",Mobile),
                new MySqlParameter("@Password", req.addInfo["Password"].ToString()),
                new MySqlParameter("Address" ,req.addInfo["Address"].ToString()),
                new MySqlParameter("Status", 1),

                }; 
            var sql=@"select * from pc_student.Shoehub_Customer where Mobile=@Mobile or Email=@Email";
            var check = ds.executeSQL(sql, para);
            if(check[0].Count()!=0)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"] = "Duplicate Credentials";
            }
            else 
            {
                var query = $"insert into pc_student.Shoehub_Customer(Username,Email,Mobile,Password,Address,Status)values(@Username,@Email,@Mobile,@Password,@Address,@Status);";
                var insertdata = ds.ExecuteInsertAndGetLastId(query, para);
                if(insertdata!=null)
                {

                    resData.rData["rCode"]=0;
                    resData.rData["rMessage"]="Registeration succesfully";
                } 
                else{
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"]="Invalid value";
                
                   }
            }
                }
            else
               {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Enter valid Email or Mobile number!";
               }

            }
            catch (Exception ex)
            {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
           return resData;
            
        }

        public static bool IsValidEmail(string Email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(Email, pattern);
        }
        public static bool IsValidMobileNumber(string Mobile)
        {
            string pattern = @"^[0-9]{7,15}$";
            return Regex.IsMatch(Mobile, pattern);
        }
    }
}
               