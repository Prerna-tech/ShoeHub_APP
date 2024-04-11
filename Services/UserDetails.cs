using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace E_commerce.Services
{
    public class UserDetails
    {
        dbServices dbs = new dbServices();

        private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _service_config = new Dictionary<string, string>();

        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<responseData> getUserDetails(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rcode"] =0;

            try{
                var details = new List<Dictionary<string,object>>();

                MySqlParameter[] show = new MySqlParameter[]{
                    new MySqlParameter("@CustomerId",req.addInfo["CustomerId"]),

                };

                var query = $"select * from pc_student.Shoehub_Customer where CustomerId=@CustomerId";
                var getdata = dbs .ExecuteSQLName(query,show);

                if (getdata == null || getdata[0].Count() == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Kindly Check Your  Payload...";
                }
                else
                {
                    resData.rData["rMessage"]="User Details..";
                    resData.rData["CustomerId"] = getdata[0][0]["CustomerId"];
                    resData.rData["Username"] = getdata[0][0]["Username"];
                    resData.rData["Email"] = getdata[0][0]["Email"];
                    resData.rData["Mobile"] = getdata[0][0]["Mobile"];
                }

            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = ex.Message;
            }
            return resData;
        }
                
    }
}