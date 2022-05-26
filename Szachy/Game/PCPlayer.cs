﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
    class moves
    {
        public moves()
        {
           movements=new List<PieceMovement>();
        }
        public List<PieceMovement> movements;
    }

    public class PCPlayer : Player
    {
        public static int gameEndedValue = 8888;
        public static int searchDepth = 3;
        public PCPlayer()
        {
            pieces = new List<Pieces.Piece>();
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
                        if (value == -2)
                        {
                            continue;
                        }
                        else
                        {
                            canMove = true;
                        }
                        if (value >= 200)
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
                        if (value == -2)
                        {
                            continue;
                        }
                        else
                        {
                            canMove = true;
                        }
                        if (value >= 200)
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
                return best;
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
   
        public void makeMove(Board board,PlayerColour color)
        {
            var  result= minmax(board, color, searchDepth,0, -99999, 99999);

               Console.WriteLine(result.value+","+result.moves+":"+result.from.X+"/"+result.from.Y+"->" + result.where.X + "/" + result.where.Y);
            board.moved(result.from, result.where);
            /*

            Console.WriteLine("zaczynam : " + depth);
            bool myPiece = true;
            Board b = board.PCClone();
            if (b.turn != color)
            {
                myPiece = false;
            }
            PieceMovement best = new PieceMovement()
            {
                value = -1000,
                from = new Point(9, 9),
                where = new Point(9, 9),
            };


            if (depth == 0 && !myPiece)
            {
                if (!b.players[(int)b.turn].pieces.Any())
                {
                    if (myPiece)
                    {
                        return -1000;
                    }
                    else
                    {
                        return 1000;
                    }
                }
                best.value = 1000;
                foreach (Piece piece in b.players[(int)b.turn].pieces)
                {
                    foreach (Point point in piece.GetPossibleMoves())
                    {
                        Board ib = board.PCClone();

                        int val = -1 * ib.moved(piece.position, point) % 100;
                        lock (locker)
                        {
                            if (val < best.value)
                            {
                                best = new PieceMovement()
                                {
                                    value = val,
                                    from = piece.position,
                                    where = point
                                };
                            }
                        }


                    }
                }
                return best.value;
            }
            else 
            {
                if(depth==0 && myPiece)
                {
                    depth = 1;
                }
                if (!myPiece)
                {
                    best.value = 1000;
                }
                foreach(Piece piece in b.players[(int)b.turn].pieces)
                {
                    foreach(Point point in piece.GetPossibleMoves())
                    {
                        Board ib=board.PCClone();
                        int val = 0;
                        if (myPiece)
                        {
                            val = ib.moved(piece.position, point) % 100+ib.players[(int)ib.turn].makeMove(ib,depth-1,color);
                            if (val > best.value)
                            {
                                best = new PieceMovement()
                                {
                                    value = val,
                                    from = piece.position,
                                    where = point
                                };
                            }
                        }
                        else
                        {
                            val = -1*(ib.moved(piece.position, point) % 100) + ib.players[(int)ib.turn].makeMove(ib, depth - 1, color);
                            if(val< best.value)
                            {
                                best = new PieceMovement()
                                {
                                    value = val,
                                    from = piece.position,
                                    where = point
                                };
                            }
                        }
                    }
                }
                board.moved(best.from,best.where);
                Console.WriteLine("koniec=" + best.value);
                return best.value;
            }
            */
        }
    }
}
