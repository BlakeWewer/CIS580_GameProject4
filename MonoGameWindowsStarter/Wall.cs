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
    class Wall : iCollidable
    {
        Game game;
        public BoundingRectangle Bounds;
        Texture2D texture;
        public bool isBombable;
        public bool destroyed;

        //Idea for next project: Invisible/Appearing Walls

        public Wall(Game game, Texture2D texture)
        {
            this.game = game;
            this.texture = texture;
        }

        public void Initialize(Vector2 position, bool bombable)
        {
            Bounds.Width = 50;
            Bounds.Height = 50;
            Bounds.X = position.X;
            Bounds.Y = position.Y;
            isBombable = bombable;
            destroyed = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!destroyed)
                spriteBatch.Draw(texture, Bounds, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 worldPosition)
        {
            if (!destroyed)
                spriteBatch.Draw(texture, new BoundingRectangle(new Vector2(Bounds.X, Bounds.Y) + worldPosition, Bounds.Width, Bounds.Height), Color.White);
        }

        public Vector2 Position()
        {
            return new Vector2(Bounds.X, Bounds.Y);
        }
    }
}
