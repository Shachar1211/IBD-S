using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text;
using finalproj.BL;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;


namespace finalproj.BL
{
    public class ChatGPTResponse
    {    
        public string Answer { get; set; }
        public int ResponseID { get; set; }
        public UserParams UserParams { get; set; }

        private readonly IConfiguration _configuration; // גישה לapi 

        public ChatGPTResponse(IConfiguration configuration, UserParams userParams)
        {
            _configuration = configuration;
            UserParams = userParams;
        }

        public ChatGPTResponse() { }
        public ChatGPTResponse(string answer, UserParams userParams)
        {
            Answer = answer;
            UserParams = userParams;
        }

        public bool Insert() // הכנסה של התשובה ופרטים נוספים לדאטה בייס
        {
            DBservicesChatGPTResponse dbs = new DBservicesChatGPTResponse();
            dbs.Insert(this);
            return true;
        }

        public List<ChatGPTResponse> Read() // קריאה של כל התשובות מהמחשבון זכויות
        {
            DBservicesChatGPTResponse dbs = new DBservicesChatGPTResponse();
            return dbs.Read();
        }

        public async Task<string> CreateGPT()
        {
            string question = this.UserParams.ParamsToQuestion(); // יצירת השאלה בעזרת פרמטרי המשתמש
            string pathPrompt = Path.Combine(Directory.GetCurrentDirectory(), "uploadFilesGPT", "prompt.txt"); // קבלת נתיב של קובץ הפרומפט
            string fileInString = "";
            try //  שמירת התוכן מקובץ הפרומפט
            {
                if (System.IO.File.Exists(pathPrompt))
                {
                    fileInString = System.IO.File.ReadAllText(pathPrompt);
                }
                else
                {
                   return("File not found: " + pathPrompt);
                }
            }

            catch (Exception e)
            {

                return(e.Message);
            }
            string pathRights = Path.Combine(Directory.GetCurrentDirectory(), "uploadFilesGPT", "rightsFile.txt"); //קבלת נתיב של קובץ הזכויות
            try // שמירת התוכן מקובץ הזכויות
            {
                if (System.IO.File.Exists(pathRights))
                {
                    fileInString += System.IO.File.ReadAllText(pathRights);
                }
                else
                {
                    return("File not found: " + pathRights);
                }
            }
            catch (Exception e)
            {
             
               return(e.Message);
            }


            string escapedString = Regex.Replace(fileInString, @"[\n\r\t""\\]", m => m.Value switch
            {
                "\n" => "\\n",
                "\r" => "\\r",
                "\t" => "\\t",
                "\"" => "\\\"", 
                "\\" => "\\\\", 
                _ => m.Value
            }); // התאמת המסמך כדי שהצ'אט יוכל לקרוא אותו
            var apiKey = _configuration["AppSettings:OpenAIApiKey"];// api key from appsetting.json
            var client = new HttpClient(); // יצירת אובייקט ששולח בקשות http
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); //הגדרת כותרת לאימות הבקשה
            var content = new StringContent("{\"model\": \"gpt-4o\"," +
                " \"messages\":" +
                " [{\"role\": \"system\", " +
                "\"content\": \"" + escapedString + "\"}, " +
                "{\"role\": \"user\", \"content\": \"" + question + "\"}]}",
                Encoding.UTF8, "application/json"); // הגדרת תוכן הבקשה 
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);//  שליחת בקשת פוסט עם התוכן לכתובת המבוקשת 
            if (!response.IsSuccessStatusCode) // בדיקה אם התקבלה שגיאה
            {
                throw new ApplicationException($"Failed to get response from OpenAI: {response.StatusCode}");
            }

            string result = await response.Content.ReadAsStringAsync(); // קריאה של גוף התגובה
            JObject responseObject = JObject.Parse(result); // המרה לגוף התגובה כדי לקבל את  התוכן הסופי
            string contentFinal = responseObject["choices"][0]["message"]["content"].ToString(); // קבלת התוכן הרלוונטי הסופי
            var chatGPTResponse = new ChatGPTResponse(contentFinal, this.UserParams);
            chatGPTResponse.Insert(); // שמירת התשובה והפרמטרים של המשתמש בדאטה בייס
            return contentFinal;

        }

    }

}
