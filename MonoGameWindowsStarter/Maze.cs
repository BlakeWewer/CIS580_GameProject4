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
    class Maze
    {
        Game game;
        public List<Wall> walls = new List<Wall>();
        public List<Spike> spikes = new List<Spike>();
        public List<Exit> exits = new List<Exit>();
        public List<iCollidable> collidables = new List<iCollidable>();
        public Cell[,] cells;
        public Vector2 startingPosition;
        public Vector2 endingPosition;
        public Tuple<int, int> worldPosition;

        public const int CELL_SIZE = 50;

        public int NUM_CELLS_WIDTH;
        public int NUM_CELLS_HEIGHT;

        public Maze(Game game)
        {
            this.game = game;
        }

        public void LoadContent(List<Tuple<int, int, int>> wallProperties, List<Tuple<int, int>> spikePositions, Vector2 startingPosition, Vector2 endingPosition, Tuple<int, int> worldPosition, Exit.Direction dir)
        {
            NUM_CELLS_WIDTH = (int)Math.Ceiling((double)game.GraphicsDevice.Viewport.Width / (double)CELL_SIZE);
            NUM_CELLS_HEIGHT = (int)Math.Ceiling((double)game.GraphicsDevice.Viewport.Height / (double)CELL_SIZE);
            cells = new Cell[NUM_CELLS_WIDTH, NUM_CELLS_HEIGHT];

            var cracked_wall_texture = game.Content.Load<Texture2D>("cracked_wall");
            var normal_wall_texture = game.Content.Load<Texture2D>("wall");
            var spike_texture = game.Content.Load<Texture2D>("spike");
            var exposedSFX = game.Content.Load<SoundEffect>("spikeSFX");

            //Change upper to load textures once & pass to initializers

            for (int i = 0; i < wallProperties.Count(); i++)
            {
                Texture2D wall_texture;
                bool isBombable = false;
                switch(wallProperties[i].Item3)
                {
                    case 0:
                        wall_texture = normal_wall_texture;
                        break;
                    case 1:
                        wall_texture = cracked_wall_texture;
                        isBombable = true;
                        break;
                    default:
                        wall_texture = normal_wall_texture;
                        break;
                }
                Wall wall = new Wall(game, wall_texture);
                wall.Initialize(new Vector2(wallProperties[i].Item1 * 50,
                                                wallProperties[i].Item2 * 50),
                                    isBombable);
                walls.Add(wall);
                collidables.Add(wall);
            }
            for (int i = 0; i < spikePositions.Count(); i++)
            {
                Spike spike = new Spike(game, spike_texture, exposedSFX);
                spike.Initialize(new Vector2(spikePositions[i].Item1 * 50,
                                                spikePositions[i].Item2 * 50));
                spikes.Add(spike);
                collidables.Add(spike);
            }
            this.startingPosition = startingPosition;
            this.endingPosition = endingPosition;

            for(int j = 0; j < NUM_CELLS_HEIGHT; j++)
            {
                for(int i = 0; i < NUM_CELLS_WIDTH; i++)
                {
                    cells[i, j] = new Cell(this, new BoundingRectangle(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
                }
            }
            Exit exit = new Exit(game);
            exit.Bounds.X = (int)(endingPosition.X + 49);
            exit.Bounds.Y = (int)endingPosition.Y;
            exit.Bounds.Height = 50;
            exit.Bounds.Width = 50;
            exit.direction = dir;
            exits.Add(exit);
            collidables.Add(exit);
            this.worldPosition = worldPosition;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Wall wall in walls)
            {
                wall.Draw(spriteBatch);
            }
            
            foreach(Spike spike in spikes)
            {
                spike.Draw(spriteBatch);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 worldPosition)
        {
            foreach (Wall wall in walls)
            {
                wall.Draw(spriteBatch, worldPosition);
            }

            foreach (Spike spike in spikes)
            {
                spike.Draw(spriteBatch, worldPosition);
            }
        }

        public void MakeWallBombable(Tuple<int, int> location)
        {
            bool found = false;
            int i = 0;
            while(!found && i < walls.Count())
            {
                if(walls[i].Bounds.X == location.Item1 * 50 
                    && walls[i].Bounds.Y == location.Item2 * 50)
                {
                    walls[i].isBombable = true;
                    found = true;
                }
                i++;
            }
        }

        public Cell[] getNearCells(iCollidable obj)
        {
            List<Cell> nearCells = new List<Cell>(); 
            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for(int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i,j].withinBounds(obj))
                    {
                        nearCells.Add(cells[i, j]);
                        if(i > 0)
                        {
                            nearCells.Add(cells[i - 1, j]);
                            if(j > 0)
                            {
                                nearCells.Add(cells[i, j - 1]);
                                nearCells.Add(cells[i - 1, j - 1]);
                            }
                            if(j < cells.GetLength(1) - 1)
                            {
                                nearCells.Add(cells[i, j + 1]);
                                nearCells.Add(cells[i - 1, j + 1]);
                            }
                        }
                        if(i < cells.GetLength(0) - 1)
                        {
                            nearCells.Add(cells[i + 1, j]);
                            if (j > 0)
                            {
                                nearCells.Add(cells[i + 1, j - 1]);
                            }
                            if (j < cells.GetLength(1) - 1)
                            {
                                nearCells.Add(cells[i + 1, j + 1]);
                            }
                        }
                    }
                }
            }
            return nearCells.ToArray();
        }
    }
}
