using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Szachy.Pieces
{
    public class King : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }



        public King(PlayerColour owner,
            int x,
            int y,
            Game.Board chessBoard)
        {
            this.colour = owner;
            this.board = chessBoard;
            this.position = new Point(x, y);
            this.PieceType = PiecesType.KING;

        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    if(checkMove(position.X+i, position.Y+j))
                    {
                        if (!(i == 0 && j == 0))
                        {
                            possibleMoves.Add(new Point(position.X+i, position.Y+j));
                        }

                    }
                }
            }

          //  if(checkMove(position.X-2,position.Y)) possibleMoves.Add(new Point(position.X-2, position.Y));
          // if (checkMove(position.X + 2, position.Y)) possibleMoves.Add(new Point(position.X+2, position.Y));



            return possibleMoves;
        }

        public bool checkMove(int x, int y)
        {
            Piece dir = board.getPiece(x, y);
            if (dir.colour == colour && dir.PieceType != PiecesType.NONE) return false;
            if (x == position.X && y == position.Y) return false;
            if (x>=0 && y>=0 && x<8 && y < 8)
            {
                if (position.X -x < 2 && position.X-x >-2 && position.Y -y<2 && position.Y -y > -2)
                {
                        return true;
                }
                List<Point> em=board.players[((int)colour+1)%2].getAttackingPoints();
                if (em.Contains(position)) return false;
                if (position.Y == y)
                {
                    if(position.X+2==x && board.SETTINGS.castle[1, (int)colour])
                    {
                        if (em.Contains(new Point(7, y))) return false;
                        for(int i = position.X + 1; i < 7; i++)
                        {
                            if (board.getPiece(i, y).colour != PlayerColour.NONE || em.Contains(new Point(i, y)))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    if(position.X-2==x && board.SETTINGS.castle[0, (int)colour])
                    {
                        if (em.Contains(new Point(7, y))) return false;
                        for (int i = position.X - 1; i > 0; i--)
                        {
                            if (board.getPiece(i, y).colour != PlayerColour.NONE || em.Contains(new Point(i,y)))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }

            return false;

        }

        public int move(Point where)
        {
            if (checkMove(where.X, where.Y))
            {
                int result = 0;
                var thread = new Thread(() =>
                {
                    result = board.moved(position, where);
                });
                thread.Start();
                thread.Join();
                return result;

            }
            return -1;
        }
    }
}
