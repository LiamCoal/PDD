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
using Stacker;
using Logging = PDD.ConsoleOutput.Logging;

namespace PDD.DataManagement
{
    public class Level
    {
        public struct SerializableLevel
        {
            public class Object : IStackable
            {
                public int X, Y, Id, Desti;
                
                public byte[] Stack()
                {
                    List<byte> list = new List<byte>();
                    list.AddRange(BitConverter.GetBytes(X));
                    list.AddRange(BitConverter.GetBytes(Y));
                    list.AddRange(BitConverter.GetBytes(Id));
                    list.AddRange(BitConverter.GetBytes(Desti));
                    return list.ToArray();
                }

                public IStackable Unstack(byte[] bytes)
                {
                    X = BitConverter.ToInt32(bytes, 0);
                    Y = BitConverter.ToInt32(bytes, sizeof(int));
                    Id = BitConverter.ToInt32(bytes, sizeof(int) * 2);
                    Desti = BitConverter.ToInt32(bytes, sizeof(int) * 3);
                    return this;
                }

                public override string ToString()
                {
                    return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Id)}: {Id}, {nameof(Desti)}: {Desti}";
                }

                public byte Size => sizeof(int) * 4;
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

        public void Save(string str = "output") => StackToBinary($"{str}_b.bin", $"{str}_e.bin");

        private void StackToBinary(string blockOutput, string entityOutput)
        {
            SingleTypeByteStack<SerializableLevel.Object> objectStack =
                new SingleTypeByteStack<SerializableLevel.Object>();
            var level = GetSerializableLevel();
            foreach (var block in level.Blocks)
            {
                objectStack.Add(block);
            }
            File.WriteAllBytes(blockOutput, objectStack.Stack());
            objectStack = new SingleTypeByteStack<SerializableLevel.Object>();
            foreach (var block in level.EntitySpawners)
            {
                objectStack.Add(block);
            }
            File.WriteAllBytes(entityOutput, objectStack.Stack());
        }

        public static Level Load(string name)
        {
            var level = new Level();
            var serializer = new XmlSerializer(typeof(SerializableLevel));
            var levelBlocks = SingleTypeByteStack<SerializableLevel.Object>.Unstack(File.ReadAllBytes($"{name}_b.bin"));
            var levelEntities = SingleTypeByteStack<SerializableLevel.Object>.Unstack(File.ReadAllBytes($"{name}_e.bin"));
            PortalData.Destinations.Clear();
            foreach (var levelObj in levelBlocks.Stackables)
            {
                Logging.Info(levelObj.ToString()!);
                level.Tiles[new LevelIndex(levelObj.X, levelObj.Y)] = levelObj.Id;
                if (levelObj.Id == Tile.Tiles.Portal)
                {
                    PortalData.Destinations[new LevelIndex(levelObj.X, levelObj.Y)] = levelObj.Desti;
                }
            }
            foreach (var (key, value) in PortalData.Destinations)
            {
                Logging.Info($"The portal at {key.X}, {key.Y} sends you to {value}");
            }

            foreach (var levelObj in levelEntities.Stackables)
            {
                Logging.Info(levelObj.ToString()!);
                level.Entities.Add(Entity.Entities.GetNew(levelObj.Id, new Vector2(levelObj.X, levelObj.Y)));
            }

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

        public static LevelIndex operator +(LevelIndex index, Vector2 vector2)
            => new LevelIndex((int) (index.X + vector2.X), (int) (index.Y + vector2.Y));

        public static LevelIndex operator -(LevelIndex index, Vector2 vector2)
            => index + -vector2;
    }
}