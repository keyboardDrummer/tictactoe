using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tictactoe_cs.Ignore
{
	public partial class StatisticsGraph : Form
	{
		public StatisticsGraph()
		{
			InitializeComponent();
			foreach(var result in _evolver.PlayGames().Take(_gamesToPlay))
			{
				_statistics.Add(result);
			}
			Console.Write("jo");
		}

		readonly Evolver _evolver = new Evolver(new RandomIai(), new RandomIai());
		readonly IList<Statistics> _statistics = new List<Statistics>();
		int _gamesToPlay = 100;

		protected override void OnPaint(PaintEventArgs e)
		{
			int x = 0;
			int y = 0;
			e.Graphics.ScaleTransform((this.Width * 0.8f) / (float)_gamesToPlay, (this.Height * 0.8f) / (float)_gamesToPlay);
			foreach (var statistic in _statistics)
			{
				var newY = (int)(statistic.FirstWinPercentage * _gamesToPlay);
				var newX = x + 1;
				e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(x, _gamesToPlay - y), new Point(newX, _gamesToPlay - newY));
				y = newY;
				x = newX;
			}
			base.OnPaint(e);
		}
	}
}
