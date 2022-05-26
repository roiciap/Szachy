using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Szachy.Game;


namespace Szachy.Pieces
{
    internal class Queen : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }
        public int Value { get => 9; }



        public Queen(PlayerColour ownerColour, int x, int y, Board chessBoard)
        {
            this.colour = ownerColour;
            this.board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.QUEEN;

        }
        public bool checkMove(int x, int y)
        {
            Piece dir = board.getPiece(x, y);
            if (dir.colour == colour && dir.PieceType != PiecesType.NONE) return false;
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if(x==position.X && y==position.Y) return false;          
            if(x==position.X || y==position.Y)
            {
                int it = 1;
                if(x==position.X)
                {
                    if (position.Y - y > 0)
                    {
                        it = -1;
                    }

                    for(int i = position.Y+it; i != y; i += it)
                    {
                        if(board.getPiece(x, i).PieceType != PiecesType.NONE)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    if (position.X - x > 0)
                    {
                        it = -1;
                    }
                    for(int i = position.X+it; i != x; i += it)
                    {
                        if (board.getPiece(i, y).PieceType != PiecesType.NONE)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                
            }
            float movement = ((float)position.X - x) / ((float)position.Y - y);
            if (movement == 1 || movement == -1)
            {
                int movementX = position.X - x;
                int movementY = position.Y - y;
                int itX = 1, itY = 1;
                if (movementX > 0)
                {
                    itX = -1;
                }
                if (movementY > 0)
                {
                    itY = -1;
                }


                for (int i = 1; position.X + i * itX != x; i++)
                {
                    if (board.getPiece(position.X + i * itX, position.Y + i * itY).PieceType != PiecesType.NONE)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();
            int x = position.X;
            int y = position.Y;
            for (int i = 0; i < 8; i++)
            {
                if (checkMove(i, y - (x - i)))
                {
                    possibleMoves.Add(new Point(i, position.Y - (position.X - i)));
                }
                if (checkMove(i, y + (x - i)))
                {
                    possibleMoves.Add(new Point(i, position.Y + (position.X - i)));
                }
                if ( checkMove(i, position.Y)) possibleMoves.Add(new Point(i, position.Y));
                if ( checkMove(position.X, i)) possibleMoves.Add(new Point(position.X, i));
            }

           /* for (int j = 0; j < 8; j++)
            {

                for (int i = j; i < possibleMoves.Count; i++)
                {
                    if (board.getPiece(possibleMoves[j].X, possibleMoves[j].Y).Value < board.getPiece(possibleMoves[i].X, possibleMoves[i].Y).Value)
                    {
                        var tmp = possibleMoves[j];
                        possibleMoves[j] = possibleMoves[i];
                        possibleMoves[i] = tmp;
                    }
                }
            }*/
            return possibleMoves;
        }
        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            var possibleMoves = GetPossibleMoves();
            var result = new List<MovementAndValue>();
            foreach (var move in possibleMoves)
            {
                result.Add(new MovementAndValue()
                {
                    from = position,
                    to = move,
                    value = board.getPiece(move.X, move.Y).Value
                });
            }
            return result;
        }
        public int move(Point where)
        {
            if (checkMove(where.X, where.Y))
            {
                int result = 0;
                result = board.moved(position, where);
                return result;
            }
            return -1;
        }
    }
}
