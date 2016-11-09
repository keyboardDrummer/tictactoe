using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace tictactoe_cs.Ignore
{
	public partial class StatisticsGraph : Form
	{
		public StatisticsGraph()
		{
			InitializeComponent();
			var ai = new QLearningTicTacToeAI();
			var evolver = new Evolver(ai, new RandomIai());

			foreach (var result in evolver.PlayGames().Take(GamesToPlay))
			{
				_statistics.Add(result);
			}
			WindowState = FormWindowState.Maximized;
		}

		readonly IList<Statistics> _statistics = new List<Statistics>();
		public const int GamesToPlay = 10000;
		const int MaxY = 1000;

		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			var margin = 10f;
			graphics.TranslateTransform(margin, margin);
			graphics.ScaleTransform((ClientSize.Width - 2*margin)/GamesToPlay, (ClientSize.Height - 2*margin)/MaxY);
			DrawOnTransformedGraphics(graphics);
			base.OnPaint(e);
		}

		void DrawOnTransformedGraphics(Graphics graphics)
		{
			DrawGrid(graphics);
			int x = 0;
			int winY = 0;
			int winOrTieY = 0;
			foreach (var statistic in _statistics)
			{
				var newX = x + 1;
				winY = DrawLinePart(graphics, statistic.FirstWinPercentage, x, winY, newX, Brushes.Black);
				winOrTieY = DrawLinePart(graphics, statistic.FirstWinOrTiePercentage, x, winOrTieY, newX, Brushes.Blue);
				x = newX;
			}
		}

		static void DrawGrid(Graphics graphics)
		{
			graphics.DrawLine(new Pen(Brushes.Red), new Point(0, MaxY/2), new Point(GamesToPlay, MaxY/2));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(GamesToPlay, 0));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY/2), new Point(GamesToPlay, MaxY/2));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY), new Point(GamesToPlay, MaxY));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(0, MaxY));
			foreach (int interval in Enumerable.Range(1, 9).Except(new[] {5}))
			{
				graphics.DrawLine(new Pen(Brushes.LightGray), new Point(0, MaxY/10*interval),
					new Point(GamesToPlay, MaxY/10*interval));
			}
		}

		static int DrawLinePart(Graphics graphics, double relativeNewY, int x, int y, int newX, Brush black)
		{
			var newY = (int) (relativeNewY*MaxY);
			graphics.DrawLine(new Pen(black), new Point(x, MaxY - y), new Point(newX, MaxY - newY));
			return newY;
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
			base.OnResize(e);
		}
	}
}