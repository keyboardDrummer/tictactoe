using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tictactoe_cs.Ignore;

namespace tictactoe_cs
{
    public partial class Game : Form
    {
	    readonly QLearningAI _ai;

	    public Game()
        {
            InitializeComponent();
			_ai = new QLearningAI();
			var evolver = new Evolver(_ai, new RandomIai());
	        evolver.PlayGames(StatisticsGraph.GamesToPlay);
		}

        private void button1_Click(object sender, EventArgs e)
        {
            grid1.NewGame(new RandomIai());
        }

		private void button2_Click(object sender, EventArgs e)
		{
			var window = new StatisticsGraph();
			window.Show();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			grid1.NewGame(_ai);
		}
	}
}
