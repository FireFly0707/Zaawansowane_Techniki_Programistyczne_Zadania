using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    // Reprezentuje pojedynczą figurę szachową, w tym jej typ (np. pion, wieża)
    // oraz kolor (biały lub czarny).
    public class ChessPiece
    {
        public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }
        public enum PieceColor { White, Black }

        public PieceType Type { get; }
        public PieceColor Color { get; }

        public ChessPiece(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }

        // Zwraca kod literowy figury, używany do wyświetlania jej na planszy.
        public char GetCode()
        {
            // Dopasowanie litery do typu figury
            char baseCode = Type switch
            {
                PieceType.Pawn => 'P',
                PieceType.Rook => 'R',
                PieceType.Knight => 'N',
                PieceType.Bishop => 'B',
                PieceType.Queen => 'Q',
                PieceType.King => 'K',
                _ => '?'
            };

            // Zwrócenie wielkiej litery dla białych figur, małej dla czarnych
            return Color == PieceColor.White ? baseCode : char.ToLower(baseCode);
        }

        // Tworzy obiekt figury szachowej na podstawie kodu literowego.
        public static ChessPiece FromCode(char code)
        {
            // Określenie koloru figury na podstawie wielkości litery
            var color = char.IsUpper(code) ? PieceColor.White : PieceColor.Black;

            // Dopasowanie typu figury do litery
            var type = char.ToUpper(code) switch
            {
                'P' => PieceType.Pawn,
                'R' => PieceType.Rook,
                'N' => PieceType.Knight,
                'B' => PieceType.Bishop,
                'Q' => PieceType.Queen,
                'K' => PieceType.King,
                _ => throw new ArgumentException($"Nieznany kod figury: {code}")
            };

            // Zwrócenie nowej figury
            return new ChessPiece(type, color);
        }
    }

}
