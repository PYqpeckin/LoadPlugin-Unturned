using System;
using System.Collections.Generic;
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
}
