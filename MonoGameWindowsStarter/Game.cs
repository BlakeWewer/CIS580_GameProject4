using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MonoGameWindowsStarter.Player;
using static MonoGameWindowsStarter.Exit;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont scoreFont;
        Player player;
        int score;
        int sum_score_prev_levels;
        int game_over_score;
        int winner_score;
        int current_level = 0;
        Maze current_maze;
        Maze prev_maze;
        Dictionary<int, Maze> levels = new Dictionary<int, Maze>();
        Maze level1;
        Maze level2;
        Maze level3;
        bool game_over = false;
        GameOver gameOver;
        bool win = false;
        Winner winner;
        KeyboardState keyboardState;
        KeyboardState oldkeyboardState;
        List<iUpdateable> updateable;
        World world;
        enum ViewState { TRANSITION_RIGHT, IDLE};
        ViewState viewState = ViewState.IDLE;
        float translationX = 0;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            level1 = new Maze(this);
            level2 = new Maze(this);
            level3 = new Maze(this);
            levels = new Dictionary<int, Maze>();
            gameOver = new GameOver(this);
            winner = new Winner(this);
            world = new World(this);
            score = 10000;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();            

            gameOver.Initialize(graphics);
            winner.Initialize(graphics);
            keyboardState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent();
            scoreFont = Content.Load<SpriteFont>("defaultFont");

            List<Tuple<int, int, int>> level1WallPositions = new List<Tuple<int, int, int>>();
            List<Tuple<int, int>> level1SpikePositions = new List<Tuple<int, int>>();
            //StreamReader wallReader1 = new StreamReader("../../../../../Level1WallPlacements.txt");
            string rawWall1 = Properties.Resources.Level1WallPlacements;
            string rawSpike1 = Properties.Resources.Level1SpikePlacements;
            string[] values;
            string[] line = rawWall1.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level1WallPositions.Add(new Tuple<int, int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1]),
                                                            Convert.ToInt32(values[2])));
            }
            line = rawSpike1.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level1SpikePositions.Add(new Tuple<int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1])));
            }
            levels.Add(0, level1);

            List<Tuple<int, int, int>> level2WallPositions = new List<Tuple<int, int, int>>();
            List<Tuple<int, int>> level2SpikePositions = new List<Tuple<int, int>>();
            string rawWall2 = Properties.Resources.Level2WallPlacements;
            string rawSpike2 = Properties.Resources.Level2SpikePlacements;
            line = rawWall2.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level2WallPositions.Add(new Tuple<int, int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1]),
                                                            Convert.ToInt32(values[2])));
            }
            line = rawSpike2.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level2SpikePositions.Add(new Tuple<int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1])));
            }
            levels.Add(1, level2);

            List<Tuple<int, int, int>> level3WallPositions = new List<Tuple<int, int, int>>();
            List<Tuple<int, int>> level3SpikePositions = new List<Tuple<int, int>>();
            string rawWall3 = Properties.Resources.Level3WallPlacements;
            string rawSpike3 = Properties.Resources.Level3SpikePlacements;
            line = rawWall3.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level3WallPositions.Add(new Tuple<int, int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1]),
                                                            Convert.ToInt32(values[2])));
            }
            line = rawSpike3.Split('\n');
            foreach (string pair in line)
            {
                values = pair.Split(' ');
                level3SpikePositions.Add(new Tuple<int, int>(Convert.ToInt32(values[0]),
                                                            Convert.ToInt32(values[1])));
            }
            levels.Add(2, level3);
            level1.LoadContent(level1WallPositions, level1SpikePositions, new Vector2(1, 351), new Vector2(1001, 351), new Tuple<int, int>(0, 0), Direction.EAST);
            level2.LoadContent(level2WallPositions, level2SpikePositions, new Vector2(1, 351), new Vector2(1001, 651), new Tuple<int, int>(0, 1), Direction.EAST);
            level3.LoadContent(level3WallPositions, level3SpikePositions, new Vector2(1, 651), new Vector2(1001, 201), new Tuple<int, int>(0, 2), Direction.EAST);
            world.LoadContent(levels);
            gameOver.LoadContent();
            winner.LoadContent();
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            updateable = new List<iUpdateable>();

            if(viewState == ViewState.IDLE)
            {
                oldkeyboardState = keyboardState;
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.P) && !oldkeyboardState.IsKeyDown(Keys.P))
                {
                    NextLevel();
                }
                if (keyboardState.IsKeyDown(Keys.Q) && !oldkeyboardState.IsKeyDown(Keys.Q))
                {
                    PreviousLevel();
                }
                if (keyboardState.IsKeyDown(Keys.E) && !oldkeyboardState.IsKeyDown(Keys.E))
                {
                    player.curPosition = current_maze.endingPosition;
                }
                if (keyboardState.IsKeyDown(Keys.S) && !oldkeyboardState.IsKeyDown(Keys.S))
                {
                    player.curPosition = current_maze.startingPosition;
                }

                if (!win && !game_over)
                {
                    if (player.Bounds.X > graphics.GraphicsDevice.Viewport.Width)
                    {
                        NextLevel();
                        return;
                    }

                    current_maze = levels[current_level];

                    // TODO: Add your update logic here
                    updateable.Add(player);
                    foreach (Spike spike in current_maze.spikes)
                    {
                        updateable.Add(spike);
                    }
                    foreach (Cell cell in current_maze.cells)
                    {
                        updateable.Add(cell);
                    }
                    foreach (iUpdateable obj in updateable)
                    {
                        obj.Update(gameTime);
                    }

                    foreach (Cell cell in current_maze.getNearCells(player))
                    {
                        foreach (iCollidable collidable in cell.collidables)
                        {
                            if (collidable is Wall wall)
                            {
                                if (player.Bounds.CollidesWith(wall.Bounds) && !wall.destroyed)
                                {
                                    float delta;
                                    switch (player.moving_state)
                                    {
                                        case MovingState.East:
                                            delta = (player.Bounds.X + player.Bounds.Width) - wall.Bounds.X;
                                            player.curPosition.X = wall.Bounds.X - player.Bounds.Width - delta;
                                            break;
                                        case MovingState.North:
                                            delta = (wall.Bounds.Y + wall.Bounds.Height) - player.Bounds.Y;
                                            player.curPosition.Y = wall.Bounds.Y + wall.Bounds.Height + delta;
                                            break;
                                        case MovingState.West:
                                            delta = (wall.Bounds.X + wall.Bounds.Width) - player.Bounds.X;
                                            player.curPosition.X = wall.Bounds.X + wall.Bounds.Width + delta + 1;
                                            break;
                                        case MovingState.South:
                                            delta = (player.Bounds.Y + player.Bounds.Height) - wall.Bounds.Y;
                                            player.curPosition.Y = wall.Bounds.Y - player.Bounds.Height - delta;
                                            break;
                                    }
                                }
                                if (player.bomb.Explosion.CollidesWith(wall.Bounds) && player.bomb.detonated && !wall.destroyed)
                                {
                                    if (wall.isBombable)
                                    {
                                        wall.Bounds.X = -50;
                                        wall.Bounds.Y = -50;
                                        score += 5000;
                                    }
                                }
                            }
                            if (collidable is Spike spike)
                            {
                                if (player.Bounds.CollidesWith(spike.Bounds) && spike.on)
                                {
                                    player.ouchSFX.Play(1, 0, 0);
                                    GameOverDeath();
                                }
                                if (player.bomb.Explosion.CollidesWith(spike.Bounds) && player.bomb.detonated && spike.on)
                                {
                                    spike.destroyed = true;
                                    score += 2000;
                                }
                            }
                            if (collidable is Exit exit)
                            {
                                if (player.Bounds.CollidesWith(exit.Bounds))
                                {
                                    NextLevelExit();    // Change to Transition Slide
                                    return;
                                }
                            }
                        }
                    }
                    foreach (Cell cell in current_maze.getNearCells(player.bomb))
                    {
                        foreach (iCollidable collidable in cell.collidables)
                        {
                            Wall wall = collidable as Wall;
                            Spike spike = collidable as Spike;
                            if (wall != null)
                            {
                                if (player.bomb.Explosion.CollidesWith(wall.Bounds) && player.bomb.detonated && !wall.destroyed)
                                {
                                    if (wall.isBombable)
                                    {
                                        wall.Bounds.X = -50;
                                        wall.Bounds.Y = -50;
                                        score += 5000;
                                    }
                                }
                            }
                            if (spike != null)
                            {
                                if (player.bomb.Explosion.CollidesWith(spike.Bounds) && player.bomb.detonated && spike.on)
                                {
                                    spike.destroyed = true;
                                    score += 2000;
                                }
                            }
                        }
                    }
                    if (player.Bounds.CollidesWith(player.bomb.Explosion) && player.bomb.detonated)
                    {
                        player.ouchBombSFX.Play(1, 0, 0);
                        GameOverDeath();
                    }

                    score--;
                }
            } else if (viewState == ViewState.TRANSITION_RIGHT)
            {
                
            }
            base.Update(gameTime);
        }

        public void NextLevel()
        {
            sum_score_prev_levels += score;
            score = 10000;
            current_level++;
            if (levels.TryGetValue(current_level, out current_maze))
            {
                player.curPosition = current_maze.startingPosition;
                player.Bounds.X = player.curPosition.X;
                player.Bounds.Y = player.curPosition.Y;
            }
            else
            {
                win = true;
                winner_score = sum_score_prev_levels;
            }
        }

        public void NextLevelExit()
        {
            sum_score_prev_levels += score;
            score = 10000;
            prev_maze = current_maze;
            current_level++;
            
            if (levels.TryGetValue(current_level, out current_maze))
            {
                player.curPosition = current_maze.startingPosition;
                player.Bounds.X = player.curPosition.X;
                player.Bounds.Y = player.curPosition.Y;
                viewState = ViewState.TRANSITION_RIGHT;
            }
            else
            {
                win = true;
                winner_score = sum_score_prev_levels;
            }
            
        }

        public void PreviousLevel()
        {
            sum_score_prev_levels -= score;
            score = 10000;
            current_level--;
            if (levels.TryGetValue(current_level, out current_maze))
            {
                player.curPosition = current_maze.startingPosition;
            }
            else
            {
                current_level = 0;
                current_maze = levels[current_level];
                player.curPosition = current_maze.startingPosition;
            }
        }

        public void GameOverDeath()
        {
            game_over = true;
            game_over_score = 0 + sum_score_prev_levels;
            player.curPosition = new Vector2(-100, -100);
            player.Bounds.X = player.curPosition.X;
            player.Bounds.Y = player.curPosition.Y;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            
            switch(viewState)
            {
                case ViewState.IDLE:
                    spriteBatch.Begin();
                    if (win)
                    {
                        winner.Draw(spriteBatch);
                        Vector2 messageCentered = scoreFont.MeasureString("Thanks For Playing!") / 2;
                        spriteBatch.DrawString(scoreFont, "Thanks For Playing!", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - messageCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2)), Color.Red);
                        Vector2 fontCentered = scoreFont.MeasureString("Score: " + winner_score.ToString()) / 2;
                        spriteBatch.DrawString(scoreFont, "Score: " + winner_score.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - fontCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2) + 100), Color.Black);
                    }
                    else if (game_over)
                    {
                        gameOver.Draw(spriteBatch);

                        Vector2 fontCentered = scoreFont.MeasureString("Score: " + game_over_score.ToString()) / 2;
                        spriteBatch.DrawString(scoreFont, "Score: " + game_over_score.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - fontCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2) + 100), Color.Black);
                    }
                    else
                    {
                        current_maze.Draw(spriteBatch);

                        // render the score in the top left of the screen
                        spriteBatch.DrawString(scoreFont, $"Score: {score}", Vector2.Zero, Color.Black);
                        player.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;
                case ViewState.TRANSITION_RIGHT:
                    DrawTransitionOld(prev_maze, translationX);
                    DrawTransitionNew(current_maze, translationX);
                    Debug.WriteLine($"{-translationX} , {graphics.GraphicsDevice.Viewport.Width - translationX}");

                    //float timer = 0F;
                    //while(timer < 1000000)
                    //{
                    //    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    //}
                    if (translationX == graphics.GraphicsDevice.Viewport.Width - 1)
                    { 
                        Debug.WriteLine("One More");
                    }
                    translationX++;
                    if(translationX >= graphics.GraphicsDevice.Viewport.Width)
                    {
                        viewState = ViewState.IDLE;
                        translationX = 0;
                    }
                    break;
            }
            base.Draw(gameTime);
        }

        void DrawTransitionOld(Maze maze, float translationX)
        {
            Matrix t = Matrix.CreateTranslation(-translationX, 0, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
            maze.Draw(spriteBatch);
            spriteBatch.End();
        }

        void DrawTransitionNew(Maze maze, float translationX)
        {
            Matrix t = Matrix.CreateTranslation(graphics.GraphicsDevice.Viewport.Width - translationX, 0, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
            maze.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
