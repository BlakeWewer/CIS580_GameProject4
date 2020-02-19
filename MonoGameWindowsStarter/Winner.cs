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
    class Winner
    {
        Game game;
        Texture2D winner;
        public BoundingCircle Bounds;

        public Winner(Game game)
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
            winner = game.Content.Load<Texture2D>("winner");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(winner, Bounds, Color.GhostWhite);
        }
    }
}
