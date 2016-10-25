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
			var first = new QLearningAI();
			var evolver = new Evolver(first, new RandomIai());

			foreach (var result in evolver.PlayGames().Take(GamesToPlay))
			{
				_statistics.Add(result);
			}
			WindowState = FormWindowState.Maximized;
		}

		readonly IList<Statistics> _statistics = new List<Statistics>();
		const int GamesToPlay = 10000;
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
			graphics.DrawLine(new Pen(Brushes.Red), new Point(0, MaxY / 2), new Point(GamesToPlay, MaxY / 2));
			int x = 0;
			int y = 0;
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(GamesToPlay, 0));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY/2), new Point(GamesToPlay, MaxY/2));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY), new Point(GamesToPlay, MaxY));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(0, MaxY));
			foreach (int interval in Enumerable.Range(1, 9).Except(new[] {5}))
			{
				graphics.DrawLine(new Pen(Brushes.LightGray), new Point(0, MaxY/10*interval),
					new Point(GamesToPlay, MaxY/10*interval));
			}
			int y2 = 0;
			foreach (var statistic in _statistics)
			{
				var newX = x + 1;
				var first = statistic.FirstWinPercentage;
				y = DrawLinePart(graphics, first, x, y, newX, Brushes.Black);
				y2 = DrawLinePart(graphics, statistic.FirstWinOrTiePercentage, x, y2, newX, Brushes.Blue);
				x = newX;
			}
		}

		static int DrawLinePart(Graphics graphics, double first, int x, int y, int newX, Brush black)
		{
			var newY = (int) (first*MaxY);
			graphics.DrawLine(new Pen(black), new Point(x, MaxY - y), new Point(newX, MaxY - newY));
			y = newY;
			return y;
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
			base.OnResize(e);
		}
	}
}
