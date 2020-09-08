using System;
using Microsoft.Xna.Framework.Content;
// ReSharper disable EventNeverSubscribedTo.Global
#pragma warning disable 67

namespace PDD.Addons
{
    public static class Events
    {
        public static event EventHandler<ContentManager>? LoadContent;
        public static event EventHandler? Update;

        public static event EventHandler? Draw;

        public static void OnLoadContent(ContentManager e)
        {
            LoadContent?.Invoke(null, e);
        }

        public static void OnUpdate()
        {
            Update?.Invoke(null, EventArgs.Empty);
        }

        public static void OnDraw()
        {
            Draw?.Invoke(null, EventArgs.Empty);
        }
    }
}