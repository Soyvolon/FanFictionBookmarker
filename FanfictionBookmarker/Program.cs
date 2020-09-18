using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace FanfictionBookmarker
{
    public class Program
    {
        public static string[] ValidUrls;
        public static string RegexUrls;

        public static void Main(string[] args)
        {
            ReadConigs();

            CalculateRegex();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void ReadConigs()
        {
            var root = "Configs";
            var file = "urls.json";

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            if (File.Exists(Path.Combine(root, file)))
            {
                using FileStream fs = new FileStream(Path.Combine(root, file), FileMode.Open);
                using StreamReader sr = new StreamReader(fs);
                var json = sr.ReadToEnd();

                ValidUrls = JsonConvert.DeserializeObject<string[]>(json);
            }
            else
            {
                var data = new string[] { "" };
                using var sw = new StreamWriter(new FileStream(Path.Combine(root, file), FileMode.Create));
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                foreach (string line in json.Split("\n"))
                    sw.WriteLine(line);
                sw.Close();

                Console.WriteLine($"New URL configuration generated. Please open and edit {Path.Combine(root, file)}.");
                Environment.Exit(0);
            }
        }

        public static void CalculateRegex()
        {
            var regex = "";
            foreach (var url in ValidUrls)
            {
                var data = url.Replace("www.", "");
                regex += $"(?:www.)?{data}|";
            }

            RegexUrls = regex[0..(regex.Length - 1)];
        }
    }
}
