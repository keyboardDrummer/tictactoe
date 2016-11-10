using System;
using System.Drawing;
using System.Windows.Forms;
using qlttt;

namespace tictactoe_cs
{
	public class Grid : System.Windows.Forms.Panel, IBoard
	{
		public readonly cell[][] Matrix = new cell[3][];
		char first = 'X';
		char current = 'X';
		bool gameOver;
		int moveCounter;
		IPlayer _iai;

		public CellValue GetPosition(Position position)
		{
			switch (Matrix[position.R][position.C].display)
			{
				case ' ':
					return CellValue.Empty;
				case 'X':
					return CellValue.Cross;
				case '0':
					return CellValue.Ring;
			}
			throw new NotSupportedException();
		}


		public Grid()
		{
			this.Size = new Size(210, 210);
			for (int r = 0; r < 3; r++)
			{
				Matrix[r] = new cell[3];
			}
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					Matrix[r][c] = new cell();
					Matrix[r][c].display = ' ';
					Matrix[r][c].color = Color.Black;
				}
			}
		}

		protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
		{
			if (!gameOver)
			{
				int r = (int) Math.Floor((double) e.Y/70);
				int c = (int) Math.Floor((double) e.X/70);
				if (Matrix[r][c].display == ' ')
				{
					Matrix[r][c].display = current;
					current = '0';
					Invalidate();
					moveCounter++;
					gameOver = CheckLines();
					if (!gameOver && moveCounter < 9)
					{
						PlayMove();
						moveCounter++;
					}
				}
			}
			base.OnMouseClick(e);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawLine(Pens.Black, 70, 0, 70, 210);
			e.Graphics.DrawLine(Pens.Black, 140, 0, 140, 210);
			e.Graphics.DrawLine(Pens.Black, 0, 70, 210, 70);
			e.Graphics.DrawLine(Pens.Black, 0, 140, 210, 140);

			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					if (Matrix[r][c].display != ' ')
					{
						e.Graphics.DrawString(Matrix[r][c].display.ToString(), new Font("Times New Roman", 20, FontStyle.Bold),
							new SolidBrush(Matrix[r][c].color), (float) ((c*70) + 25), (float) ((r*70) + 25));
					}
				}
			}
		}


		public bool CheckLines()
		{
			for (int r = 0; r < 3; r++)
			{
				if (Matrix[r][0].display != ' ' && Matrix[r][0].display == Matrix[r][1].display &&
				    Matrix[r][0].display == Matrix[r][2].display)
				{
					Matrix[r][0].color = Matrix[r][1].color = Matrix[r][2].color = Color.Red;
					return true;
				}
			}
			for (int c = 0; c < 3; c++)
			{
				if (Matrix[0][c].display != ' ' && Matrix[0][c].display == Matrix[1][c].display &&
				    Matrix[0][c].display == Matrix[2][c].display)
				{
					Matrix[0][c].color = Matrix[1][c].color = Matrix[2][c].color = Color.Red;
					return true;
				}
			}
			if (Matrix[0][0].display != ' ' && Matrix[0][0].display == Matrix[1][1].display &&
			    Matrix[0][0].display == Matrix[2][2].display)
			{
				Matrix[0][0].color = Matrix[1][1].color = Matrix[2][2].color = Color.Red;
				return true;
			}
			if (Matrix[0][2].display != ' ' && Matrix[0][2].display == Matrix[1][1].display &&
			    Matrix[0][2].display == Matrix[2][0].display)
			{
				Matrix[0][2].color = Matrix[1][1].color = Matrix[2][0].color = Color.Red;
				return true;
			}
			return false;
		}

		public void NewGame(IPlayer ai)
		{
			_iai = ai;
			gameOver = false;
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					Matrix[r][c].display = ' ';
					Matrix[r][c].color = Color.Black;
				}
			}
			first = first == 'X' ? '0' : 'X';
			current = first;
			Invalidate();
			moveCounter = 0;
			if (current == '0')
			{
				PlayMove();
			}
		}

		public void PlayMove()
		{
			var aiChoice = _iai.Play(new qlttt.Board(this), first == 'X' ? 2 : 1);
			var cell = Matrix[aiChoice/3][aiChoice%3];
			if (cell.display == ' ')
			{
				cell.display = '0';
				current = 'X';
				gameOver = CheckLines();
			}
			else
			{
				MessageBox.Show("AI made a move on an existing position!");
				gameOver = true;
			}
			Invalidate();
		}
	}
}