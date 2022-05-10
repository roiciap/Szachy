using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szachy.Game
{
    public class Board
    {
        


        private Pieces.Piece[, ] board;

        public PlayerColour turn;

        public Player[] players;

        public BoardSettings SETTINGS;


        public Board(Player White , Player Black)
        {
            board = new Pieces.Piece[8, 8];
            GenerateStartingBoard();
            turn = PlayerColour.WHITE;
            SETTINGS= new BoardSettings();
            players=new Player[2];
            players[1] = White;
            players[0] = Black;
            refreshPlayerPieces();

        }
        public Board(Player White, Player Black, BoardSettings settings)
        {
            board = new Pieces.Piece[8, 8];
            GenerateStartingBoard();
            turn = PlayerColour.WHITE;
            SETTINGS = settings;
            players = new Player[2];
            players[1] = White;
            players[0] = Black;
            refreshPlayerPieces();
        }

        public Board(Player White, Player Black, BoardSettings settings, Pieces.Piece[,] chessBoard)
        {
            board = chessBoard;
            board = new Pieces.Piece[8, 8];
            GenerateStartingBoard();
            turn = PlayerColour.WHITE;
            SETTINGS = settings;
            players = new Player[2];
            players[1] = White;
            players[0] = Black;
            refreshPlayerPieces();
        }

        private void GenerateStartingBoard()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    board[i, j] = new Pieces.EmptyPiece(i, j, this);
                }
            }

            for (int i = 0; i < 8; i++)
            {
               board[i, 1] = new Pieces.Pawn(PlayerColour.WHITE, i, 1, this);
               board[i, 6] = new Pieces.Pawn(PlayerColour.BLACK, i, 6, this);
            }

            board[3, 0] = new Pieces.King(PlayerColour.WHITE, 3, 0, this);
            board[3, 7] = new Pieces.King(PlayerColour.BLACK, 3, 7, this);
            board[4, 0] = new Pieces.Queen(PlayerColour.WHITE, 4, 0, this);
            board[4, 7] = new Pieces.Queen(PlayerColour.BLACK, 4, 7, this);
            board[0, 0] = new Pieces.Rook(PlayerColour.WHITE, 0, 0, this);
            board[7,0]=new Pieces.Rook(PlayerColour.WHITE,7,0, this);
            board[0, 7] = new Pieces.Rook(PlayerColour.BLACK, 0, 7, this);
            board[7, 7] = new Pieces.Rook(PlayerColour.BLACK, 7, 7, this);
            board[1, 0] = new Pieces.Knight(PlayerColour.WHITE, 1, 0, this);
            board[6, 0] = new Pieces.Knight(PlayerColour.WHITE, 6, 0, this);
            board[1, 7] = new Pieces.Knight(PlayerColour.BLACK, 1, 7, this);
            board[6, 7] = new Pieces.Knight(PlayerColour.BLACK, 6, 7, this);
            board[2, 0] = new Pieces.Bishop(PlayerColour.WHITE, 2, 0, this);
            board[5, 0] = new Pieces.Bishop(PlayerColour.WHITE, 5, 0, this);
            board[2, 7] = new Pieces.Bishop(PlayerColour.BLACK, 2, 7, this);
            board[5, 7] = new Pieces.Bishop(PlayerColour.BLACK, 5, 7, this);



        }
        
        public Pieces.Piece getPiece(int x , int y)
        {
            if(x>=0 && y>=0 && x<8 && y<8)
                return board[x, y];
            else return new Pieces.EmptyPiece();
        }

        public void refreshPlayerPieces()
        {
            List<Pieces.Piece> whitePieces = new List<Pieces.Piece>();
            List<Pieces.Piece> blackPieces = new List<Pieces.Piece>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].PieceType != PiecesType.NONE)
                    {
                        if (board[i, j].colour == PlayerColour.BLACK)
                        {
                            blackPieces.Add(board[i, j]);
                        }
                        else if (board[i, j].colour == PlayerColour.WHITE)
                        {
                            whitePieces.Add(board[i, j]);
                        }
                    }
                }
            }
            players[1].pieces = whitePieces;
            players[0].pieces = blackPieces;

        }


        public int kingUnderAttack()
        {
            Board b = Clone();

            refreshPlayerPieces();
            foreach (Pieces.Piece piece in players[(int)turn].pieces)
            {
                if (piece.PieceType == PiecesType.KING)
                {

                    if (players[((int)turn + 1) % 2].getAttackingPoints().Contains(piece.position))
                    {
                        return -1;//gdy gracz sie ruszy jego krol bedzie pod atakiem
                    }
                }
            }
            
            foreach (Pieces.Piece piece in b.players[((int)turn + 1) % 2].pieces)
            {
                if (piece.PieceType == PiecesType.KING)
                {

                    if (players[((int)turn)].getAttackingPoints().Contains(piece.position))
                    {
                        if (b.turn == PlayerColour.WHITE)
                        {
                            b.turn = PlayerColour.BLACK;
                        }
                        else
                        {
                            b.turn = PlayerColour.WHITE;
                        }
                        b.refreshPlayerPieces();
                        foreach (Pieces.Piece p in b.players[(int)b.turn].pieces)
                        {
                            foreach (Point l in p.GetPossibleMoves())
                            {
                                int xd = p.move(l);
                                if (xd >= 0)
                                {
                                    return 1;
                                }
                            }
                        }

                        return 2;//checkmate
                    }
                }
            }
            
            return 0;
        }

        public int moved(Point from, Point to)
        {
            if (from.X >= 0 && from.Y >= 0 && from.X < 8 && from.Y < 8)
            {
                if (to.X >= 0 && to.Y >= 0 && to.X < 8 && to.Y < 8)
                {
                    if(board[from.X,from.Y].colour != turn)
                    {
                        return -1;
                    }
                    int value=0;
                    Pieces.Piece backup = board[to.X,to.Y];

                    refreshPlayerPieces();

                    if (board[from.X, from.Y].PieceType == PiecesType.PAWN)
                    {
                        if(from.Y==1&& board[from.X, from.Y].colour == PlayerColour.WHITE)
                        {
                            if (to.Y == 3)
                            {
                                board[from.X, 2] = new Pieces.PawnPointer(PlayerColour.WHITE, to.X, to.Y, this);
                            }
                        }
                        if (from.Y == 6 && board[from.X, from.Y].colour == PlayerColour.BLACK)
                        {
                            if (to.Y == 4)
                            {
                                board[from.X, 5] = new Pieces.PawnPointer(PlayerColour.BLACK, to.X, to.Y, this);
                            }
                        }

                        if(board[to.X,to.Y].PieceType == PiecesType.NONE && board[to.X, to.Y].colour != PlayerColour.NONE)
                        {
                            Point pawnToKillPos = board[to.X, to.Y].position;
                            board[pawnToKillPos.X, pawnToKillPos.Y] = new Pieces.EmptyPiece();
                        }

                    }


                    if (board[from.X, from.Y].PieceType == PiecesType.ROOK)
                    {
                        if (from.X == 0)
                        {
                            if(from.Y == 7 && board[from.X, from.Y].colour == PlayerColour.BLACK)
                            {
                                SETTINGS.castle[0, (int)board[from.X, from.Y].colour] = false;
                            }
                            if (from.Y == 0 && board[from.X, from.Y].colour == PlayerColour.WHITE)
                            {
                                SETTINGS.castle[0, (int)board[from.X, from.Y].colour] = false;
                            }

                        }
                        if (from.X == 7)
                        {
                            if (from.Y == 7 && board[from.X, from.Y].colour == PlayerColour.BLACK)
                            {
                                SETTINGS.castle[1, (int)board[from.X, from.Y].colour] = false;
                            }
                            if (from.Y == 0 && board[from.X, from.Y].colour == PlayerColour.WHITE)
                            {
                                SETTINGS.castle[1, (int)board[from.X, from.Y].colour] = false;
                            }
                        }
                    }

                    if (board[from.X, from.Y].PieceType == PiecesType.KING)
                    {
                        if (from.X - 2 == to.X)
                        {
                            board[to.X + 1, to.Y] = board[0, from.Y];
                            board[to.X + 1, to.Y].position = new Point(to.X + 1, to.Y);
                            board[0, from.Y] = new Pieces.EmptyPiece();
                        }
                        if (from.X + 2 == to.X)
                        {
                            board[to.X - 1, to.Y] = board[7, from.Y];
                            board[to.X - 1, to.Y].position = new Point(to.X - 1, to.Y);
                            board[7, from.Y] = new Pieces.EmptyPiece();
                        }
                        SETTINGS.castle[0, (int)board[from.X, from.Y].colour] = false;
                        SETTINGS.castle[1, (int)board[from.X, from.Y].colour] = false;
                    }
                    //przenies figure
                    board[to.X, to.Y] = board[from.X, from.Y];
                    board[from.X, from.Y] = new Pieces.EmptyPiece();
                    board[to.X, to.Y].position = to;


                    //sprawdz czy krol jest atakowany po tym ruchu
                    refreshPlayerPieces();
                    int check = kingUnderAttack();
                    if (check == -1)
                    {
                        board[from.X, from.Y] = board[to.X, to.Y];
                        board[from.X, from.Y].position = from;
                        board[to.X, to.Y] = backup;
                        refreshPlayerPieces();
                        return -2;
                    }
                    if (check == 1)
                    {
                        value += 100;
                    }
                    if (check == 2)
                    {
                        value += 200;
                    }


                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (board[i, j].PieceType == PiecesType.NONE)
                            {
                                if(board[i, j].colour != turn)
                                {
                                    board[i,j]=new Pieces.EmptyPiece();
                                }
                            }
                        }
                    }
                    refreshPlayerPieces();

                   

                    //zmien ture
                    if (turn == PlayerColour.WHITE)
                    {
                        turn = PlayerColour.BLACK;
                    }
                    else
                    {
                        turn = PlayerColour.WHITE;
                    }


                    //zaaktualizuj figury graczom
                    refreshPlayerPieces();

                    return value;
                    
                }
            }
            return -1;
        }

       

        public void setBoard(Pieces.Piece[,] chessBoard)
        {
            board = chessBoard;
        }

        public Board Clone()
        {
            Player white=new Gamer();
            Player black=new Gamer();
            BoardSettings setings=new BoardSettings(SETTINGS.castle);
            Board clone = new Board(white, black, setings); 

            Pieces.Piece[,] clonedboard = new Pieces.Piece[8, 8];
            Pieces.Piece cp=new Pieces.EmptyPiece();
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                   Pieces.Piece currp=board[i,j];
                    if (currp.colour == PlayerColour.NONE)
                    {
                        cp=new Pieces.EmptyPiece();
                    }
                    else if(currp.colour != PlayerColour.NONE && currp.PieceType == PiecesType.NONE)
                    {
                        cp = new Pieces.PawnPointer(currp.colour, currp.position.X, currp.position.Y,clone);
                    }
                    else if (currp.PieceType == PiecesType.KING)
                    {
                        cp = new Pieces.King(currp.colour,currp.position.X,currp.position.Y,clone);
                    }
                    else if (currp.PieceType == PiecesType.PAWN)
                    {
                        cp = new Pieces.Pawn(currp.colour, currp.position.X, currp.position.Y, clone);
                    }
                    else if (currp.PieceType == PiecesType.QUEEN)
                    {
                        cp = new Pieces.Queen(currp.colour, currp.position.X, currp.position.Y, clone);
                    }
                    else if (currp.PieceType == PiecesType.ROOK)
                    {
                        cp = new Pieces.Rook(currp.colour, currp.position.X, currp.position.Y, clone);
                    }
                    else if (currp.PieceType == PiecesType.KNIGHT)
                    {
                        cp = new Pieces.Knight(currp.colour, currp.position.X, currp.position.Y, clone);
                    }
                    else if (currp.PieceType == PiecesType.BISHOP)
                    {
                        cp = new Pieces.Bishop(currp.colour, currp.position.X, currp.position.Y, clone);
                    }

                    clonedboard[i, j] = cp;
                }
            }

            clone.setBoard(clonedboard);
            clone.turn = turn;
            clone.refreshPlayerPieces();


            return clone;
        }
    }
}
