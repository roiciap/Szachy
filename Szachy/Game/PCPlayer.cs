using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Szachy.Pieces;

namespace Szachy.Game
{
    public class PieceMovement
    {
        public int value { get; set; }
        public Point where { get; set; }
        public Point from { get; set; }
        public int moves { get; set; }
    }
   

    public class PCPlayer : Player
    {
        private int movesMade { get; set; }
        public static int gameEndedValue = 8888;
        public static int searchDepth = 3;
        public PCPlayer()
        {
            pieces = new List<Pieces.Piece>();
            movesMade = 0;
        }

        public bool realPerson => false;
        public static object locker=new object();
        public static int numberOfMoves = 0;
        public List<Pieces.Piece> pieces { get; set; }

        public List<Point> getAttackingPoints()
        {
            List<Point> attackingPoints = new List<Point>();
            foreach (Pieces.Piece piece in pieces)
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
        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            List<MovementAndValue> movements = new List<MovementAndValue>();
            foreach (Piece piece in pieces)
            {
                movements.AddRange(piece.GetAllMovementsWithValues());
            }
            return movements;
        }
        public PieceMovement minmax(Board board,PlayerColour color,int depth,int prevValue,int max,int min)
        {

           // Console.WriteLine(numberOfMoves);
            bool canMove = false;
            var playerPieces = board.players[(int)board.turn].pieces;
            PieceMovement best = new PieceMovement
            {
                value = 9999,
                moves = depth
            };
            bool myTurn = false;
            if (board.turn == color)
            {
                myTurn = true;
                best.value = -9999;
            }
            if (!playerPieces.Any())
            {
                if (myTurn)
                {
                    return new PieceMovement()
                    {
                        value = -gameEndedValue,
                        moves = depth-1
                    };
                }
                else
                {

                    return new PieceMovement()
                    {
                        value = gameEndedValue,
                        moves = depth - 1
                    };
                }
            }
            if (depth == 0)
            {
                foreach (var piece in playerPieces)
                {
                    var moves = piece.GetPossibleMoves();
                    foreach (var point in piece.GetPossibleMoves())
                    {
                        numberOfMoves++;
                        int totalValue;
                        var clone = board.PCClone();
                        int value = clone.moved(piece.position, point);
                        if (value ==-200)
                        {
                            continue;
                        }
                        else
                        {
                            canMove = true;
                        }
                        if (value >= 1900)
                        {
                            if (myTurn)
                            {
                                return new PieceMovement()
                                {
                                    value = gameEndedValue,
                                    moves = depth,
                                    from = piece.position,
                                    where = point
                                };
                            }
                            else
                            {

                                return new PieceMovement()
                                {
                                    value = -gameEndedValue,
                                    moves = depth,
                                    from = piece.position,
                                    where = point
                                };
                            }
                        }

                        if (myTurn)
                         totalValue= value%100 + prevValue;
                        else
                            totalValue= prevValue - value%100;
                        if(myTurn && totalValue > best.value) 
                        { 
                            best.value = totalValue;
                            best.from = piece.position;
                            best.where = point;
                            best.moves = searchDepth;
                            if (best.value > max)
                            {
                                max = best.value;
                                if (min <= max)
                                {
                                   
                                    return best;
                                }
                            }
                        }
                        else if (!myTurn && totalValue < best.value)
                        {
                            best.value = totalValue;
                            best.from = piece.position;
                            best.where = point;
                            best.moves = searchDepth;
                            if (best.value < min)
                            {
                                min = best.value;
                                if (min <= max)
                                {
                                   
                                    return best;
                                }
                            }
                        }
                        if(best.value==9999 || best.value == -9999)
                        {
                            Console.WriteLine("lipton "+playerPieces.Count+": "+piece.position.X+"/"+piece.position.Y+" - "+point.X+"/"+point.Y+" = "+piece.PieceType+"=>"+totalValue);
                        }
                    }
                }

               

            }
            else
            {
                foreach (var piece in playerPieces)
                {
                    var moves =piece.GetPossibleMoves();
                    foreach (var point in moves)
                    {
                        

                        PieceMovement totalValue;
                        var clone = board.PCClone();
                        int value = clone.moved(piece.position, point);
                        if (value == -200)
                        {
                            continue;
                        }
                        else
                        {
                            canMove = true;
                        }
                        if (value >= 1900)
                        {
                            if (myTurn)
                            {
                                return new PieceMovement()
                                {
                                    value = gameEndedValue,
                                    moves = depth ,
                                    from = piece.position,
                                    where=point
                                };
                            }
                            else
                            {

                                return new PieceMovement()
                                {
                                    value = -gameEndedValue,
                                    moves = depth ,
                                    from = piece.position,
                                    where = point
                                };
                            }
                        }

                        if (myTurn)
                            totalValue = minmax(clone, color, depth - 1, value%100 + prevValue, max, min);
                        else
                            totalValue = minmax(clone, color, depth - 1, prevValue - value%100, max, min);



                        if (totalValue.value == best.value)
                        {
                            if (best.moves < totalValue.moves)
                            {
                                best.moves=totalValue.moves;
                                best.from= piece.position;
                                best.where = point;
                                best.value = totalValue.value;
                            }
                        }
                        
                        if (myTurn)
                        {
                            if(totalValue.value > best.value)
                            {
                                best.value = totalValue.value;
                                best.from = piece.position;
                                best.where = point;
                                best.moves = totalValue.moves;
                                if (best.value > max)
                                {
                                    max = best.value;
                                    if (min <= max)
                                    {
                                       
                                        return best;
                                    }
                                }
                            }
                         
                        }
                        else
                        {
                            if (totalValue.value < best.value)
                            {
                                best.value = totalValue.value;
                                best.from = piece.position;
                                best.where = point;
                                best.moves = totalValue.moves;
                                if (best.value < min)
                                {
                                    min = best.value;
                                    if (min <= max)
                                    {
                                       
                                        return best;
                                    }
                                }
                            }
                          
                        }
                        
                        
                    }
                }
               
            }
            if (canMove)
            {
                if (best.value == 9999 || best.value == -9999)
                {
                    Console.WriteLine("error" + board.turn);
                    Console.WriteLine(playerPieces.Count);
                    foreach (var piece in board.players[0].pieces)
                    {
                        Console.WriteLine(piece.position.X + "/" + piece.position.Y + "-" + piece.PieceType + "," + piece.colour);
                    }
                    foreach (var piece in board.players[1].pieces)
                    {
                        Console.WriteLine(piece.position.X + "/" + piece.position.Y + "-" + piece.PieceType + "," + piece.colour);
                    }
                }
                return best;//zwroc najlepszy ruch
            }
            else
            {//pat
                if (myTurn)
                {
                    return new PieceMovement()
                    {
                        value = gameEndedValue,
                        moves = depth - 1
                    };
                }
                else
                {

                    return new PieceMovement()
                    {
                        value = -gameEndedValue,
                        moves = depth - 1
                    };
                }
            }

        }
   
        public MoveInfo makeMove(Board board,PlayerColour color,Form1 form)
        {
            MoveInfo returnVal=new MoveInfo();
            Console.WriteLine(color);
            bool found = false;
            Point from = new Point(-1, -1), to = new Point(-1, -1);
            string[] files = { "a", "b", "c", "d", "e" };
            if (!form.movesHistory.Any())
            {
                Random random = new Random();
                int file=random.Next(files.Length);
                var res = File.ReadAllLines("./openings/" + files[file] + ".tsv");
                var openingPossibilites = res.Where(l => !l.Contains("Defense"));
                int openingNumber = random.Next(openingPossibilites.Count());
                string line = openingPossibilites.ElementAt(openingNumber);
                var columns=line.Split('\t');
                if (columns.Length > 2) {
                    var moves = columns[3].Split(' ');
                    string toDo = moves[0];
                    var info = toDo.ToCharArray();
                    char[] columnsChar = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

                    if (info.Length > 3)
                    {
                        for (int j = 0; j < columnsChar.Length; j++)
                        {
                            if (info[0].Equals(columnsChar[j])) from = new Point(j, (info[1] - '0') - 1);
                            if (info[2].Equals(columnsChar[j])) to = new Point(j, (info[3] - '0') - 1);
                        }
                        found = true;
                        int val = board.moved(from, to);
                        string[] columnsString = { "a", "b", "c", "d", "e", "f", "g", "h" };
                        string[] pieceString = { "K", "Q", "R", "N", "B", "" };
                        returnVal = new MoveInfo()
                        {
                            uci = toDo,
                            pgn = pieceString[(int)board.getPiece(to.X, to.Y).PieceType] + columnsString[to.X] + (to.Y + 1),
                            value = val,
                        };
                    }
                }
            }
            foreach (string file in files)
            {



                if (found) break;
                var res = File.ReadAllLines("./openings/" + file + ".tsv");

                foreach (var line in res)
                {
                    bool nextLine = false;
                    if (found) break;
                    if ((color == PlayerColour.BLACK && line.Contains("Defense")) || (color == PlayerColour.WHITE && !line.Contains("Defense")))
                    {
                        var columns = line.Split('\t');
                        if (columns.Length > 3)
                        {
                            var ucis = columns[3];
                            var moves = ucis.Split(' ');
                            if (moves.Length <= form.movesHistory.Count) continue;

                            foreach (string mh in form.movesHistory)
                            {
                                if (!moves.Contains(mh)) nextLine = true;
                            }
                            if (nextLine) continue;

                            for (int i = 0; i < form.movesHistory.Count; i++)//sprawdz czy to aktualna pozycja
                            {
                                if (!form.movesHistory.Contains(moves[i])) nextLine = true;
                            }

                            if (nextLine) continue;

                            string toDo = moves[form.movesHistory.Count];
                            var info = toDo.ToCharArray();
                            char[] columnsChar = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

                            if (info.Length > 3)
                            {
                                for (int j = 0; j < columnsChar.Length; j++)
                                {
                                    if (info[0].Equals(columnsChar[j])) from = new Point(j, (info[1] - '0') - 1);
                                    if (info[2].Equals(columnsChar[j])) to = new Point(j, (info[3] - '0') - 1);
                                }
                                found = true;
                                int val = board.moved(from, to);
                                string[] columnsString = { "a", "b", "c", "d", "e", "f", "g", "h" };
                                string[] pieceString = { "K", "Q", "R", "N", "B", "" };
                                returnVal = new MoveInfo()
                                {
                                    uci = toDo,
                                    pgn = pieceString[(int)board.getPiece(to.X, to.Y).PieceType] + columnsString[to.X] + (to.Y + 1),
                                    value = val,
                                };
                            }


                        }
                    }
                }
            }
            if (!found || returnVal.value==-200)
            {

                    var result = minmax(board, color, searchDepth, 0, -99999, 99999);

                    Console.WriteLine(result.value + "," + result.moves + ":" + result.from.X + "/" + result.from.Y + "->" + result.where.X + "/" + result.where.Y);
                    board.moved(result.from, result.where);
                    string[] columns = { "a", "b", "c", "d", "e", "f", "g", "h" };
                    string[] pieceString = { "K", "Q", "R", "N", "B", "" };
                    returnVal = new MoveInfo()
                    {
                        uci = columns[result.from.X] + (result.from.Y + 1) + columns[result.where.Y] + (result.where.X + 1),
                        pgn = pieceString[(int)board.getPiece(result.where.X, result.where.Y).PieceType] + columns[result.where.X] + (result.where.Y + 1),
                        value = result.value,
                    };
                
            }
            form.refreshBoard();
            form.AddMoveHistory(returnVal.pgn);
            form.movesHistory.Add(returnVal.uci);
          return returnVal;
        }
    }
}
