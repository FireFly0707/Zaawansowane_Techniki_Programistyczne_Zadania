using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess
{
    // Reprezentuje szachownicę, przechowując stan wszystkich pól i figur.
    // Umożliwia ustawianie, usuwanie i przesuwanie figur na planszy.
    public class ChessBoard
    {
        private readonly ChessPiece[,] board;

        public ChessBoard()
        {
            board = new ChessPiece[8, 8];
        }

        // Zwraca figurę na podanym polu.
        public ChessPiece GetPiece(int row, int col)
        {
            return board[row, col];
        }

        // Ustawia figurę na podanym polu.
        public void SetPiece(int row, int col, ChessPiece piece)
        {
            board[row, col] = piece;
        }

        // Usuwa figurę z podanego pola.
        public void RemovePiece(int row, int col)
        {
            board[row, col] = null;
        }

        // Przesuwa figurę z jednego pola na drugie.
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Pobierz figurę z pola startowego
            ChessPiece piece = GetPiece(fromRow, fromCol);
            if (piece == null)
            {
                return false; // Brak figury do przesunięcia (pole startowe było puste)
            }

            // Usunięcie figury z pola startowego
            RemovePiece(fromRow, fromCol);

            // Ustawienie figury na polu docelowym
            SetPiece(toRow, toCol, piece);
            return true;
        }

        // Inicjalizuje szachownicę na podstawie tekstowego opisu figur, np. "Pa2 Rb1 kf8".
        public void InitializeFromString(string notation)
        {
            // Wyczyszczenie planszy
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    board[row, col] = null;
                }
            }

            // Rozdziel tekst na figury (używając białych znaków jako separatorów)
            string[] pieces = Regex.Split(notation.Trim(), "\\s+");
            foreach (string pieceNotation in pieces)
            {
                // Walidacja formatu opisu figury (np. 'Pa2')
                if (pieceNotation.Length != 3)
                {
                    throw new ArgumentException($"Nieprawidłowy format notacji: {pieceNotation}");
                }

                // Rozbicie tekstu na kod figury, kolumnę i wiersz
                char code = pieceNotation[0];
                char colChar = pieceNotation[1];
                char rowChar = pieceNotation[2];

                // Konwersja kolumny i wiersza na indeksy tablicy
                int col = colChar - 'a';          // 'a' → 0, 'b' → 1, ...
                int row = 8 - (rowChar - '0');    // '8' → 0, '7' → 1, ...

                // Utworzenie obiektu figury na podstawie kodu
                ChessPiece piece = ChessPiece.FromCode(code);

                // Ustawienie figury na planszy
                board[row, col] = piece;
            }
        }

        // Zwraca planszę jako tekst - używane do wyświetlenia aktualnego stanu szachownicy w konsoli.
        public override string ToString()
        {
            var sb = new StringBuilder();

            // Wiersz z literami kolumn
            sb.AppendLine("  a b c d e f g h");

            for (int row = 0; row < 8; row++)
            {
                // Numer wiersza na początku
                sb.Append(8 - row + " ");

                for (int col = 0; col < 8; col++)
                {
                    // Pobierz figurę z planszy i wypisz jej kod
                    ChessPiece piece = board[row, col];
                    sb.Append(piece != null ? piece.GetCode() + " " : ". ");
                }

                // Przejście do nowej linii po każdej iteracji wiersza
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
