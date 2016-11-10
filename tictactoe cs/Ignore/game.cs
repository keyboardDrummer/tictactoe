using System;
using System.Windows.Forms;
using qlttt;
using tictactoe_cs.Ignore;

namespace tictactoe_cs
{
	public partial class Game : Form
	{
		readonly QLearningTicTacToeAI _ai;
		QPlayer _qPlayer = new QPlayer();

		public Game()
		{
			InitializeComponent();
			_ai = new QLearningTicTacToeAI();
			var evolver = new Evolver(_ai, new RandomIai());
			evolver.PlayGames(StatisticsGraph.GamesToPlay);
		}

		/// <summary>
		/// New game against random player
		/// </summary>
		private void button1_Click(object sender, EventArgs e)
		{
			grid1.NewGame(new RandomPlayer());
		}

		/// <summary>
		/// Evolve QPlayer/AI
		/// </summary>
		private void button2_Click(object sender, EventArgs e)
		{
			_qPlayer.Learn();
			//var window = new StatisticsGraph();
			//window.Show();
		}

		/// <summary>
		/// New game against QPlayer/AI
		/// </summary>
		private void button3_Click(object sender, EventArgs e)
		{
			grid1.NewGame(_qPlayer);
		}
	}
}