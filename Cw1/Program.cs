using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    internal static class Program
    {
        private const string EmailPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        private static async Task Main(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var isUri = Uri.IsWellFormedUriString(args[0], UriKind.RelativeOrAbsolute);
            if (!isUri)
                throw new ArgumentException("url");

            try
            {
                var extractEmails = await GetEmails(args[0]);
                ShoEmails(extractEmails);
            }
            catch 
            {
                Console.WriteLine("Błąd wczasie pobierania strony");
            }
        }

        private static void ShoEmails(IList<string> extractEmails)
        {
            if (extractEmails == null || extractEmails.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
                return;
            }

            foreach (var extractEmail in extractEmails.Distinct())
            {
                Console.WriteLine(extractEmail);
            }
        }

        private static async Task<IList<string>> GetEmails(string url)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsByteArrayAsync();
            var data = Encoding.UTF8.GetString(content);
            var extractEmails = ExtractEmails(data);
            return extractEmails;
        }

        private static IList<string> ExtractEmails(string data)
        {
            var emailRegex = new Regex(EmailPattern, RegexOptions.IgnoreCase);

            return emailRegex
                .Matches(data)
                .Select(n => n.Value)
                .ToList();
        }
    }
}