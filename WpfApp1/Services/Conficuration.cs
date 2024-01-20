using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public static class Conficuration
    {
        public static string getConficration(string Key,string Value)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("D:\\Desktop\\WpfApp1\\WpfApp1\\appsettings.json");
            return builder.Build().GetSection(Key)[Value];
           
        }
    }
}
