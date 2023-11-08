using EvernoteClone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        private static string api_key = "AIzaSyDCViISbSPykopxo4RFARdVeLJLcApIS0M";

        public static async Task<bool> Register(User user)
        {
            //Init a new http client because this API use REST calls
            using (HttpClient client = new HttpClient())
            {
                //Create a new anonimous object with email, password and a bool called returnSecureToken (mandatory)
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };
                //Serialize the body object to json
                string bodyJson = JsonConvert.SerializeObject(body);
                //Define the string content using the json string, the utf8 encoding and defining the tipe of content
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                //Send the request via async post to a specific url and retrieve the result response
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={api_key}", data);
                //If request success
                if (response.IsSuccessStatusCode)
                {
                    //Read async the content of the response as a string
                    string resultJson = await response.Content.ReadAsStringAsync();
                    //Deserialize the result in json format using the class RegisterResult as "template" of the structure
                    var result = JsonConvert.DeserializeObject<RegisterResult>(resultJson);
                    //Save the local id in the result as user id
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    //Read async the content of the error response as a string
                    string errorJson = await response.Content.ReadAsStringAsync();
                    //Deserialize the result in json format using the class Error as "template" of the structure
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    //Display the error message using a message box
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }

        public static async Task<bool> Login(User user)
        {
            //Init a new http client because this API use REST calls
            using (HttpClient client = new HttpClient())
            {
                //Create a new anonimous object with email, password and a bool called returnSecureToken (mandatory)
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };
                //Serialize the body object to json
                string bodyJson = JsonConvert.SerializeObject(body);
                //Define the string content using the json string, the utf8 encoding and defining the tipe of content
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                //Send the request via async post to a specific url and retrieve the result response
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={api_key}", data);
                //If request success
                if (response.IsSuccessStatusCode)
                {
                    //Read async the content of the response as a string
                    string resultJson = await response.Content.ReadAsStringAsync();
                    //Deserialize the result in json format using the class RegisterResult as "template" of the structure
                    var result = JsonConvert.DeserializeObject<RegisterResult>(resultJson);
                    //Save the local id in the result as user id
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    //Read async the content of the error response as a string
                    string errorJson = await response.Content.ReadAsStringAsync();
                    //Deserialize the result in json format using the class Error as "template" of the structure
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    //Display the error message using a message box
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }

        public class RegisterResult
        {
            public string kind { get; set; }
            public string idToken { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
            public string localId { get; set; }
        }

        public class ErrorDetails
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Error
        {
            public ErrorDetails error { get; set; }
        }
    }
}
