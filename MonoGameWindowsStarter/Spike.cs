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
    class Spike : iUpdateable, iCollidable
    {
        Game game;
        Texture2D texture;
        public BoundingCircle Bounds;
        TimeSpan timer;
        int timer_randomizer;
        public bool on;
        Random random = new Random();
        public bool destroyed;
        public SoundEffect exposedSFX;

        public Spike(Game game, Texture2D texture, SoundEffect soundFX)
        {
            this.game = game;
            timer = new TimeSpan(0);
            on = false;
            this.texture = texture;
            exposedSFX = soundFX;
        }

        public void Initialize(Vector2 position)
        {
            Bounds.Radius = 25;
            Bounds.X = position.X + Bounds.Radius;
            Bounds.Y = position.Y + Bounds.Radius;
            timer_randomizer = random.Next(2000);
            destroyed = false;
        }

        public void Update(GameTime gameTime)
        {
            bool prev_on = on;
            timer += gameTime.ElapsedGameTime;
            if ((timer.TotalMilliseconds + timer_randomizer) % 3000 > 1999 && !destroyed)
            {
                on = true;
                if (!prev_on)
                    exposedSFX.Play(.025F, 0, 0);
            }  
            else
                on = false;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           if(on & !destroyed)
           {
                spriteBatch.Draw(texture, Bounds, Color.White);
           }  
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 worldPosition)
        {
            if (on & !destroyed)
            {
                spriteBatch.Draw(texture, new BoundingRectangle(new Vector2(Bounds.X, Bounds.Y) + worldPosition, 2*Bounds.Radius, 2*Bounds.Radius), Color.White);
            }
        }
        


        public Vector2 Position()
        {
            return new Vector2(Bounds.X, Bounds.Y);
        }
    }
}
