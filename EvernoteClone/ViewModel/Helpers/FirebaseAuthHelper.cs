using EvernoteClone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        private static string api_key = "AIzaSyDCViISbSPykopxo4RFARdVeLJLcApIS0M";

        public static async Task<bool> Register(User user)
        {
            //Init a new http client because this API use REST calls
            using(HttpClient client = new HttpClient())
            {
                //Create a new anonimous object with username, password and a bool called returnSecureToken (mandatory)
                var body = new
                {
                    email = user.Username,
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
                if(response.IsSuccessStatusCode)
                {
                    //Read async the content of the response as a string
                    string resultJson = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
