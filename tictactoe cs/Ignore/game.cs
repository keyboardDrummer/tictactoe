using System;
using System.Windows.Forms;

namespace tictactoe_cs
{
	public partial class Game : Form
	{
		MyOwnPlayer _qPlayer = new MyOwnPlayer();

		public Game()
		{
			InitializeComponent();
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
			_qPlayer = new MyOwnPlayer();
			_qPlayer.Learn();
			Stats.Print(_qPlayer);
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