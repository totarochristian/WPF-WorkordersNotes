using EvernoteClone.Model;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
        private static string dbPath = "https://notes-app-wpf-bc58c-default-rtdb.europe-west1.firebasedatabase.app/";

        /// <summary>
        /// Method that will insert a generic type of object in a generic table of the database.
        /// </summary>
        /// <typeparam name="T">Type of the object to be inserted in the database</typeparam>
        /// <param name="item">Object to be inserted in the database</param>
        /// <returns></returns>
        public static async Task<bool> Insert<T>(T item)
        {
            //Serialize the item passed as parameter making the body of the REST call that will be done to save data in the firebase database
            var body = JsonConvert.SerializeObject(item);
            //Define the content using the serialized object, the utf-8 encoding and the string "application/json" as media type
            var content  = new StringContent(body, Encoding.UTF8, "application/json");
            //Initialize the http client
            using(var client = new HttpClient())
            {
                //Start the post request to the path of the database composed with the name of the item type
                var result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);
                //If the result is success
                if(result.IsSuccessStatusCode)
                {
                    return true;
                }else { 
                    return false;
                }
            }
        }

        /// <summary>
        /// Method that will update a generic type of object in a generic table of the database.
        /// </summary>
        /// <typeparam name="T">Type of the object to be updated in the database</typeparam>
        /// <param name="item">Object to be updated in the database</param>
        /// <returns></returns>
        public static async Task<bool> Update<T>(T item) where T : HasId
        {
            //Serialize the item passed as parameter making the body of the REST call that will be done to save data in the firebase database
            var body = JsonConvert.SerializeObject(item);
            //Define the content using the serialized object, the utf-8 encoding and the string "application/json" as media type
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            //Initialize the http client
            using (var client = new HttpClient())
            {
                //Start the patch request to the path of the database composed with the name of the item type, and use the id of the item to patch data
                var result = await client.PatchAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);
                //If the result is success
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Method that will delete a generic type of object in a generic table of the database.
        /// </summary>
        /// <typeparam name="T">Type of the object to be deleted in the database</typeparam>
        /// <param name="item">Object to be deleted in the database</param>
        /// <returns></returns>
        public static async Task<bool> Delete<T>(T item) where T : HasId
        {
            //Initialize the http client
            using (var client = new HttpClient())
            {
                //Start the patch request to the path of the database composed with the name of the item type, and use the id of the item to patch data
                var result = await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");
                //If the result is success
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Method that will read a list of objects of a generic type from a generic table of the database.
        /// </summary>
        /// <typeparam name="T">Type of the objects to be readed in the database</typeparam>
        /// <returns>List with a generic type of object</returns>
        public static async Task<List<T>> Read<T>() where T : HasId
        {
            //initialize the http client
            using (var client = new HttpClient())
            {
                //Start the request to get data to the path of the database composed with the name of the T type
                var result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                //Read the result as json string async
                var jsonResult = await result.Content.ReadAsStringAsync();
                //If the result is success
                if (result.IsSuccessStatusCode)
                {
                    //Deserialize the json result in a dictionary with string key and T value
                    var objects = JsonConvert.DeserializeObject<Dictionary<string,T>>(jsonResult);
                    //If there are objects readed from the database
                    if(objects != null)
                    {
                        //Initialize an empty list
                        List<T> list = new List<T>();
                        //Foreach element in the dictionary, add the value in the list defined
                        foreach (var o in objects)
                        {
                            //Set the id of the value equals to the key in the dictionary object.
                            //To do that, create an interface called HasId, extend Notebook and Note to this class and in this
                            //method tell that each T implement HasId.
                            o.Value.Id = o.Key;
                            list.Add(o.Value);
                        }
                        return list;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
