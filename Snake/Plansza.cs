using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    class SnakeBoard
    {
        int punkty;
        Point apple;
        public Snake snake;
        public Size size { get; }
        public event Action BoardChanged;
        public event Action GameIsOver;
        Timer timer;
        List<Point> obstacles;

        public SnakeBoard(Size sizeN)
        {
            obstacles = new List<Point>();
            punkty = 0;
            size = sizeN;
            
            List<Point> tmp = new List<Point>();
            
            tmp.Add(new Point(size.Width / 2, size.Height / 2 - 1));
            tmp.Add(new Point(size.Width / 2, size.Height / 2));
            tmp.Add(new Point(size.Width / 2, size.Height / 2 + 1));
            tmp.Add(new Point(size.Width / 2, size.Height / 2 + 2));
            tmp.Add(new Point(size.Width / 2, size.Height / 2 + 3));
            snake = new Snake(tmp);
            timer = new Timer();
            generateApple();
            timer.Interval = 220;
            timer.Start();
            timer.Tick += Timer_Tick; ;


        }
        private void generateApple()
        {
            Random rnd = new Random();
            bool gotGoodOne = false;
            while (!gotGoodOne)
            {
                gotGoodOne = true;
                apple = new Point(rnd.Next(0, size.Width), rnd.Next(0, size.Height));
                foreach (Point c in snake.segments)
                {
                    if (c.X == apple.X && c.Y == apple.Y)
                    {
                        gotGoodOne = false;
                        break;
                    }
                }
                if (obstacles.Count > 0)
                {
                    foreach (Point c in obstacles)
                    {
                        if (c.X == apple.X && c.Y == apple.Y)
                        {
                            gotGoodOne = false;
                            break;
                        }
                    }
                }

            }
        }
        public Point getApple()
        {
            return apple;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Point x=new Point(0,0);
            Point returnMe= snake.segments[snake.segments.Count - 1];
            snake.segments.RemoveAt(snake.segments.Count - 1);
            if (snake.dir == Snake.Direction.LEFT)
            {
                x = new Point(snake.segments[0].X-1, snake.segments[0].Y);
                foreach (Point c in snake.segments)
                {
                    if (c.X == x.X && c.Y == x.Y)
                    {
                        timer.Stop(); GameIsOver?.Invoke();
                        return;
                    }
                }
                snake.segments.Insert(0,x);

            }
            if (snake.dir == Snake.Direction.RIGHT)
            {
                x = new Point(snake.segments[0].X + 1, snake.segments[0].Y);
                foreach (Point c in snake.segments)
                {
                    if (c.X == x.X && c.Y == x.Y)
                    {
                        timer.Stop(); GameIsOver?.Invoke();
                        return;
                    }
                }
                snake.segments.Insert(0, x);
            }
            if (snake.dir == Snake.Direction.DOWN)
            {
                x = new Point(snake.segments[0].X, snake.segments[0].Y + 1);
                foreach (Point c in snake.segments)
                {
                    if (c.X == x.X && c.Y == x.Y)
                    {
                        timer.Stop(); GameIsOver?.Invoke();
                        return;
                    }
                }
                snake.segments.Insert(0, x);

            }
            if (snake.dir == Snake.Direction.UP)
            {
                x = new Point(snake.segments[0].X, snake.segments[0].Y - 1);
                foreach (Point c in snake.segments)
                {
                    if(c.X==x.X&&c.Y==x.Y)
                    {
                        timer.Stop(); GameIsOver?.Invoke();
                        return;
                    }
                }
                snake.segments.Insert(0, x);

            }
            
            if (x.X == apple.X && x.Y == apple.Y)
            {
                snake.segments.Add(returnMe);
                punkty++;
                 generateObstacle();
                generateApple();
                if(timer.Interval>50)timer.Interval -= 5;
            }
            if (snake.segments[0].Y < 0 || snake.segments[0].Y >= size.Height)
            {
                timer.Stop(); GameIsOver?.Invoke();
                return;
            }
            if (snake.segments[0].X < 0 || snake.segments[0].X >= size.Width)
            {
                timer.Stop();
                GameIsOver?.Invoke();
                return;
            }
            foreach(Point obstacle in obstacles)
            {
                if(obstacle.X==snake.segments[0].X&& obstacle.Y == snake.segments[0].Y)
                {
                    timer.Stop();
                    GameIsOver?.Invoke();
                    return;
                }
            }
           

            BoardChanged?.Invoke();
        }
        public List<Point> drawObstacles()
        {
            return obstacles;
        }
        private void generateObstacle()
        {
            Random rnd = new Random();
            bool gotGoodOne = false;
            int howMany = rnd.Next(0, 6);
            Point addMeToObstacles;
            int added = 0;
                while (added!=howMany)
                {
                    
                    gotGoodOne = true;
                    addMeToObstacles = new Point(rnd.Next(0, size.Width), rnd.Next(0, size.Height));
                    foreach (Point c in snake.segments)
                    {
                        if (c.X == addMeToObstacles.X && c.Y == addMeToObstacles.Y)
                        {
                            gotGoodOne = false;
                            break;
                        }
                    }
                    foreach (Point c in obstacles)
                    {
                        if (c.X == addMeToObstacles.X && c.Y == addMeToObstacles.Y)
                        {
                            gotGoodOne = false;
                            break;
                        }
                    }
                if (gotGoodOne)
                {
                    obstacles.Add(addMeToObstacles);
                    added++;
                }
               }
            
            
        }
        

        public List<Point> getSnake()
        {
            return snake.segments;
        }

    }
    class Snake
    {
        public List<Point> segments;
        
        public enum Direction {UP,DOWN,RIGHT,LEFT};
        public Direction blocked()
        {
           
            if (segments.Count > 2)
            {
                int c = segments[0].X - segments[1].X;
                if (c != 0)
                {
                    if (c > 0) return Direction.LEFT;
                    else return Direction.RIGHT;
                }
                c = segments[0].Y - segments[1].Y;
                if (c != 0)
                {
                    if (c < 0) return Direction.DOWN;
                    else return Direction.UP;
                }
            }

            return Direction.DOWN;
        }
        public Direction dir;
        public Snake(List<Point> startPosition)
        {
            dir = Direction.UP;
            segments = startPosition;
        }

    }
}
