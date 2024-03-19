using System;
using UnityEngine;

namespace FactoryBasedOnEnum.Examples
{
    public class BasicExample
    {
        public void Example()
        {
            GenericFactory<ChessPieceType, IChessPiece>
                factory = new GenericFactory<ChessPieceType, IChessPiece>(false);

            IChessPiece pawn = factory.GetInstance(ChessPieceType.Pawn);
            IChessPiece rook = factory.GetInstance(ChessPieceType.Rook);
            IChessPiece knight = factory.GetInstance(ChessPieceType.Knight);
            IChessPiece bishop = factory.GetInstance(ChessPieceType.Bishop);
            IChessPiece queen = factory.GetInstance(ChessPieceType.Queen);
            IChessPiece king = factory.GetInstance(ChessPieceType.King);
        }
    }

    public enum ChessPieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    internal interface IChessPiece
    {
        bool CanMoveTo(Vector2Int position);
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.Pawn)]
    internal class Pawn : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.Rook)]
    internal class Rook : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.Knight)]
    internal class Knight : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.Bishop)]
    internal class Bishop : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.Queen)]
    internal class Queen : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }

    [EnumAssociated(typeof(ChessPieceType), ChessPieceType.King)]
    internal class King : IChessPiece
    {
        public bool CanMoveTo(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }
}