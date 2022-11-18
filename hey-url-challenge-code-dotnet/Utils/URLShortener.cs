using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;

namespace hey_url_challenge_code_dotnet.Utils
{
    public class RandomURL
    {
        private List<char> characters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public RandomURL()
        {

        }

        public string GetShortURL()
        {
            string URL = string.Empty;

            Random rand = new Random();
            int random;

            for(int i=0; i<5; i++)
            {
                random = rand.Next(0, characters.Count);
                URL += characters[random].ToString();
            }

            return URL;
        }
    }
}
