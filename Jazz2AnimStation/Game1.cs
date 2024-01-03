using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Jazz2AnimStation
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

       // const double MS_PER_UPDATE = 1.00 / 10.00 * 1000;

        Color _clearColor = new Color((byte)72, (byte)48, (byte)168);
  
        Texture2D _textureBG;
        Texture2D _textureRectangle;

        private int _displayWidth => _graphics.GraphicsDevice.Viewport.Width;
        private int _displayHeight => _graphics.GraphicsDevice.Viewport.Height;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            //TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / 1024));
           // TargetElapsedTime = TimeSpan.FromMilliseconds(1.00 / 120.00 * 1000.00);
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.HardwareModeSwitch = true;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
         
            //TouchPanel.EnableMouseTouchPoint = true;
            //TouchPanel.EnableMouseGestures = true;
            //_graphics.ToggleFullScreen();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
             base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
           
            GetHashCode();
        }

        double previous;
        double lag = 0.0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(Keyboard.GetState().IsKeyDown(Keys.RightAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
                _graphics.ToggleFullScreen();
            if (Keyboard.GetState().IsKeyDown(Keys.RightAlt) && Keyboard.GetState().IsKeyDown(Keys.V))
            {
                _graphics.SynchronizeWithVerticalRetrace = !Keyboard.GetState().IsKeyDown(Keys.RightControl);
                _graphics.ApplyChanges();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.RightAlt) && Keyboard.GetState().IsKeyDown(Keys.H))
            {
                _graphics.HardwareModeSwitch = !_graphics.HardwareModeSwitch;
                _graphics.ApplyChanges();
            }
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(_clearColor);
            base.Draw(gameTime);
        }


       
    }
}