﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szachy
{
    public partial class Form1 : Form
    {
        public Button[,] boardButtons ;
        private Game.Board board;
        Bitmap[,] piecesBitmap;
        Pieces.Piece currentPiece;
        object lokokontigo;
        public Game.Board tmpb;

        public Form1(Game.Board chessBoard)
        {
            InitializeComponent();
            boardButtons = new Button[8, 8];
            piecesBitmap = new Bitmap[6, 2];
            loadPiecesBitmap();
            board = chessBoard;
            GenerateBoardPanel();
        }
        public Form1()
        {
            InitializeComponent();
            boardButtons = new Button[8, 8];
            piecesBitmap = new Bitmap[6, 2];
            loadPiecesBitmap();
            board = new Game.Board(new Game.Gamer(), new Game.PCPlayer());
            GenerateBoardPanel();
           // reveseBoard();
            refreshBoard();
            currentPiece = null;
            tmpb = new Game.Board(new Game.Gamer(), new Game.Gamer());
            lokokontigo = new Pieces.EmptyPiece();

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
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Point pos = (Point)boardButtons[i, j].Tag;
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


        private void BoardClick(object sender, EventArgs e)
        {
            lock (lokokontigo)
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
                    List<Point> moves = currentPiece.GetPossibleMoves();

                    Point zerozero = (Point)boardButtons[0, 0].Tag;
                    if (zerozero.X == 0)
                    {
                        foreach (Point move in moves)
                        {
                            boardButtons[move.X,7- move.Y].BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        foreach (Point move in moves)
                        {
                            boardButtons[7 - move.X,  move.Y].BackColor = Color.Red;
                        }
                    }
                    return;
                }

                if (clickedPiece.colour != board.turn)
                {
                    if (currentPiece != null)
                    {
                        //Console.WriteLine("a");
                        if (currentPiece.checkMove(position.X, position.Y))
                        {
                            int val = currentPiece.move(position);
                            currentPiece = null;
                            if (val < 0)
                            {
                                MessageBox.Show("bron krola");
                            }

                            if (val >= 200)
                            {
                                MessageBox.Show("szachmat");
                                val = val % 10;
                                if (board.turn == PlayerColour.WHITE) val = -val;
                                listBox1.Items.Add(val.ToString() + "#");
                            }
                            else if (val >= 100)
                            {
                                MessageBox.Show("dales szacha!");
                                val = val % 10;
                                if (board.turn == PlayerColour.WHITE) val = -val;
                                listBox1.Items.Add(val.ToString() + "+");
                            }
                            else
                            {
                                if (board.turn == PlayerColour.WHITE) val = -val;
                                listBox1.Items.Add(val.ToString());
                            }
                            refreshBoard();
                            if (!board.players[(int)(board.turn)].realPerson)
                            {
                                board.players[(int)(board.turn)].makeMove(board, board.turn);
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
    }
}
