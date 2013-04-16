using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Squared.Tiled;
using System.IO;

namespace TiledExample {
    public class TiledExample : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Vector2 viewportPosition;

        public TiledExample () {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS || XBOX
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsFixedTimeStep = true;
#endif
        }

        protected override void LoadContent () {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = Map.Load(Path.Combine(Content.RootDirectory, "MapTest.tmx"), Content);
            map.ObjectGroups["events"].Objects["hero"].Texture = Content.Load<Texture2D>("hero");
        }

        protected override void UnloadContent () {
        }

        protected override void Update (GameTime gameTime) {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();
            float scrollx = 0, scrolly = 0;

            if (keyState.IsKeyDown(Keys.Left))
                scrollx = -1;
            if (keyState.IsKeyDown(Keys.Right))
                scrollx = 1;
            if (keyState.IsKeyDown(Keys.Up))
                scrolly = 1;
            if (keyState.IsKeyDown(Keys.Down))
                scrolly = -1;

            scrollx += gamePadState.ThumbSticks.Left.X;
            scrolly += gamePadState.ThumbSticks.Left.Y;

            if (gamePadState.IsButtonDown(Buttons.Back) || keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            float scrollSpeed = 8.0f;

            map.ObjectGroups["events"].Objects["hero"].X += (int)(scrollx * scrollSpeed);
            map.ObjectGroups["events"].Objects["hero"].Y -= (int)(scrolly * scrollSpeed);
            map.ObjectGroups["events"].Objects["hero"].Width = 100;
            map.Layers["Layer 1"].Opacity = (float)(Math.Cos(Math.PI * (gameTime.TotalGameTime.Milliseconds * 4) / 10000));

            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin();
            map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), viewportPosition);
            spriteBatch.End();
        }
    }
}
