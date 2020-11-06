using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    static class Program
    {
        private const string EmailPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(args[0]);
            var content = await response.Content.ReadAsByteArrayAsync();
            var data = Encoding.UTF8.GetString(content);
            var extractEmails = ExtractEmails(data);
            foreach (var extractEmail in extractEmails)
            {
                Console.WriteLine(extractEmail);
            }
        }

        private static IEnumerable<string> ExtractEmails(string data)
        {
            var emailRegex = new Regex(EmailPattern, RegexOptions.IgnoreCase);

            return emailRegex
                .Matches(data)
                .Select(n => n.Value)
                .ToList();
        }
    }
}
