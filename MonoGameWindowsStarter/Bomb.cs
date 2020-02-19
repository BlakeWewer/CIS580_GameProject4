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
    class Bomb : iCollidable
    {
        Game game;
        TimeSpan activated_timer;
        TimeSpan detonated_timer;
        TimeSpan timer;
        Texture2D bomb;
        Texture2D explosion;
        public BoundingCircle Bounds;
        public BoundingCircle Explosion;
        public SoundEffect fuseSFX;
        public SoundEffect explosionSFX;
        public bool placed;
        public bool active;
        public bool toDetonate;
        public bool detonated;

        public Bomb(Game game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            placed = false;
            active = false;
            toDetonate = false;
            detonated = false;
        }

        public void LoadContent()
        {
            bomb = game.Content.Load<Texture2D>("bomb");
            explosion = game.Content.Load<Texture2D>("explosion");
            fuseSFX = game.Content.Load<SoundEffect>("fuseSFX");
            explosionSFX = game.Content.Load<SoundEffect>("explosionSFX");
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if(placed)
            {
                fuseSFX.Play();
                toDetonate = true;
                placed = false;
            }
            if(timer.TotalMilliseconds - activated_timer.TotalMilliseconds > fuseSFX.Duration.TotalMilliseconds && toDetonate)
            {
                explosionSFX.Play();
                active = false;
                detonated = true;
                toDetonate = false;
                detonated_timer = new TimeSpan(0);
            }
            if (timer.TotalMilliseconds - detonated_timer.TotalMilliseconds > explosionSFX.Duration.TotalMilliseconds * 10 && detonated)
            {
                detonated = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(active)
            {
                spriteBatch.Draw(bomb, Bounds, Color.White);
            }
            if(detonated)
            {
                spriteBatch.Draw(explosion, Explosion, Color.White);
            }
            
        }

        public Vector2 Position()
        {
            return new Vector2(Bounds.X, Bounds.Y);
        }

        public void Place(Vector2 position)
        {
            Bounds.Radius = 12;
            Bounds.X = position.X + Bounds.Radius;
            Bounds.Y = position.Y + Bounds.Radius;
            Explosion.Radius = 50;
            Explosion.X = Bounds.X;
            Explosion.Y = Bounds.Y;
            activated_timer = new TimeSpan(0);
            timer = new TimeSpan(0);
            placed = true;
            active = true;
            toDetonate = false;
            detonated = false;
        }
    }
}
