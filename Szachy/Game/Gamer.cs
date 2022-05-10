using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void makeMove()
        {
            return;
        }
    }
}
