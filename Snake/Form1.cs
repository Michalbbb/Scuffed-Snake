using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        SnakeBoard game;
        private Graphics g;
        private const int cellSize=25;
        bool gameOver = false;
        public Form1()
        {
            InitializeComponent();
            setGame();

        }
        private void setGame()
        {
            gameOver = false;
            game = new SnakeBoard(new Size(20, 20));
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Padding = new Padding(0);
            pictureBox1.Width = cellSize * game.size.Width;
            pictureBox1.Height = cellSize * game.size.Height;

            pictureBox1.Image = new Bitmap(game.size.Width * cellSize, game.size.Height * cellSize);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            g = Graphics.FromImage(pictureBox1.Image);
            DrawBoard();
            game.BoardChanged += DrawBoard;
            game.GameIsOver += MakeGameReapeatable; 
        }
        private void MakeGameReapeatable()
        {
            gameOver = true;
        }
        private void DrawBoard()
        {
            g.Clear(Color.White);
            for(int i=0;i< game.size.Width; i++)
            {
                for(int j=0;j< game.size.Height; j++)
                {
                    g.DrawRectangle(new Pen(Color.Black), i * cellSize, j * cellSize, cellSize, cellSize);
                }
            }
            bool firstOne = true;
            foreach(Point c in game.getSnake())
            {
                if (firstOne)
                {
                    firstOne = false;
                    g.FillRectangle(Brushes.Green, c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                    g.DrawRectangle(new Pen(Color.Black), c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                    continue;
                }
                g.FillRectangle(Brushes.LightGreen, c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                g.DrawRectangle(new Pen(Color.Black), c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
            }
            foreach(Point c in game.drawObstacles())
            {
                g.FillRectangle(Brushes.SaddleBrown, c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                g.DrawRectangle(new Pen(Color.Black), c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
            }
            g.FillRectangle(Brushes.Red, game.getApple().X* cellSize, game.getApple().Y* cellSize, cellSize, cellSize);
            g.DrawRectangle(new Pen(Color.Black), game.getApple().X* cellSize, game.getApple().Y* cellSize, cellSize, cellSize);
            pictureBox1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && game.snake.blocked() != Snake.Direction.LEFT) { game.snake.dir = Snake.Direction.LEFT; }
            if (e.KeyCode == Keys.D && game.snake.blocked() != Snake.Direction.RIGHT) { game.snake.dir = Snake.Direction.RIGHT; }
            if (e.KeyCode == Keys.W && game.snake.blocked() != Snake.Direction.UP) { game.snake.dir = Snake.Direction.UP; }
            if (e.KeyCode == Keys.S && game.snake.blocked() != Snake.Direction.DOWN) { game.snake.dir = Snake.Direction.DOWN; }
            if (e.KeyCode == Keys.R && gameOver) setGame();
        }
    }
}
