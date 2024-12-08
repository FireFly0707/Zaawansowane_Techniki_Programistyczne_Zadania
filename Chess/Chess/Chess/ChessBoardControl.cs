using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class ChessBoardControl : Control
    {
        public ChessBoard? ChessBoard { get; set; } // Odwołanie do obiektu ChessBoard
        public CommandManager? CommandManager { get; set; }

        private int tileSize = 60; // Rozmiar pojedynczego pola
        private int offsetX;    // Przesunięcie szachownicy w poziomie
        private int offsetY;    // Przesunięcie szachownicy w pionie

        private readonly Brush lightBrush = Brushes.Beige; // Kolor jasnych pól
        private readonly Brush darkBrush = Brushes.Brown;  // Kolor ciemnych pól
        private readonly Brush selectedTileBrush = Brushes.YellowGreen; // Podświetlenie dla zaznaczenia

        private Bitmap? spriteSheet;  // Plik PNG z grafikami figur
        private Size spriteSize;      // Rozmiar pojedynczej grafiki figury

        private Point? selectedTile = null; // Wybrane pole (null, jeśli brak wybranego pola)

        public ChessBoardControl()
        {
            InitializeComponent();
            DoubleBuffered = true; // Włączenie buforowania, aby zapobiec migotaniu
            ResizeRedraw = true; // Odrysowanie kontrolki po zmianie rozmiaru

            // Załaduj spritesheet (plik PNG)
            try
            {
                spriteSheet = new Bitmap("chess_pieces.png"); // Ścieżka do pliku PNG

                // Rozmiar każdej figury w spritesheet
                spriteSize.Width = spriteSheet.Width / 6;    // 6 kolumn (P, R, N, B, Q, K)
                spriteSize.Height = spriteSheet.Height / 2;  // 2 wiersze (białe, czarne)
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var graphics = pe.Graphics;

            // Rysowanie pól szachownicy
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Wybierz kolor pola
                    Brush brush = (row + col) % 2 == 0 ? lightBrush : darkBrush;

                    // Rysuj prostokąt pola
                    graphics.FillRectangle(brush, offsetX + col * tileSize, offsetY + row * tileSize, tileSize, tileSize);

                    if (ChessBoard == null)
                    {
                        continue; // Nie rysujemy figur, jeśli ChessBoard nie jest ustawiony
                    }

                    // Rysowanie podświetlenia, jeśli pole jest wybrane
                    if (selectedTile != null && selectedTile.Value.X == col && selectedTile.Value.Y == row)
                    {
                        graphics.FillRectangle(selectedTileBrush, offsetX + col * tileSize, offsetY + row * tileSize, tileSize, tileSize);
                    }

                    // Pobierz figurę z ChessBoard
                    ChessPiece piece = ChessBoard.GetPiece(row, col);
                    if (piece != null)
                    {
                        char pieceCode = piece.GetCode(); // Pobierz literę figury

                        // Wyznacz prostokąt docelowy na szachownicy
                        Rectangle destRect = new Rectangle(
                            offsetX + col * tileSize,
                            offsetY + row * tileSize,
                            tileSize,
                            tileSize);

                        if (spriteSheet != null) // Udało się poprawnie wczytać plik z grafikami
                        {
                            // Pobierz pozycję grafiki reprezentującej figurę
                            Point spritePosition = GetSpritePosition(pieceCode);

                            // Wyznacz źródłowy prostokąt w spritesheet
                            Rectangle sourceRect = new Rectangle(
                                spritePosition.X * spriteSize.Width,
                                spritePosition.Y * spriteSize.Height,
                                spriteSize.Width,
                                spriteSize.Height);

                            // Rysowanie fragmentu spritesheet
                            graphics.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            // Nie udało się wczytać grafiki - wypisywanie symbolu figury
                            string pieceText = GetPieceUnicode(piece.GetCode());

                            // Format wypisywania tekstu
                            var textBrush = piece.Color == ChessPiece.PieceColor.White ? Brushes.Gray : Brushes.Black;
                            var font = new Font("Arial", tileSize * 0.4f);
                            var textSize = graphics.MeasureString(pieceText, font);
                            var stringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                            // Rysowanie symbolu figury
                            graphics.DrawString(pieceText, font, textBrush, destRect, stringFormat);

                        }
                    }
                }
            }

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Obliczenie TileSize na podstawie mniejszego z wymiarów okna
            tileSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / 8;

            // Obliczenie przesunięcia w celu wyśrodkowania szachownicy
            offsetX = (this.ClientSize.Width - tileSize * 8) / 2;
            offsetY = (this.ClientSize.Height - tileSize * 8) / 2;
        }

        // Zwraca pozycję figury w spritesheet na podstawie jej kodu.
        public static Point GetSpritePosition(char code)
        {
            return code switch
            {
                'P' => new Point(5, 0), // Biały pion
                'R' => new Point(4, 0), // Biała wieża
                'N' => new Point(3, 0), // Biały skoczek
                'B' => new Point(2, 0), // Biały goniec
                'Q' => new Point(1, 0), // Biała królowa
                'K' => new Point(0, 0), // Biały król
                'p' => new Point(5, 1), // Czarny pion
                'r' => new Point(4, 1), // Czarna wieża
                'n' => new Point(3, 1), // Czarny skoczek
                'b' => new Point(2, 1), // Czarny goniec
                'q' => new Point(1, 1), // Czarna królowa
                'k' => new Point(0, 1), // Czarny król
                _ => new Point(-1, -1)  // Nieprawidłowy kod
            };
        }

        // Zwraca symbol Unicode reprezentujący figurę.
        public static string GetPieceUnicode(char letter)
        {
            return letter switch
            {
                'P' => "\u2659", // Biały pion
                'R' => "\u2656", // Biała wieża
                'N' => "\u2658", // Biały skoczek
                'B' => "\u2657", // Biały goniec
                'Q' => "\u2655", // Biała królowa
                'K' => "\u2654", // Biały król
                'p' => "\u265F", // Czarny pion
                'r' => "\u265C", // Czarna wieża
                'n' => "\u265E", // Czarny skoczek
                'b' => "\u265D", // Czarny goniec
                'q' => "\u265B", // Czarna królowa
                'k' => "\u265A", // Czarny król
                _ => ""          // Nieprawidłowa litera
            };
        }

        // Obsługa kliknięcia myszy
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            // Przekonwertuj kliknięcie myszą na współrzędne szachownicy
            int col = (e.X - offsetX) / tileSize;
            int row = (e.Y - offsetY) / tileSize;

            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {
                // Jeśli zaznaczone pole jest już wybrane, spróbuj przesunąć figurę
                if (selectedTile != null)
                {
                    int fromRow = selectedTile.Value.Y;
                    int fromCol = selectedTile.Value.X;

                    if (ChessBoard?.GetPiece(fromRow, fromCol) != null)
                    {
                        // Przesunięcie figury
                        if (CommandManager != null)
                        {
                            CommandManager.ExecuteCommand(new MovePieceCommand(ChessBoard, fromRow, fromCol, row, col));
                        }
                        selectedTile = null; // Anuluj zaznaczenie po wykonaniu ruchu
                    }
                    else
                    {
                        // Nie wybrano żadnej figury do przesunięcia
                        selectedTile = new Point(col, row);
                    }
                }
                else
                {
                    // Ustaw zaznaczone pole
                    selectedTile = new Point(col, row);
                }

                // Odśwież kontrolkę, aby pokazać zmiany
                Invalidate();
            }
        }
    }
}
