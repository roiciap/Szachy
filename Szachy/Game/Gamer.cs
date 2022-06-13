using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szachy.Pieces;

namespace Szachy.Game
{
    public class Gamer : Player
    {
        public bool realPerson { get; private set; }
        public List<Pieces.Piece> pieces { get; set; }
        public Gamer()
        {
            realPerson = true;
            pieces = new List<Pieces.Piece>();
        }

        public List<Point> getAttackingPoints()
        {
            List<Point> attackingPoints = new List<Point>();
            foreach(Pieces.Piece piece in pieces)
            {
                foreach (Point point in piece.GetPossibleMoves())
                {
                    if (!attackingPoints.Contains(point))
                    {
                        attackingPoints.Add(new Point(point.X, point.Y));
                    }
                }
            }
            return attackingPoints;
        }

        public MoveInfo makeMove(Board board, PlayerColour color, Form1 form)
        {
            return new MoveInfo();
        }

        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            List<MovementAndValue> movements = new List<MovementAndValue>();
            foreach(Piece piece in pieces)
            {
                movements.AddRange(piece.GetAllMovementsWithValues());
            }
           
            return movements;
        }
    }
}
