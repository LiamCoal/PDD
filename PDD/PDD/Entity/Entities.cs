using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Logging = PDD.ConsoleOutput.Logging;

namespace PDD.Entity
{
    public static class Entities
    {
        public const int
            Test = 0,
            PlayerOne = 1,
            Cat = 2;
        
        public static readonly Dictionary<int, Type> EntityList = new Dictionary<int, Type>();

        public static void InitializeAll()
        {
            EntityList[Test] = typeof(EntityTest);
            EntityList[PlayerOne] = typeof(PlayerOne);
            EntityList[Cat] = typeof(Cat);
            Logging.Info($"Initialized {EntityList.Count} entities.");
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public static T GetNew<T>(int id, Vector2 pos)
            where T : IEntity
        {
            if (!EntityList[id].IsAssignableFrom(typeof(T)) && typeof(T) != typeof(IEntity))
                throw new InvalidOperationException("Entity received is not required type.");
            var t = ((T) Activator.CreateInstance(EntityList[id])!)!;
            if(t is Player player)
                player.StartingPosition = pos;
            t.Position = pos;
            return t;
        }

        public static IEntity GetNew(int id, Vector2 pos) => GetNew<IEntity>(id, pos);

        internal static List<IEntity> GetAll(ContentManager contentManager)
        {
            List<IEntity> entities = new List<IEntity>();
            foreach (var e in EntityList.Select(key => GetNew(key.Key, Vector2.Zero)))
            {
                e.LoadContent(contentManager);
                entities.Add(e);
            }

            return entities;
        }
    }
}