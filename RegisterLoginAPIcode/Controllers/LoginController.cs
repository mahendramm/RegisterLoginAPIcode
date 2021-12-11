using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using RegisterLoginAPIcode.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RegisterLoginAPIcode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _iconfiguration;

        public LoginController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }

        
        [HttpPost("ValidateOTP_Login")]
        public JsonResult ValidateOTP_Registration(validOTP_login validotp)
        {
            string mymqldatasourc = _iconfiguration.GetConnectionString("cons");
            MySqlDataReader myreader;
            int statuscount = 0;
            using (MySqlConnection con = new MySqlConnection(mymqldatasourc))

            {
                con.Open();
                MySqlCommand mycmd = new MySqlCommand();
                mycmd.CommandText = "validateOTP_Login";
                mycmd.Connection = con;
                mycmd.CommandType = CommandType.StoredProcedure;

                mycmd.Parameters.AddWithValue("@mobileno", validotp.mobile);
                mycmd.Parameters.AddWithValue("@otpno", validotp.otp);
                
                mycmd.Parameters.Add("@statuscount", MySqlDbType.Int32);
                mycmd.Parameters["@statuscount"].Direction = ParameterDirection.Output;

                mycmd.ExecuteNonQuery();
                statuscount = Convert.ToInt32(mycmd.Parameters["@statuscount"].Value);
                con.Close();

            }

            JsonResult retunstring = null;
            if (statuscount != 1)
            {
                var jsonObj = new
                {
                    success = false,
                    message = "user not valid"
                };
                retunstring = new JsonResult(jsonObj);
            }
            if (statuscount == 1)
            {
                var jsonObj2 = new
                {
                    success = true,
                    message = "Login Success"
                };
                // retunstring = jsonObj;
                //   return Request.(HttpStatusCode.OK, jsonObj);
                retunstring = new JsonResult(jsonObj2);
            }
            return retunstring;
        }



        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        [HttpPost("Login_sendOTP")]
        public JsonResult Login_sendOTP(sendOTP sendotp)
        {
            string Result = "";
            JsonResult retunstring = null;
            int otp = GenerateRandomNo();
            try
            {


                string mymqldatasourc = _iconfiguration.GetConnectionString("cons");
                MySqlDataReader myreader;
                int statuscount = 0;
                string otpmessage = "";
                using (MySqlConnection con = new MySqlConnection(mymqldatasourc))

                {
                    con.Open();
                    MySqlCommand mycmd = new MySqlCommand();
                    mycmd.CommandText = "sendOTP_login";
                    mycmd.Connection = con;
                    mycmd.CommandType = CommandType.StoredProcedure;

                    mycmd.Parameters.AddWithValue("@mobileno", sendotp.mobile);
                    mycmd.Parameters.AddWithValue("@otpno", otp);
                    mycmd.Parameters.AddWithValue("@rstatus", "0");

                    mycmd.Parameters.Add("@statuscount", MySqlDbType.Int32);
                    mycmd.Parameters["@statuscount"].Direction = ParameterDirection.Output;
                  
                    mycmd.ExecuteNonQuery();
                    statuscount = Convert.ToInt32(mycmd.Parameters["@statuscount"].Value);
                   
                    con.Close();

                }
                // Check mobile number entry present or not 0 as no entry there in dB 
                if (statuscount == 1)
                {
                    TwilioClient.Init("AC23c9c53d8a1783cbf63d48a98032412a", "56fb8cd7835cda4af5a6206ae821e42c");
                    var message1 = MessageResource.Create(
                         body: "Your OTP :" + otp,
                         from: new Twilio.Types.PhoneNumber("+12514281487"),
                         to: new Twilio.Types.PhoneNumber("+91" + sendotp.mobile)
                     ); 
                    var jsonObj2 = new
                    {
                        success = true,
                         message = message1.ErrorMessage,
                        otpmessage = "OTP has been sent to your mobile number "

                    };
                    retunstring = new JsonResult(jsonObj2);
                }
                if (statuscount != 1)
                {
                    var jsonObj2 = new
                    {
                        success = false,
                        message = "User not registerd via API"

                    };
                    retunstring = new JsonResult(jsonObj2);
                }
            }
            catch (SystemException ex)
            {

                var jsonObj2 = new
                {
                    success = false,
                    message = ex.Message.ToString()
                };
                retunstring = new JsonResult(jsonObj2);
            }
            return retunstring;


        }

        private string sendsms2(string mobileno, string message)
        {
            string Result = "";
            try
            {
                TwilioClient.Init("AC23c9c53d8a1783cbf63d48a98032412a", "56fb8cd7835cda4af5a6206ae821e42c");

                var message1 = MessageResource.Create(
                    body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
                    from: new Twilio.Types.PhoneNumber("+12514281487"),
                    to: new Twilio.Types.PhoneNumber("+918698472485")
                );

            }
            catch (SystemException ex)
            {
                Result = ex.Message.ToString();
            }
            return Result;


        }




    }
}
