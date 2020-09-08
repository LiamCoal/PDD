using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Additive;

namespace PDD.Addons
{
    /*
     * TODO Make Loaders a thing for Additive
     */
    public class DefaultLoader : ILoader
    {
        private static IEnumerable<Addon> LoadAddons(string file)
        {
            var asm = Assembly.LoadFile(file);
            var list = new List<Addon>();

            foreach (var type in asm.GetTypes())
            {
                if(!type.IsSubclassOf(typeof(Addon))) continue;
                if(!(Activator.CreateInstance(type) is Addon addon))
                    throw new InvalidProgramException("Unreachable code.");
                addon.Load();
                
                list.Add(addon);
            }

            return list;
        }

        public IEnumerable<Addon> LoadAddonsFromDirectory(string directory)
        {
            var list = new List<Addon>();
            foreach (var filename in Directory.EnumerateFiles(directory))
            {
                list.AddRange(LoadAddons(filename));
            }

            return list;
        }
    }
}