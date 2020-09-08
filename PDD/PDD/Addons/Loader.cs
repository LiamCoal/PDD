using System;
using System.Collections.Generic;
using Additive;

namespace PDD.Addons
{
    /// <summary>
    /// Loads Addons.
    ///
    /// </summary>
    /// <remarks>It is important that this is not recursive.</remarks>
    public interface ILoader
    {
        // ReSharper disable once UnusedMethodReturnValue.Global
        public IEnumerable<Addon> LoadAddonsFromDirectory(string directory);

        public static readonly Type DefaultType = typeof(DefaultLoader);
        public static ILoader NewDefault 
            => Activator.CreateInstance(DefaultType) as ILoader ?? new DefaultLoader();
    }
}