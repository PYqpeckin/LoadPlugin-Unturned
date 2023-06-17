using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderPluginsForDedic
{
    public class Config : IRocketPluginConfiguration
    {
        public string _directory = @"C:\Plugins";
        public void LoadDefaults()
        {
        }
    }
}
