using Rocket.API;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoaderPluginsForDedic
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;

        public List<Assem> _assemblies = new List<Assem>();

        protected override void Load()
        {
            base.Load();
            Instance    = this;

            ConnectToRocket();
            Console.WriteLine("Plugins create for qpeckin");
        }

        public void ConnectToRocket()
        {
            foreach (var assembly in System.IO.Directory.GetFiles(Configuration.Instance._directory))
            {
                GameObject gameObject = new GameObject(RocketHelper.GetTypesFromInterface(Assembly.LoadFile(assembly), "IRocketPlugin").FirstOrDefault().Name, RocketHelper.GetTypesFromInterface(Assembly.LoadFile(assembly), "IRocketPlugin").FirstOrDefault());

                _assemblies.Add(new Assem(Assembly.LoadFile(assembly), gameObject));
            }

            List<Assembly> list = (List<Assembly>)typeof(RocketPluginManager).GetField("pluginAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(R.Plugins);
            foreach(var asses in _assemblies)
            {
                list.Add(asses.Assembly);
                DontDestroyOnLoad(asses.Object);
            }
        }

        protected override void Unload()
        {
            Instance = null;
            base.Unload();

        }
    }

   
    
}
