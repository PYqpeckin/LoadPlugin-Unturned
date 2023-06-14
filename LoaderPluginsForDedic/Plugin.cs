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
    public class Assem
    {
        public Assembly Assembly;
        public GameObject Object;

        public Assem(Assembly assembly, GameObject @object)
        {
            Assembly = assembly;
            Object = @object;
        }

        public Assem() { }
    }
    public class Plugin : RocketPlugin
    {
        public static Plugin Instance;
        public string _directory = @"C:\Plugins";

        public List<Assem> _assemblies = new List<Assem>();

        protected override void Load()
        {
            base.Load();
            Instance    = this;

            ConnectToRocket();
            
        }

        public void ConnectToRocket()
        {
            foreach (var assembly in System.IO.Directory.GetFiles(_directory))
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
    }

    class Command : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "lprd";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length < 2)
            {
                UnturnedChat.Say(caller, "lprd <load/unload> <name>");
                return;
            }

            if (command[0].ToLower() == "unload")
            {
                var Find = Plugin.Instance._assemblies.Find(x => x.Assembly.FullName.ToLower().Contains(command[1].ToLower()));
                UnityEngine.GameObject.Destroy(Find.Object);
                Console.WriteLine("Отгружен");
                Plugin.Instance._assemblies.Remove(Find);
            }
            else if (command[0].ToLower() == "load")
            {
                foreach (var assembly in System.IO.Directory.GetFiles(Plugin.Instance._directory))
                {
                    if (!assembly.ToLower().Contains(command[1].ToLower()))
                        continue;

                    GameObject gameObject = new GameObject(RocketHelper.GetTypesFromInterface(Assembly.LoadFile(assembly), "IRocketPlugin").FirstOrDefault().Name, RocketHelper.GetTypesFromInterface(Assembly.LoadFile(assembly), "IRocketPlugin").FirstOrDefault());

                    Plugin.Instance._assemblies.Add(new Assem(Assembly.LoadFile(assembly), gameObject));
                }

                List<Assembly> list = (List<Assembly>)typeof(RocketPluginManager).GetField("pluginAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(R.Plugins);
                foreach (var asses in Plugin.Instance._assemblies)
                {
                    list.Add(asses.Assembly);
                    Plugin.DontDestroyOnLoad(asses.Object);
                }
            }
            else
            {
                UnturnedChat.Say(caller, "lprd <load/unload> <name>");
                return;
            }
        }
    }
}
