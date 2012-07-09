using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private const short margin = 10;
        private const short squareSize = 10;
        private Board game;
        private SolidBrush br;
        private Pen p;
        private System.Timers.Timer clock;
        private bool on = false; // game is running
        private int generation; // current generation


        /// <summary>
        /// Points of limits
        /// </summary>
        private Point zero;
        private Point xLimit;
        private Point yLimit;
        private Point limit;
        
        public Form1()
        {
            InitializeComponent();
            game = new Board();
            br = new SolidBrush(Color.Black);
            p = new Pen(br);
            generation = 1;
            clock = new System.Timers.Timer(1000);
            clock.Elapsed += new ElapsedEventHandler(Advance);
            zero = new Point(margin, margin);
            xLimit = new Point(margin + game.Size * squareSize, margin);
            yLimit = new Point(margin, margin + game.Size * squareSize);
            limit = new Point(margin + game.Size * squareSize, margin + game.Size * squareSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (on) // if game is running, stop game
            { 
                on = false;
                clock.Stop();
                startButton.Text = "Start";
            }
            else // if game isn't running, start game
            {
                try
                {
                    clock.Interval = int.Parse(textBox1.Text);
                }
                catch
                {
                    clock.Interval = 1000;
                }
                on = true;
                clock.Start();
                startButton.Text = "Stop";
            }
        }
        private void Advance(object sender, EventArgs e)
        {
            game.TickNextGen();
            updateCounter();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            drawBoard(e.Graphics);
        }

        private void updateCounter()
        {
            if (counter.InvokeRequired)
                counter.Invoke(new Action(updateCounter));
            else
            {
                generation++;
                counter.Text = generation.ToString();
            }
        }

        /// <summary>
        /// Draws board game on form
        /// </summary>
        private void drawBoard(Graphics painter)
        {
            painter.Clear(SystemColors.Control);
            painter.DrawLine(p, zero, xLimit);
            painter.DrawLine(p, zero, yLimit);
            painter.DrawLine(p, limit, xLimit);
            painter.DrawLine(p, limit, yLimit);
            for (int i = 0; i < game.Size; i++)
            {
                for (int j = 0; j < game.Size - 1; j++)
                {
                    if (game.Grid[i, j] == (int)CellStatus.alive)
                       painter.FillRectangle(br, new Rectangle(margin + i * squareSize, margin + j * squareSize, squareSize, squareSize));      
                }
             }
        }
    }
}
