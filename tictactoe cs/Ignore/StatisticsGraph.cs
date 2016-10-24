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
		const int GamesToPlay = 1000;
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
			foreach (var statistic in _statistics)
			{
				var newY = (int) (statistic.FirstWinPercentage*MaxY);
				var newX = x + 1;
				graphics.DrawLine(new Pen(Brushes.Black), new Point(x, MaxY - y), new Point(newX, MaxY - newY));
				y = newY;
				x = newX;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
			base.OnResize(e);
		}
	}
}