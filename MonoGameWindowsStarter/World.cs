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
    class World
    {
        Game game;
        Maze[][] world;

        public World(Game game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(Dictionary<int, Maze> levels)
        {
            world = new Maze[1][];
            world[0] = new Maze[3];
            foreach(Maze maze in levels.Values)
            {
                world[maze.worldPosition.Item1][maze.worldPosition.Item2] = maze;
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < world.Length; i ++)
            {
                for(int j = 0; j < world[i].Length; j++)
                {
                    world[i][j].Draw(spriteBatch, new Vector2(world[i][j].worldPosition.Item1 * 50, world[i][j].worldPosition.Item2 * 50));
                }
            }
        }
    }
}
