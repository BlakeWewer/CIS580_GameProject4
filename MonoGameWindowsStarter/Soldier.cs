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
    class Soldier : Enemy
    {
        public Soldier(Game game) : base(game) { }

        public override void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("");
        }

        public override void Update()
        {
            // Need to have the soldiers move and attack player when close
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
