using System;

namespace Chess
{
    public class MoveCommand : ICommand
    {
        private readonly ChessBoard _board;
        private readonly int _fromRow, _fromCol, _toRow, _toCol;
        private ChessPiece _capturedPiece;

        public MoveCommand(ChessBoard board, int fromRow, int fromCol, int toRow, int toCol)
        {
            _board = board;
            _fromRow = fromRow;
            _fromCol = fromCol;
            _toRow = toRow;
            _toCol = toCol;
        }

        public void Execute()
        {
            _capturedPiece = _board.GetPiece(_toRow, _toCol);
            _board.MovePiece(_fromRow, _fromCol, _toRow, _toCol);
        }

        public void Undo()
        {
            _board.MovePiece(_toRow, _toCol, _fromRow, _fromCol);
            _board.SetPiece(_toRow, _toCol, _capturedPiece);
        }
    }
}
