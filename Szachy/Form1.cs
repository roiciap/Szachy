using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Szachy.Game;

namespace Szachy
{
    public partial class Form1 : Form
    {
        public Button[,] boardButtons ;
        private Game.Board board;
        Bitmap[,] piecesBitmap;
        Pieces.Piece currentPiece;
        public Game.Board tmpb;//save
        public List<string> movesHistory { get; set; }

        private bool showPossibleMoves { get; set; }
        private bool showKingAtackedInfo { get; set; }

        public Form1(Game.Board chessBoard)
        {
            InitializeComponent();
            boardButtons = new Button[8, 8];
            piecesBitmap = new Bitmap[6, 2];
            loadPiecesBitmap();
            board = chessBoard;
            GenerateBoardPanel();
            movesHistory = new List<string>();
            showKingAtackedInfo = false;
            showPossibleMoves = false;
        }
        public Form1(Player white,Player black)
        {
            InitializeComponent();
            boardButtons = new Button[8, 8];
            piecesBitmap = new Bitmap[6, 2];
            loadPiecesBitmap();
            board = new Game.Board(white, black);
            GenerateBoardPanel();
            reveseBoard();
            refreshBoard();
            currentPiece = null;
            tmpb = new Game.Board(new Game.Gamer(), new Game.Gamer());
            movesHistory = new List<string>();
            if (!white.realPerson)
            {
                white.makeMove(board, PlayerColour.WHITE, this);
            }
            showKingAtackedInfo = false;
            showPossibleMoves = false;
        }

        private void loadPiecesBitmap()
        {
            Bitmap img = new Bitmap("SeZ5y.png");
            for(int i = 0; i < 6; i++)
            {
                for(int j=0; j < 2; j++)
                {
                    Rectangle rect = new Rectangle(i*64, j*64, 64, 64);
                    piecesBitmap[i,j] = img.Clone(rect, img.PixelFormat);
                }
            }


        }

        private void GenerateBoardPanel()
        {
            int width=boardPanel.Width/8;
            int height=boardPanel.Height/8;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    boardButtons[i, j] = new Button();
                    boardButtons[i, j].Width = width;
                    boardButtons[i, j].Height = height;
                    boardButtons[i, j].Click += BoardClick;
                    boardButtons[i,j].Location = new Point(i*width,j*height);
                    boardPanel.Controls.Add(boardButtons[i,j]);

                    boardButtons[i, j].Tag=new Point(i,7-j);
                }
            }
            refreshBoard();
        }


        public void refreshBoard()
        {
            string[] linie = { "A", "B", "C", "D", "E", "F", "G", "H" };
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Point pos = (Point)boardButtons[i, j].Tag;
                  //  boardButtons[i, j].Text = (linie[pos.X] + " " + (pos.Y+1));
                    if (pos.X % 2 == pos.Y % 2)
                    {
                        boardButtons[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        boardButtons[i, j].BackColor = Color.Black;
                    }
                    if (board.getPiece(pos.X, pos.Y).PieceType != PiecesType.NONE)
                    {
                        boardButtons[i,j].Image=piecesBitmap[(int )board.getPiece(pos.X,pos.Y).PieceType,(int )board.getPiece(pos.X, pos.Y).colour];
                    }
                    else
                    {
                        boardButtons[i,j].Image = null;
                    }
                }
            }
            
        }

        public void reveseBoard()
        {
            Button[,] reversed = new Button[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    reversed[i, j] = boardButtons[7 - i, 7 - j];
                }
            }
            boardButtons = reversed;
            for(int i= 0; i < 8; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    var tmp=boardButtons[i, j].Tag;
                    boardButtons[i, j].Tag = boardButtons[7 - i, 7 - j].Tag;
                    boardButtons[7 - i, 7 - j].Tag = tmp;
                }
            }
        }

        public void AddMoveHistory(string pgn)
        {
         //   listBox1.Items.Add(pgn);
        }

        public ListBox.ObjectCollection getMoveHistory()
        {
            return listBox1.Items;
        }
        private void BoardClick(object sender, EventArgs e)
        {
            if (!board.players[(int)board.turn].realPerson)
            {
                return;
            }


            Button clickedButton = (Button)sender;
            Point position = (Point)clickedButton.Tag;

            Pieces.Piece clickedPiece = board.getPiece(position.X, position.Y);
            refreshBoard();
            if (clickedPiece.colour == board.turn)
            {
                currentPiece = clickedPiece;
                if (showPossibleMoves)
                {
                    List<Point> moves = currentPiece.GetPossibleMoves();

                    Point zerozero = (Point)boardButtons[0, 0].Tag;
                    if (zerozero.X == 0)
                    {
                        foreach (Point move in moves)
                        {
                            boardButtons[move.X, 7 - move.Y].BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        foreach (Point move in moves)
                        {
                            boardButtons[7 - move.X, move.Y].BackColor = Color.Red;
                        }
                    }
                }
                else
                {
                    clickedButton.BackColor = Color.Red;
                }
                return;
            }

            if (clickedPiece.colour != board.turn)
            {
                if (currentPiece != null)
                {
                    if (currentPiece.checkMove(position.X, position.Y))
                    {
                        string[] columns = { "a", "b", "c", "d", "e", "f", "g", "h" };
                        string[] pieceString = { "K", "Q", "R", "N", "B", "" };
                        var movement = new MoveInfo()
                        {
                            uci = columns[currentPiece.position.X] + (currentPiece.position.Y + 1) + columns[position.X] + (position.Y + 1),
                            pgn = pieceString[(int)currentPiece.PieceType] + columns[position.X] + (position.Y + 1),
                            value = 0
                        };

                        int val = currentPiece.move(position);
                        movement.value = val;
                        movesHistory.Add(movement.uci);
                        currentPiece = null;
                        if (val == -200)
                        {
                            if (showKingAtackedInfo)
                                MessageBox.Show("BROŃ KRÓLA !");
                        }




                        if (val >= 2000)
                        {
                            if (showKingAtackedInfo)
                                MessageBox.Show("SZACHMAT");
                            listBox1.Items.Add(movement.pgn + "#");
                            refreshBoard();
                            return;
                        }
                        else if (val >= 1000)
                        {
                            if (showKingAtackedInfo)
                                MessageBox.Show("SZACH");
                            listBox1.Items.Add(movement.pgn + "+");
                        }
                        else if(val!=-200)
                        {
                            listBox1.Items.Add(movement.pgn);
                        }
                        refreshBoard();

                        if (!board.players[(int)(board.turn)].realPerson)
                        {
                            Thread t = new Thread(a => board.players[(int)(board.turn)].makeMove(board, board.turn, this));
                            t.Start();

                            refreshBoard();
                        }
                    }
                    else
                    {
                        currentPiece = null;
                    }
                }
            }



        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            tmpb = board.Clone();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            board = tmpb.Clone();
            refreshBoard();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                showPossibleMoves = true;
            }
            else
            {
                showPossibleMoves = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                showKingAtackedInfo = true;
            }
            else
            {
                showKingAtackedInfo = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reveseBoard();
            currentPiece = null;
            refreshBoard();
        }
    }
}
