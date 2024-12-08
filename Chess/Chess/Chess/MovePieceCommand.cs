using System;

namespace Chess
{
    public class MovePieceCommand : ICommand
    {
        private readonly ChessBoard _board;
        private readonly int _fromRow;
        private readonly int _fromCol;
        private readonly int _toRow;
        private readonly int _toCol;
        private ChessPiece? _movedPiece;
        private ChessPiece? _capturedPiece;

        public MovePieceCommand(ChessBoard board, int fromRow, int fromCol, int toRow, int toCol)
        {
            _board = board;
            _fromRow = fromRow;
            _fromCol = fromCol;
            _toRow = toRow;
            _toCol = toCol;
        }

        public void Execute()
        {
            _movedPiece = _board.GetPiece(_fromRow, _fromCol);
            _capturedPiece = _board.GetPiece(_toRow, _toCol);
            _board.MovePiece(_fromRow, _fromCol, _toRow, _toCol);
        }

        public void Undo()
        {
            if (_movedPiece != null)
            {
                _board.SetPiece(_fromRow, _fromCol, _movedPiece);
                if (_capturedPiece != null)
                {
                    _board.SetPiece(_toRow, _toCol, _capturedPiece);
                }
                else
                {
                    _board.RemovePiece(_toRow, _toCol);
                }
            }
        }
    }
}
