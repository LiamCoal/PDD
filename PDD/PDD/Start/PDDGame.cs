using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Additive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PDD.Addons;
using PDD.DataManagement;
using PDD.Entity;
using PDD.Graphics;
using PDD.ResourceLoading;
using PDD.Tile;
using static PDD.ConsoleOutput.Logging;
using Color = Microsoft.Xna.Framework.Color;
using Logging = PDD.ConsoleOutput.Logging;

namespace PDD.Start
{
    public class PddGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch = null!;
        private ILoader _loader = null!;
        // ReSharper disable once NotAccessedField.Local
        private List<Addon> _addons = null!; // TODO
        private SpriteFont _liberationMonoFont = null!;
        internal static Level Level = new Level(), FutureLevel = new Level();
        internal string? LevelLoad = null;
        private Vector2 _startDrag, _endDrag;
        private bool _dragFrame, _drawDragFrame;

        internal static bool HideTilePlacer
            => CurrentIndicatorTtl > 0 || OperationType != LevelOperationType.None;
        //public const float ScreenScale = 2.0f; // TODO
        internal static string CurrentIndicator = "";
        internal static int CurrentIndicatorTtl;
        internal static IndicatorType CurrentIndicatorType = IndicatorType.Zero;
        internal static StringBuilder CurrentFileOp = new StringBuilder();
        internal static LevelOperationType OperationType;

        // ReSharper disable once ConvertToConstant.Global
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool RequireRefresh = false;
        public static Mode Mode = Mode.Default;
        private MouseState _prevMouseState;
        private int _currentBlock, _currentPlaceMode;
        public static Song BackgroundMusic = null!;

        // ReSharper disable once MemberCanBePrivate.Global
        public const string Version = "0.0.5::09082020/1";

        public PddGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = new TimeSpan(166666);
            IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            Info($"Starting PDD {Version}");
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            
            _loader = ILoader.NewDefault;
            _addons = new List<Addon>();
            
            Tiles.InitializeAll();
            Entities.InitializeAll();
            
            Info("Loading core addons...");
            _loader.LoadAddonsFromDirectory("Addons/Core");
            
            // This is used when Addons change the default loader.
            if (RequireRefresh)
            {
                _loader = ILoader.NewDefault;
                Info($"Refreshed loader (now {ILoader.DefaultType})");
            }

            Info("Loading main addons...");
            _loader.LoadAddonsFromDirectory("Addons");

            if (Mode != Mode.LevelEditor)
                Level = LevelFileManager.Load(0);
            else
                if(LevelLoad != null)
                    Level = Level.Load(LevelLoad);
                else
                    Level = new Level();

            if (Mode == Mode.LevelEditor)
            {
                CurrentIndicator = "Started Level Editor";
                CurrentIndicatorTtl = 60;
                CurrentIndicatorType = IndicatorType.Info;
            }

                // _testEntity = _level.InitializeEntity(Entities.Test, new Vector2(0, 0));
            
            // _level.UnknownBlockAccessed += (x, y) 
            //     => _level.SetTile(x, y, y < 15 ? Tiles.NormalGround : Tiles.Air);

            if(Mode == Mode.LevelEditor)
                Window.TextInput += (sender, args) =>
                {
                    Info($"{args.Key} {OperationType}");
                    if (OperationType == LevelOperationType.None) return;
                    var key = args.Key;
                    switch (key)
                    {
                        case Keys.Enter:
                            try
                            {
                                switch (OperationType)
                                {
                                    case LevelOperationType.Save:
                                        if (CurrentFileOp.ToString().StartsWith('='))
                                        {
                                            var num = Convert.ToInt32(CurrentFileOp.ToString().Substring(1));
                                            LevelFileManager.Save(num, Level);
                                        }
                                        else
                                        {
                                            var str = CurrentFileOp.ToString();
                                            Level.Save(str);
                                        }

                                        CurrentIndicator = "Successfully saved to file.";
                                        CurrentIndicatorTtl = 120;
                                        CurrentIndicatorType = IndicatorType.Info;
                                        break;
                                    case LevelOperationType.Load:
                                        if (CurrentFileOp.ToString().StartsWith('='))
                                        {
                                            var num = Convert.ToInt32(CurrentFileOp.ToString().Substring(1));
                                            Level = LevelFileManager.Load(num);
                                        }
                                        else
                                        {
                                            var str = CurrentFileOp.ToString();
                                            Level = Level.Load(str);
                                        }
                                    
                                        CurrentIndicator = "Successfully loaded from file.";
                                        CurrentIndicatorTtl = 120;
                                        CurrentIndicatorType = IndicatorType.Info;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                Level.LoadEntityContent(Content);
                            }
                            catch (FileNotFoundException e)
                            {
                                Error(e.ToString());
                                CurrentIndicator = $"Could not find file: {CurrentFileOp}";
                                CurrentIndicatorTtl = 300;
                                CurrentIndicatorType = IndicatorType.Error;
                            }

                            OperationType = LevelOperationType.None;

                            break;
                        case Keys.Back:
                            if (CurrentFileOp.Length - 1 < 0) OperationType = LevelOperationType.None;
                            else CurrentFileOp.Length--;
                            break;
                        default:
                            CurrentFileOp.Append(args.Character);
                            break;
                    }
                };
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // pfft, loading content normally is for losers
            Info("Magically loading content...");
            MagicResourceInitializing.InitializeAll(Content);
            
            Info("Loading entity content...");
            Level.LoadEntityContent(Content);

            _liberationMonoFont = Content.Load<SpriteFont>("Font/LiberationMono");
            _entities = Entities.GetAll(Content);

            // if (Mode == Mode.Default)
            // {
            //     GraphicsManager.BackgroundParallaxLayer.Add(new GraphicsManager.DrawableObject
            //     {
            //         Position = new Vector2(0,0),
            //         Texture = _background,
            //         Scale = (float)ScreenSize.Height / _background.Height
            //     });
            // }

            BackgroundMusic = Content.Load<Song>("Sound/Music/BGMusic0");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(BackgroundMusic);

            Info("Loading content from addons...");
            Events.OnLoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().HorizontalScrollWheelValue > _prevMouseState.HorizontalScrollWheelValue)
                _currentPlaceMode++;
            if (Mouse.GetState().HorizontalScrollWheelValue < _prevMouseState.HorizontalScrollWheelValue)
                _currentPlaceMode--;

            const int placeModeCount = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.O) &&
                !_prevKeyboardState.IsKeyDown(Keys.O))
            {
                OperationType = LevelOperationType.Load;
                CurrentFileOp = new StringBuilder();
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Q) &&
                !_prevKeyboardState.IsKeyDown(Keys.Q))
            {
                OperationType = LevelOperationType.Save;
                CurrentFileOp = new StringBuilder();
            }
            
            if (_currentPlaceMode >= placeModeCount) _currentPlaceMode = 0;
            if (_currentPlaceMode < 0) _currentPlaceMode = placeModeCount - 1;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_currentPlaceMode)
            {
                case 0:
                    // Place tiles.
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                        Mode == Mode.LevelEditor)
                        Level.SetTile(Mouse.GetState().Y / 16, Mouse.GetState().X / 16, _currentBlock);
                    if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                        _prevMouseState.RightButton == ButtonState.Released &&
                        Mode == Mode.LevelEditor)
                    {
                        _startDrag = new Vector2(Mouse.GetState().X / 16.0f, Mouse.GetState().Y / 16.0f);
                        _drawDragFrame = true;
                    } else if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                               Mode == Mode.LevelEditor)
                    {
                        _endDrag = new Vector2(Mouse.GetState().X / 16.0f, Mouse.GetState().Y / 16.0f);
                    }

                    break;
                
                case 1:
                    // Place entities.
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                        _prevMouseState.LeftButton == ButtonState.Released &&
                        Mode == Mode.LevelEditor)
                    {
                        Level.InitializeEntity(_currentBlock, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Content);
                    }
                    break;
                
                case 2:
                    // Place portals.
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                        Mode == Mode.LevelEditor)
                    {
                        Level.SetTile(Mouse.GetState().Y / 16, Mouse.GetState().X / 16, Tiles.Portal);
                        PortalData.Destinations[new LevelIndex(Mouse.GetState().Y / 16, Mouse.GetState().X / 16)]
                            = _currentBlock;
                    }

                    break;
            }

            _dragFrame = Mouse.GetState().RightButton == ButtonState.Released &&
                         _prevMouseState.RightButton == ButtonState.Pressed &&
                         Mode == Mode.LevelEditor;

            if (_dragFrame)
            {
                for (var i = (int) _startDrag.X; i < _endDrag.X; i++)
                {
                    for (var j = (int) _startDrag.Y; j < _endDrag.Y; j++)
                    {
                        Level.SetTile(j, i, _currentBlock);
                    }
                }

                _drawDragFrame = false;
            }

            if (Mouse.GetState().ScrollWheelValue > _prevMouseState.ScrollWheelValue)
                _currentBlock++;
            if (Mouse.GetState().ScrollWheelValue < _prevMouseState.ScrollWheelValue)
                _currentBlock--;
            
            // if(Keyboard.GetState().IsKeyDown(Keys.Q) && Mode == Mode.LevelEditor)
            //     Level.Save();
            if (Keyboard.GetState().IsKeyDown(Keys.C) && Mode == Mode.LevelEditor)
            {
                Level.Tiles.Clear();
                Level.Entities.Clear();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && Mode == Mode.LevelEditor)
            {
                Level.Entities.Clear();
            }
            
            switch (_currentPlaceMode)
            {
                case 0:
                    if (_currentBlock < 0) _currentBlock = Tiles.TileList.Count - 1;
                    if (_currentBlock >= Tiles.TileList.Count - 1) _currentBlock = 0;
                    break;
                
                case 1:
                    if (_currentBlock < 0) _currentBlock = Entities.EntityList.Count - 1;
                    if (_currentBlock >= Entities.EntityList.Count) _currentBlock = 0;
                    break;
                
                case 2:
                    break;
            }


            Events.OnUpdate();
            if(Mode != Mode.LevelEditor)
                for (var index = 0; index < Level.Entities.Count; index++)
                {
                    var entity = Level.Entities[index];
                    var levelhash = FutureLevel.GetHashCode();
                    if (entity is MovingEntity movingEntity)
                    {
                        movingEntity.SetNextPosition(Level, Content);
                        // Info($"{_level._entities.IndexOf(entity)} checked tile: {movingEntity.LastTileCheckX} {movingEntity.LastTileCheckY}");
                        entity = movingEntity;
                    }

                    if (levelhash != FutureLevel.GetHashCode())
                    {
                        Level.Tiles.Clear();
                        foreach (var (key, value) in FutureLevel.Tiles)
                        {
                            Level.Tiles[key] = value;
                        }
                        
                        Level.Entities.Clear();    
                        for (var i = 0; i < FutureLevel.Entities.Count; i++)
                        {
                            var value = FutureLevel.Entities[i];
                            Level.Entities.Add(value);
                            Info($"{Level.Entities[i]} = {value}");
                        }

                        break;
                    }
                    // Info($"Updating entity {index} (id={entity.Id})");
                    entity.Update(gameTime, Level);
                    Level.UpdateEntity(index, entity);
                }

            _prevMouseState = Mouse.GetState();
            _prevKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        private List<IEntity>? _entities;
        private KeyboardState _prevKeyboardState;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(16, 16, 24));

            _spriteBatch.Begin();
            
            GraphicsManager.MainLayer.Clear();
            //Info(ScreenSize.ToString());
            const int lowerTileX = 0;
            var upperTileX = (int) Math.Ceiling((double)_graphics.PreferredBackBufferWidth / 16);
            const int lowerTileY = 0;
            var upperTileY = (int) Math.Ceiling((double)_graphics.PreferredBackBufferWidth / 16);

            for (var i = 0; i < Level.Tiles.Count; i++)
            {
                var (key, value) = Level.Tiles.ToArray()[i];
                if(key.X < lowerTileX || key.X > upperTileX || key.Y < lowerTileY || key.Y > upperTileY)
                    continue;
                if(value == Tiles.Air)
                    continue;
                //Info($"{j} {i} {tileId}");
                var tile = Tiles.TileList[value];
                GraphicsManager.MainLayer.Add(new GraphicsManager.DrawableObject
                {
                    Position = new Vector2(key.Y * 16, key.X * 16),
                    Texture = tile.GetTexture2D(key.X, key.Y, Level)
                });
            }

            if (Mode == Mode.Default)
            {
                foreach (var entity in Level.Entities)
                {
                    GraphicsManager.MainLayer.Add(new GraphicsManager.DrawableObject
                    {
                        Position = entity.Position,
                        Texture = entity.GetTexture2D()
                    });
                }
            }
            GraphicsManager.Draw(ref _spriteBatch);
            Events.OnDraw();
            #if DEBUG
            _spriteBatch.DrawString(_liberationMonoFont, $"{1000 / gameTime.ElapsedGameTime.Milliseconds} FPS", new Vector2(3, 3), Color.White);
            _spriteBatch.DrawString(_liberationMonoFont, $"PDD {Version}", new Vector2(3, 19), Color.White);
            #endif
            var color = Color.Transparent;
            color.R = 32;
            color.G = 32;
            color.B = 32;

            color.A = 31;
            if (Mode == Mode.LevelEditor && !HideTilePlacer)
            {
                if(_currentPlaceMode != 1) 
                    _spriteBatch.Draw(_currentPlaceMode switch
                    {
                        0 => Tiles.TileList[_currentBlock].GetTexture2D(-16, -32, Level),
                        2 => Tiles.TileList[Tiles.Portal].GetTexture2D(-16, -32, Level),
                        _ => throw new InvalidOperationException("Invalid place mode.")
                    }, new Vector2(4, 702), Color.White);
                _spriteBatch.DrawString(_liberationMonoFont, _currentBlock.ToString(), new Vector2(22, 704), Color.White);
                var wid = _currentPlaceMode switch
                {
                    0 => _drawDragFrame ? ((int) Math.Abs(_startDrag.X - _endDrag.X) + 1) * 16 : 16,
                    1 => _entities![_currentBlock].GetTexture2D().Width,
                    2 => 16,
                    _ => 0
                };
                var hei = _currentPlaceMode switch
                {
                    0 => _drawDragFrame ? ((int) Math.Abs(_startDrag.Y - _endDrag.Y) + 1) * 16 : 16,
                    1 => _entities![_currentBlock].GetTexture2D().Height,
                    2 => 16,
                    _ => 0
                };
                if (wid > 0 && hei > 0)
                {
                    Texture2D rect = new Texture2D(_graphics.GraphicsDevice, wid, hei);

                    Color[] data = new Color[wid*hei];
                    for(var i=0; i < data.Length; ++i) data[i] = Color.White;
                    rect.SetData(data);

                    var coor = _currentPlaceMode switch
                    {
                        0 => !_drawDragFrame
                            ? new Vector2(Mouse.GetState().X / 16 * 16,
                                Mouse.GetState().Y / 16 * 16)
                            : new Vector2((int) _startDrag.X * 16, (int) _startDrag.Y * 16),
                        1 => new Vector2(Mouse.GetState().X,
                            Mouse.GetState().Y),
                        2 => new Vector2(Mouse.GetState().X / 16 * 16,
                            Mouse.GetState().Y / 16 * 16),
                        _ => Vector2.Zero
                    };
                    _spriteBatch.Draw(rect, coor, color);
                }
            }

            #if DEBUG
            foreach (var entity in Level.Entities)
            {
                int wid, hei;
                if (Mode == Mode.LevelEditor)
                {
                    wid = entity.GetTexture2D().Width;
                    hei = entity.GetTexture2D().Height;
                }
                else
                {
                    wid = 16;
                    hei = 16;
                }
                var tex = new Texture2D(GraphicsDevice, wid, hei);
                Color[] colors = new Color[wid * hei];
                for (var i = 0; i < wid * hei; i++)
                {
                    colors[i] = Color.White;
                }

                tex.SetData(colors);
                
                var vector16 = new Vector2(16, 16);

                if (Mode == Mode.LevelEditor || entity is MovingEntity)
                {
                    if (Mode != Mode.LevelEditor && entity is MovingEntity movingEntity)
                    {
                        if (movingEntity.TileCheckArray != null)
                        {
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.Lower.Vector * vector16, color);
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.Upper.Vector * vector16, color);
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.LowerLeft.Vector * vector16, color);
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.LowerRight.Vector * vector16, color);
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.UpperLeft.Vector * vector16, color);
                            _spriteBatch.Draw(tex, movingEntity.TileCheckArray.UpperRight.Vector * vector16, color);
                        }
                    }
                    else
                    {
                        _spriteBatch.Draw(tex, entity.Position, color);
                    }
                }
                var num = Mode == Mode.LevelEditor ? entity.Id : Level.Entities.IndexOf(entity);
                _spriteBatch.DrawString(_liberationMonoFont, $"{num}", entity.Position - new Vector2(0, 16), Color.Yellow);
            }
            #endif

            if (CurrentIndicatorTtl > 0)
            {
                static Color Warn()
                {
                    Warning($"Invalid Color Type {CurrentIndicatorType}");
                    return Color.Transparent;
                }
                
                _spriteBatch.DrawString(_liberationMonoFont, CurrentIndicator, new Vector2(3, 701),
                    CurrentIndicatorType switch
                {
                    IndicatorType.Zero => Color.Transparent,
                    IndicatorType.Info => Color.Green,
                    IndicatorType.Warning => Color.Yellow,
                    IndicatorType.Error => Color.Red,
                    IndicatorType.Status => Color.LightBlue,
                    _ => Warn()
                });
                CurrentIndicatorTtl--;
            }

            if (OperationType != LevelOperationType.None)
            {
                var str = $"{OperationType} file > {CurrentFileOp}";
                if (CurrentFileOp.ToString().StartsWith('='))
                {
                    str = $"{OperationType} level > Levels/level:{CurrentFileOp.ToString().Substring(1)}.xml";
                }
                _spriteBatch.DrawString(_liberationMonoFont, str, new Vector2(3, 702), Color.Yellow);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Useful for testing if somethings the wrong way around
        // ReSharper disable once UnusedMember.Local
        private static Vector2 Reverse(Vector2 vector2)
        {
            var (x, y) = vector2;
            return new Vector2(y, x);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Close();
            
            base.OnExiting(sender, args);
        }
    }

    public enum Mode
    {
        Default,
        LevelEditor
    }
}