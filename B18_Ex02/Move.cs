﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Move
     {
          private Square m_sourceSquare;
          private Square m_capturedSquare;
          private Square m_destinationSquare;

          public Move()
          {
               m_sourceSquare = new Square();
               m_destinationSquare = new Square();
          }

          public Move(ref Square i_sourceSquare,ref Square i_destinationSquare)
          {
               m_sourceSquare = i_sourceSquare;
               m_destinationSquare = i_destinationSquare;
          }

          public Move(Square i_sourceSquare, Square i_capturedSquare, Square i_destinationSquare)
          {
               m_sourceSquare = i_sourceSquare;
               m_capturedSquare = i_capturedSquare;
               m_destinationSquare = i_destinationSquare;

          }

          public Square sourceSquare
          {
               get { return m_sourceSquare; }
               set { m_sourceSquare = value; }
          }

          public Square capturedSquare
          {
               get { return m_capturedSquare; }
               set { m_capturedSquare = value; }
          }

          public Square destinationSquare
          {
               get { return m_destinationSquare; }
               set { m_destinationSquare = value; }
          }

          public Move Parse(string i_userInput)
          {
               string[] splittedInput = i_userInput.Split('>');
               Move requestedPlayerMove = new Move();
               requestedPlayerMove.sourceSquare.squarePosition = new Square.SquarePosition((int)splittedInput[0][1] - 'a', (int)splittedInput[0][0] - 'A');
               requestedPlayerMove.destinationSquare.squarePosition = new Square.SquarePosition((int)splittedInput[1][1] - 'a', (int)splittedInput[1][0] - 'A');

               return requestedPlayerMove;
          }

          public bool TryParse(string i_userInput, ref Move i_requestedMove, Team i_activeTeam)
          {
               bool canConvertInputToMove = false;
               if (UserInterface.IsLegalInputFormat(i_userInput))
               {
                    i_requestedMove = i_requestedMove.Parse(i_userInput);
                    if (i_requestedMove.IsLegalMove(ref i_requestedMove, i_activeTeam))
                    {
                         canConvertInputToMove = true;
                    }
               }

               return canConvertInputToMove;

          }

          public bool IsLegalMove(ref Move io_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalMove = false;

               foreach (Move availableMove in i_activeTeam.attackMoves)
               {
                    if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                    {
                         isLegalMove = true;
                         io_userRequestForMove = availableMove;
                    }
               }

               if (isLegalMove == false && i_activeTeam.attackMoves.Capacity == 0)
               {
                    foreach (Move availableMove in i_activeTeam.regularMoves)
                    {
                         if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                         {
                              isLegalMove = true;
                         }
                    }
               }

               return isLegalMove;
          }

          public bool IsMoveMatchToMoveInMovesList(ref Move i_userRequestForMove, Move i_availableMove)
          {
               bool isMoveMatchToMoveInMovesList = false;
               if (i_availableMove.m_destinationSquare.squarePosition.x == i_userRequestForMove.m_destinationSquare.squarePosition.x &&
                    i_availableMove.m_destinationSquare.squarePosition.y == i_userRequestForMove.m_destinationSquare.squarePosition.y &&
                    i_availableMove.m_sourceSquare.squarePosition.x == i_userRequestForMove.m_sourceSquare.squarePosition.x &&
                   i_availableMove.m_sourceSquare.squarePosition.y == i_userRequestForMove.m_sourceSquare.squarePosition.y)
               {
                    i_userRequestForMove = i_availableMove;
                    isMoveMatchToMoveInMovesList = true;
               }

               return isMoveMatchToMoveInMovesList;
          }

          public void ExecuteMove()
          {
              if (m_capturedSquare == null)
               {
                    m_destinationSquare.currentMan = m_sourceSquare.currentMan;
                    m_sourceSquare.currentMan.currentPosition = m_destinationSquare;
                    m_sourceSquare.currentMan = null;
               }

               else
               {
                    m_destinationSquare.currentMan = m_sourceSquare.currentMan;
                    m_sourceSquare.currentMan.currentPosition = m_destinationSquare;
                    m_capturedSquare.currentMan = null;
                    m_sourceSquare.currentMan = null;

               }
          }

     }
}