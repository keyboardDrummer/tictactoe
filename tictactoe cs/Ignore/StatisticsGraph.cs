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
		const int GamesToPlay = 100000;
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
			int x = 0;
			int y = 0;
			int dx = 0;
			int dy = 0;
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(GamesToPlay, 0));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY/2), new Point(GamesToPlay, MaxY/2));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, MaxY), new Point(GamesToPlay, MaxY));
			graphics.DrawLine(new Pen(Brushes.Gray), new Point(0, 0), new Point(0, MaxY));
			foreach (int interval in Enumerable.Range(1, 9).Except(new[] {5}))
			{
				graphics.DrawLine(new Pen(Brushes.LightGray), new Point(0, MaxY/10*interval),
					new Point(GamesToPlay, MaxY/10*interval));
			}
			foreach (var statistic in _statistics)
			{
				var newY = (int) (statistic.FirstWinPercentage*MaxY);
				var newX = x + 1;
				graphics.DrawLine(new Pen(Brushes.Black), new Point(x, MaxY - y), new Point(newX, MaxY - newY));
				y = newY;
				x = newX;

				var newdY = (int) (statistic.DrawPercentage*MaxY);
				var newdX = dx + 1;
				graphics.DrawLine(new Pen(Brushes.Green), new Point(dx, MaxY - dy), new Point(newdX, MaxY - newdY));
				dy = newdY;
				dx = newdX;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
			base.OnResize(e);
		}
	}
}