using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class Login
    {
        
     dbServices ds = new dbServices();
        
        private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _service_config = new Dictionary<string, string>();

        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public Login()
        {
            jwt_config["Key"] = appsettings["jwt_config:Key"].ToString();
            jwt_config["Issuer"] = appsettings["jwt_config:Issuer"].ToString();
            jwt_config["Audience"] = appsettings["jwt_config:Audience"].ToString();
            jwt_config["Subject"] = appsettings["jwt_config:Subject"].ToString();
            jwt_config["ExpiryDuration_app"] = appsettings["jwt_config:ExpiryDuration_app"].ToString();
            jwt_config["ExpiryDuration_web"] = appsettings["jwt_config:ExpiryDuration_web"].ToString();
        }
        public async Task<responseData> login(requestData req)
        {
            responseData resData= new responseData();
            resData.rData["rCode"]=0;
            try
            {
                string input = req.addInfo["UserId"].ToString();
                bool isEmail = IsValidEmail(input);
                bool isMobileNumber = IsValidMobileNumber(input);
                string columnName;
                if (isEmail)
                {
                    columnName = "Email";
                }
                else if (isMobileNumber)
                {
                    columnName = "Mobile";
                }
                else
                {
                    columnName = "";
                }

                MySqlParameter[] myParams = new MySqlParameter[] {
                new MySqlParameter("@UserId", input),
                new MySqlParameter("@Password", req.addInfo["Password"].ToString())
                };
                var sq = $"SELECT * FROM pc_student.Shoehub_Customer WHERE {columnName} = @UserId AND Password = @Password";
                var data = ds.ExecuteSQLName(sq, myParams);
                
                if (data==null || data[0].Count()==0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Invalid Credentials";
                }
                else

                {
                    var id = data[0][0]["CustomerId"];

                    var claims = new[]
               {
                    new Claim("uid",id.ToString()),
                    new Claim("guid", cf.CalculateSHA256Hash(req.addInfo["guid"].ToString())),
                };
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_config["Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                    var tokenDescriptor = new JwtSecurityToken(issuer: jwt_config["Issuer"], audience: jwt_config["Audience"], claims: claims,
                        expires: DateTime.Now.AddMinutes(Int32.Parse(jwt_config["ExpiryDuration_app"])), signingCredentials: credentials);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
                    resData.eventID = req.eventID;
                    resData.rData["rMessage"] = "Login Successfully";
                   resData.rData["CustomerId"] = data[0][0]["CustomerId"];
                    resData.rData["uid"]=id;
                   resData.rData["Username"] = data[0][0]["Username"];
                    resData.rData["Mobile"] = data[0][0]["Mobile"];
                    resData.rData["Email"] = data[0][0]["Email"];
                    resData.rData["Token"] = token;
                    
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