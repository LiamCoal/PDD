using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Additive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PDD.Entity;
using Logging = PDD.ConsoleOutput.Logging;

namespace PDD.DataManagement
{
    public class Level
    {
        public struct SerializableLevel
        {
            public struct Object
            {
                public int X, Y, Id, Desti;
            }

            public Object[] Blocks, EntitySpawners;
        }

        internal readonly Dictionary<LevelIndex, int> Tiles;
        internal readonly List<IEntity> Entities;

        // ReSharper disable once MemberCanBePrivate.Global
        public Level(Dictionary<LevelIndex, int> tiles, List<IEntity> entities)
        {
            Tiles = tiles;
            Entities = entities;
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public Level(Dictionary<LevelIndex, int> tiles) : this(tiles, new List<IEntity>()) {}

        public Level() : this(new Dictionary<LevelIndex, int>()) {}

        public int GetTile(int x, int y)
        {
            try
            {
                var index = new LevelIndex(y, x);
                return Tiles[index];
            }
            catch (KeyNotFoundException)
            {
                return Tile.Tiles.Air;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public IEntity GetEntity(int index) => Entities[index];
        public void UpdateEntity(int index, IEntity @new) => Entities[index] = @new;

        // ReSharper disable once UnusedMethodReturnValue.Global
        public int InitializeEntity(int id, Vector2 vector2, ContentManager? content = null!)
        {
            IEntity e = Entity.Entities.GetNew(id, vector2);
            if(content != null) e.LoadContent(content);
            Entities.Add(e);
            return Entities.IndexOf(e);
        }

        public void LoadEntityContent(ContentManager contentManager)
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                var e = Entities[i]!;
                e.LoadContent(contentManager);
                Entities[i] = e;
            }
        }

        public void SetTile(int x, int y, int id)
            => Tiles[new LevelIndex(x, y)] = id;

        private SerializableLevel GetSerializableLevel()
        {
            var blockPoses = new List<SerializableLevel.Object>();
            foreach (var (key, value) in Tiles)
            {
                if(value == 0) continue;
                blockPoses.Add(new SerializableLevel.Object
                {
                    X = key.X,
                    Y = key.Y,
                    Id = value,
                    Desti = value == Tile.Tiles.Portal ? PortalData.Destinations[key] : 0
                });
            }

            return new SerializableLevel
            {
                Blocks = blockPoses.ToArray(),
                EntitySpawners = Entities.Select(entity => new SerializableLevel.Object
                    {X = (int) entity.Position.X, Y = (int) entity.Position.Y, Id = entity.Id}).ToArray()
            };
        }

        public void Save(string str = "output.xml") => File.WriteAllText(str, ToString(), Encoding.UTF8);

        public static Level Load(Stream stream)
        {
            var level = new Level();
            var serializer = new XmlSerializer(typeof(SerializableLevel));
            var level2 = (SerializableLevel) serializer.Deserialize(stream);
            PortalData.Destinations.Clear();
            foreach (var level2Obj in level2.Blocks)
            {
                level.Tiles[new LevelIndex(level2Obj.X, level2Obj.Y)] = level2Obj.Id;
                if (level2Obj.Id == Tile.Tiles.Portal)
                {
                    PortalData.Destinations[new LevelIndex(level2Obj.X, level2Obj.Y)] = level2Obj.Desti;
                }
            }
            foreach (var (key, value) in PortalData.Destinations)
            {
                Logging.Info($"The portal at {key.X}, {key.Y} sends you to {value}");
            }

            foreach (var level2Obj in level2.EntitySpawners)
                level.Entities.Add(Entity.Entities.GetNew(level2Obj.Id, new Vector2(level2Obj.X, level2Obj.Y)));
            return level;
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(SerializableLevel));
            var level = GetSerializableLevel();
            var stringBuilder = new StringBuilder();
            var stream = new StringWriter(stringBuilder);
            serializer.Serialize(stream, level);
            stream.Close();
            return stringBuilder.ToString().Replace("utf-16", "utf-8");
        }

        protected bool Equals(Level other)
        {
            return Tiles.Equals(other.Tiles) && Entities.Equals(other.Entities);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Level) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tiles, Entities);
        }
    }

    public readonly struct LevelIndex
    {
        public readonly int X;
        public readonly int Y;

        public LevelIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool Equals(LevelIndex other) => X == other.X && Y == other.Y;
        public override bool Equals(object? obj) => obj is LevelIndex other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        public LevelIndex Reverse() => new LevelIndex(Y, X);
    }
}