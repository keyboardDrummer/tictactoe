using System;
using System.Drawing;

namespace tictactoe_cs
{
	class grid : System.Windows.Forms.Panel, IBoard
	{
		cell[][] matrix = new cell[3][];
		char first = 'X';
		char current = 'X';
		bool gameOver = false;
		int moveCounter = 0;
		private IAI _iai = new RandomIai();

		public bool? GetPosition(Position position)
		{
			switch (matrix[position.R][position.C].display)
			{
				case ' ':
					return null;
				case 'X':
					return true;
				case '0':
					return false;
			}
			throw new NotSupportedException();
		}


		public grid()
		{
			this.Size = new Size(210, 210);
			for (int r = 0; r < 3; r++)
			{
				matrix[r] = new cell[3];
			}
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					matrix[r][c] = new cell();
					matrix[r][c].display = ' ';
					matrix[r][c].color = Color.Black;
				}
			}
		}

		protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
		{
			if (!gameOver)
			{
				int r = (int) Math.Floor((double) e.Y/70);
				int c = (int) Math.Floor((double) e.X/70);
				if (matrix[r][c].display == ' ')
				{
					matrix[r][c].display = current;
					current = '0';
					this.Invalidate();
					moveCounter++;
					gameOver = checkLines();
					if (!gameOver && moveCounter < 9)
					{
						playMove();
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
					if (matrix[r][c].display != ' ')
					{
						e.Graphics.DrawString(matrix[r][c].display.ToString(), new Font("Times New Roman", 20, FontStyle.Bold),
							new SolidBrush(matrix[r][c].color), (float) ((c*70) + 25), (float) ((r*70) + 25));
					}
				}
			}
		}


		public bool checkLines()
		{
			for (int r = 0; r < 3; r++)
			{
				if (matrix[r][0].display != ' ' && matrix[r][0].display == matrix[r][1].display &&
				    matrix[r][0].display == matrix[r][2].display)
				{
					matrix[r][0].color = matrix[r][1].color = matrix[r][2].color = Color.Red;
					return true;
				}
			}
			for (int c = 0; c < 3; c++)
			{
				if (matrix[0][c].display != ' ' && matrix[0][c].display == matrix[1][c].display &&
				    matrix[0][c].display == matrix[2][c].display)
				{
					matrix[0][c].color = matrix[1][c].color = matrix[2][c].color = Color.Red;
					return true;
				}
			}
			if (matrix[0][0].display != ' ' && matrix[0][0].display == matrix[1][1].display &&
			    matrix[0][0].display == matrix[2][2].display)
			{
				matrix[0][0].color = matrix[1][1].color = matrix[2][2].color = Color.Red;
				return true;
			}
			if (matrix[0][2].display != ' ' && matrix[0][2].display == matrix[1][1].display &&
			    matrix[0][2].display == matrix[2][0].display)
			{
				matrix[0][2].color = matrix[1][1].color = matrix[2][0].color = Color.Red;
				return true;
			}
			return false;
		}

		public void newGame()
		{
			gameOver = false;
			for (int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					matrix[r][c].display = ' ';
					matrix[r][c].color = Color.Black;
				}
			}
			first = (first == 'X') ? '0' : 'X';
			current = first;
			this.Invalidate();
			moveCounter = 0;
			if (current == '0')
			{
				playMove();
			}
		}

		public void playMove()
		{
			var aiChoice = _iai.Step(this);
			matrix[aiChoice.R][aiChoice.C].display = '0';
			current = 'X';
			gameOver = checkLines();
			this.Invalidate();
		}
	}
}