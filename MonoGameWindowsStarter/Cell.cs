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
    class Cell : iUpdateable
    {
        Maze maze;
        public BoundingRectangle Bounds;
        public List<iCollidable> collidables;

        public Cell(Maze maze, BoundingRectangle bounds)
        {
            this.maze = maze;
            Bounds = bounds;
        }

        public void Update(GameTime gameTime)
        {
            collidables = new List<iCollidable>();
            foreach(iCollidable x in maze.collidables)
            {
                if (withinBounds(x))
                {
                    collidables.Add(x);
                }
            }
        }

        public bool withinBounds(iCollidable obj)
        {
            Vector2 objPosition = obj.Position();
            return (objPosition.X >= Bounds.X && objPosition.X <= Bounds.X + Bounds.Width
                && objPosition.Y >= Bounds.Y && objPosition.Y <= Bounds.Y + Bounds.Height);
        }
    }
}
