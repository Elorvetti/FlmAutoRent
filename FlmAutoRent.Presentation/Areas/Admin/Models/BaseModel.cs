using System;
using System.Text;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class BaseModel
    {
        private static string key = "cxz92k13md8f981hu6y7alkc";

        public string ConvertToEncrypt(string text){
            if(string.IsNullOrEmpty(text))
                return "";
            
            text += key;
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public string ConvertToDecrypt(string text){
            if(string.IsNullOrEmpty(text))
                return "";       

            var encoding = Convert.FromBase64String(text);
            var result = Encoding.UTF8.GetString(encoding);
            var indexStrat = result.Length - key.Length;
            result = result.Substring(0, indexStrat);

            return result;
        }
    }
}