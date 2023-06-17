using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Core;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoaderPluginsForDedic.Command
{
    class lprd : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "lprd";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {


            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, "lprd <load/unload/list> <*name>");
                return;
            }

            if (command[0].ToLower() == "unload" && command.Length >= 2)
            {
                var Find = Plugin.Instance._assemblies.Find(x => x.Assembly.FullName.ToLower().Contains(command[1].ToLower()));
                UnityEngine.GameObject.Destroy(Find.Object);
                Console.WriteLine("Отгружен");
                Plugin.Instance._assemblies.Remove(Find);
            }
            else if (command[0].ToLower() == "load" && command.Length >= 2)
            {
                foreach (var assembly in System.IO.Directory.GetFiles(Plugin.Instance.Configuration.Instance._directory))
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
            else if (command[0].ToLower() == "list" && command.Length >= 1)
            {
                string ls = "";
                foreach (var List in Plugin.Instance._assemblies)
                    ls += $"{List.Assembly.GetName().Name}, ";

                UnturnedChat.Say(caller, ls);
            }
            else
            {
                UnturnedChat.Say(caller, "lprd <load/unload/list> <*name>");
                return;
            }
        }
    }
}
