using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    class GameOver
    {
        Game game;
        Texture2D game_over;
        public BoundingCircle Bounds;

        public GameOver(Game game)
        {
            this.game = game;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            Bounds.Radius = 384;
            Bounds.X = graphics.GraphicsDevice.Viewport.Width / 2;
            Bounds.Y = graphics.GraphicsDevice.Viewport.Height / 2;
        }

        public void LoadContent()
        {
            game_over = game.Content.Load<Texture2D>("game_over");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(game_over, Bounds, Color.GhostWhite);
        }
    }
}
