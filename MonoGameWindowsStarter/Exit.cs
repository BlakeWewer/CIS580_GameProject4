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
    class Exit : iCollidable
    {
        Game game;
        public BoundingRectangle Bounds;
        public enum Direction
        { NORTH, SOUTH, EAST, WEST};
        public Direction direction;

        public Exit(Game game)
        {
            this.game = game;
        }

        public void Initialize(BoundingRectangle bounds, Direction direction)
        {
            Bounds = bounds;
            this.direction = direction;
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public Vector2 Position()
        {
            return new Vector2(Bounds.X, Bounds.Y);
        }
    }
}
