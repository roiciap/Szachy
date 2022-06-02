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

        public static int[] KnightPos={-50,-40,-30,-30,-30,-30,-40,-50,
                                -40,-20,  0,  0,  0,  0,-20,-40,
                                -30,  0, 10, 15, 15, 10,  0,-30,
                                -30,  5, 15, 20, 20, 15,  5,-30,
                                -30,  0, 15, 20, 20, 15,  0,-30,
                                -30,  5, 10, 15, 15, 10,  5,-30,
                                -40,-20,  0,  5,  5,  0,-20,-40,
                                -50,-40,-30,-30,-30,-30,-40,-50};
        public static int[] PawnPos = { 0,  0,  0,  0,  0,  0,  0,  0,
                               40, 40, 40, 40, 40, 40, 40, 40,
                                10, 10, 20, 30, 30, 20, 10, 10,
                                 5,  5, 10, 25, 25, 10,  5,  5,
                                 0,  0,  0, 20, 20,  0,  0,  0,
                                 5, -5,-10,  0,  0,-10, -5,  5,
                                 5, 10, 10,-20,-20, 10, 10,  5,
                                 0,  0,  0,  0,  0,  0,  0,  0};
        public static int[] BishopPos ={-20,-10,-10,-10,-10,-10,-10,-20,
                                -10,  0,  0,  0,  0,  0,  0,-10,
                                -10,  0,  5, 10, 10,  5,  0,-10,
                                -10,  5,  5, 10, 10,  5,  5,-10,
                                -10,  0, 10, 10, 10, 10,  0,-10,
                                -10, 10, 10, 10, 10, 10, 10,-10,
                                -10,  5,  0,  0,  0,  0,  5,-10,
                                -20,-10,-10,-10,-10,-10,-10,-20};
        public static int[] RookPos={  0,  0,  0,  0,  0,  0,  0,  0,
                                  5, 10, 10, 10, 10, 10, 10,  5,
                                 -5,  0,  0,  0,  0,  0,  0, -5,
                                 -5,  0,  0,  0,  0,  0,  0, -5,
                                 -5,  0,  0,  0,  0,  0,  0, -5,
                                 -5,  0,  0,  0,  0,  0,  0, -5,
                                 -5,  0,  0,  0,  0,  0,  0, -5,
                                  0,  0,  0,  5,  5,  0,  0,  0 };
        public static int[] QueenPos ={-20,-10,-10, -5, -5,-10,-10,-20,
                                -10,  0,  0,  0,  0,  0,  0,-10,
                                -10,  0,  5,  5,  5,  5,  0,-10,
                                 -5,  0,  5,  5,  5,  5,  0, -5,
                                  0,  0,  5,  5,  5,  5,  0, -5,
                                -10,  5,  5,  5,  5,  5,  0,-10,
                                -10,  0,  5,  0,  0,  0,  0,-10,
                                -20,-10,-10, -5, -5,-10,-10,-20};
        public static int[] KingPos ={-30,-40,-40,-50,-50,-40,-40,-30,
                                -30,-40,-40,-50,-50,-40,-40,-30,
                                -30,-40,-40,-50,-50,-40,-40,-30,
                                -30,-40,-40,-50,-50,-40,-40,-30,
                                -20,-30,-30,-40,-40,-30,-30,-20,
                                -10,-20,-20,-20,-20,-20,-20,-10,
                                 20, 20,  0,  0,  0,  0, 20, 20,
                                 20, 30, 10,  0,  0, 10, 30, 20};
        public Board(Player White , Player Black)
        {
            board = new Pieces.Piece[8, 8];

            turn = PlayerColour.WHITE;
            SETTINGS= new BoardSettings();
            GenerateStartingBoard();
            players =new Player[2];
            players[1] = White;
            players[0] = Black;
            refreshPlayerPieces();

        }
        public Board(Player White, Player Black, BoardSettings settings)
        {
            board = new Pieces.Piece[8, 8];
            turn = PlayerColour.WHITE;
            SETTINGS = settings;
            GenerateStartingBoard();
            players = new Player[2];
            players[1] = White;
            players[0] = Black;
            refreshPlayerPieces();
        }

        public Board(Player White, Player Black, BoardSettings settings, Pieces.Piece[,] chessBoard)
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


        public static int getPositionValue(PiecesType pieceType,Point position,PlayerColour color)
        {
            int x = position.X, y = position.Y;
            if (color == PlayerColour.WHITE)
            {
                y = 7 - y;
                x = 7 - x;
            }
            if (pieceType == PiecesType.PAWN)
            {
                return PawnPos[y * 8 + x]/5;
            }
            if (pieceType == PiecesType.KNIGHT)
            {
                return KnightPos[y * 8 + x] / 5;
            }
            if (pieceType == PiecesType.BISHOP)
            {
                return BishopPos[y * 8 + x] / 5;
            }
            if(pieceType== PiecesType.ROOK)
            {

                return RookPos[y * 8 + x] / 5;
            }
            if(pieceType == PiecesType.QUEEN) 
            {

                return QueenPos[y * 8 + x] / 5;
            }
            if (pieceType == PiecesType.KING)
            {
                return KingPos[y * 8 + x] / 5;
            }
            return 0;

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
             
              // board[4, 0] = new Pieces.King(PlayerColour.WHITE, 4, 0, this);
               //board[4, 7] = new Pieces.King(PlayerColour.BLACK, 4, 7, this);
               board[3, 0] = new Pieces.Queen(PlayerColour.WHITE, 3, 0, this);
               board[3, 7] = new Pieces.Queen(PlayerColour.BLACK, 3, 7, this);
               board[0, 0] = new Pieces.Rook(PlayerColour.WHITE, 0, 0, this);
              // board[7,0]=new Pieces.Rook(PlayerColour.WHITE,7,0, this);
               board[0, 7] = new Pieces.Rook(PlayerColour.BLACK, 0, 7, this);
               //board[7, 7] = new Pieces.Rook(PlayerColour.BLACK, 7, 7, this);
              board[1, 0] = new Pieces.Knight(PlayerColour.WHITE, 1, 0, this);
              // board[6, 0] = new Pieces.Knight(PlayerColour.WHITE, 6, 0, this);
               board[1, 7] = new Pieces.Knight(PlayerColour.BLACK, 1, 7, this);
               //board[6, 7] = new Pieces.Knight(PlayerColour.BLACK, 6, 7, this);
             board[2, 0] = new Pieces.Bishop(PlayerColour.WHITE, 2, 0, this);
             // board[5, 0] = new Pieces.Bishop(PlayerColour.WHITE, 5, 0, this);
              board[2, 7] = new Pieces.Bishop(PlayerColour.BLACK, 2, 7, this);
             // board[5, 7] = new Pieces.Bishop(PlayerColour.BLACK, 5, 7, this);

            /*

                       */
            board[6, 0] = new Pieces.King(PlayerColour.WHITE, 6, 0, this);
            board[6, 7] = new Pieces.King(PlayerColour.BLACK, 6, 7, this);
            board[5, 0] = new Pieces.Rook(PlayerColour.WHITE, 5, 0, this);
            board[5, 7] = new Pieces.Rook(PlayerColour.BLACK, 5, 7, this);
            board[2, 3] = new Pieces.Bishop(PlayerColour.WHITE, 2, 3, this);
            board[2, 4] = new Pieces.Bishop(PlayerColour.BLACK, 2, 4, this);
            board[3, 1] = new Pieces.EmptyPiece();
            board[4, 1] = new Pieces.EmptyPiece();
            board[3, 6] = new Pieces.EmptyPiece();
            board[4, 6] = new Pieces.EmptyPiece();
            board[3, 2] = new Pieces.Pawn(PlayerColour.WHITE, 3, 2, this);
            board[4, 3] = new Pieces.Pawn(PlayerColour.WHITE, 4, 3, this);
            board[3, 5] = new Pieces.Pawn(PlayerColour.BLACK, 3, 5, this);
            board[4, 4] = new Pieces.Pawn(PlayerColour.BLACK, 4, 4, this);
            board[5, 2] = new Pieces.Knight(PlayerColour.WHITE, 5, 2, this);
            board[5, 5] = new Pieces.Knight(PlayerColour.BLACK, 5, 5, this);

            SETTINGS.castle[0, 0] = false;
            SETTINGS.castle[0, 1] = false;
            SETTINGS.castle[1, 0] = false;
            SETTINGS.castle[1, 1] = false;
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
            for(int i = 0; i < whitePieces.Count; i++)
            {
                for(int j = 0; j < whitePieces.Count - (i+1); j++)
                {
                    if (whitePieces[j].PieceType < whitePieces[j + 1].PieceType)
                    {
                        var tmp = whitePieces[j];
                        whitePieces[j]=whitePieces[j + 1];
                        whitePieces[j + 1] = tmp;
                    }
                }
            }
            for (int i = 0; i < blackPieces.Count; i++)
            {
                for (int j = 0; j < blackPieces.Count - (i + 1); j++)
                {
                    if (blackPieces[j].PieceType < blackPieces[j + 1].PieceType)
                    {
                        var tmp = blackPieces[j];
                        blackPieces[j] = blackPieces[j + 1];
                        blackPieces[j + 1] = tmp;
                    }
                }
            }
            players[1].pieces = whitePieces;
            players[0].pieces = blackPieces;

        }


        public int kingUnderAttack()
        {
            Board b = Clone();


            foreach (Pieces.Piece piece in b.players[(int)turn].pieces)
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

                    if (b.players[((int)turn)].getAttackingPoints().Contains(piece.position))
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
                        return -100;
                    }
                    int value=board[to.X,to.Y].Value*10;
                    int initialPosValue= Board.getPositionValue(board[from.X, from.Y].PieceType, from, board[from.X, from.Y].colour);
                    if (value == 1) {//jezeli atakujemy miejsce gdzie mozna wykonać bicie w przelocie a figura to nie pion
                        if (board[to.X, to.Y].PieceType == PiecesType.NONE)
                        {
                            if (board[from.X, from.Y].PieceType != PiecesType.PAWN)
                            {
                                value = 0;
                            }
                        }
                    }
                    Pieces.Piece backup = board[to.X,to.Y];


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
                        return -200;
                    }
                    else if (to.Y == 7 || to.Y == 0)
                    {
                        if (board[to.X,to.Y].PieceType==PiecesType.PAWN)
                        {
                            board[to.X, to.Y] = new Pieces.Queen(board[to.X, to.Y].colour, to.X, to.Y, this);
                            check=kingUnderAttack();
                            value += 80;
                        }
                    }
                    value += Board.getPositionValue(board[to.X, to.Y].PieceType, to, board[to.X, to.Y].colour)-initialPosValue;
                    if (check == 1)
                    {
                        value += 1000;
                    }
                    else if (check == 2)
                    {
                        value += 2000;
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

        public Board PCClone()
        {
            Board clone = Clone();
            clone.players[0] = new PCPlayer();
            clone.players[1] = new PCPlayer();
            clone.refreshPlayerPieces();
            return clone;
        }

        public Board Clone()
        {
            Player white=new Gamer();
            Player black=new Gamer();
            if(!players[1].realPerson)
              white=new Gamer();
            if(!players[0].realPerson)
             black=new Gamer();
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
