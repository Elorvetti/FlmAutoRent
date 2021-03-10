using System.Text.RegularExpressions;

namespace FlmAutoRent.Services
{
    public interface ICommonService 
    {
        string cleanStringPath(string name);
    }
    public class CommonService : ICommonService
    {
        public string cleanStringPath(string name){
            //Remove whitespace start end from string and replace white space white -
            string path = name.Trim().Replace(" ", "-").Replace("/", "-").Replace("_", "-");

            //remove accented letters
            path = path.Replace("à", "a").Replace("è", "e").Replace("é", "e").Replace("ò", "o").Replace("ç", "c").Replace("ù", "u");
            
            //remove spacial char
            path = Regex.Replace(path,"[^0-9a-zA-Z-]+", "");

            return path;

        }
    }
}