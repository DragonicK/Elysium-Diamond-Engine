using System;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public class EngineTextBox : EngineObject {
        private Color _textColor;
        private byte _textTransparency;

        /// <summary>
        /// Obtem ou altera o cursor.
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Obtem ou altera o texto.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Quantidade máxima de caracteres.
        /// </summary>
        public short MaxLenght { get; set; } = 12;

        /// <summary>
        /// Obtem ou altera a cor do texto.
        /// </summary>
        public Color TextColor {
            get {
                return _textColor;
            }
            set {
                _textColor = new Color(value.R, value.G, value.B, _textTransparency);
            }
        }

        /// <summary>
        /// Obtem ou altera a visibilidade do texto.
        /// </summary>
        public bool TextVisible { get; set; }

        /// <summary>
        /// Obtem ou altera a transparencia do texto.
        /// </summary>
        public byte TextTransparency {
            get {
                return _textTransparency;
            }
            set {
                _textTransparency = value;
                _textColor = new Color(_textColor.R, _textColor.G, _textColor.B, value);
            }
        }

        /// <summary>
        /// Obtem ou altera a senha.
        /// </summary>
        public string PasswordText { get; set; }

        /// <summary>
        /// Obtem ou altera o modo para password.
        /// </summary>
        public bool Password { get; set; }

        /// <summary>
        /// Obtem ou altera o estado do cursor. Desativado ou ativado.
        /// </summary>
        public byte CursorState { get; set; }

        /// <summary>
        /// Obtem ou altera o valor para habilitar o cursor.
        /// </summary>
        public bool CursorEnabled { get; set; }

        /// <summary>
        /// Obtem ou altera o formato do texto. Left, Right, Bottom.
        /// </summary>
        public FontDrawFlags TextFormat { get; set; }

        /// <summary>
        /// Posição do texto dentro do controle.
        /// </summary>
        public Point TextPosition { get; set; }

        /// <summary>
        /// Tempo para piscar o cursor na tela.
        /// </summary>
        int varTime;
        Rectangle rec_cursor;

        /// <summary>
        /// Construtor.
        /// </summary>
        public EngineTextBox(string name, int width, int height) : base() {
            Cursor = " ";
            Text = string.Empty;
            TextColor = Color.White;
            TextTransparency = byte.MaxValue;
            TextVisible = true;
            Password = false;
            PasswordText = string.Empty;
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/{name}.png", width, height);
            varTime = Environment.TickCount;
        }

        /// <summary>
        /// Destrutor.
        /// </summary>
        ~EngineTextBox() {
            Dispose(false);
        }

        /// <summary>
        /// Desenha o texto centralizado no controle.
        /// </summary>
        public void DrawTextMesured() {
            DrawCursor();

            if (Password) {
                rec_cursor = EngineFont.MeasureString(EngineFontStyle.Regular, PasswordText, TextFormat);
                EngineFont.DrawText(PasswordText, Size, new Point(Position.X, Position.Y + 4), _textColor, EngineFontStyle.Regular, TextFormat);
            }
            else {
                rec_cursor = EngineFont.MeasureString(EngineFontStyle.Regular, Text, TextFormat);
                EngineFont.DrawText(Text, Size, new Point(Position.X, Position.Y + 4), _textColor, EngineFontStyle.Regular, TextFormat, false);
            }

            TextPosition = new Point(Position.X + ((Size.Width - rec_cursor.Width) / 2) + rec_cursor.Width, Position.Y + 4);
  
            EngineFont.DrawText(Cursor, Position.X + ((Size.Width - rec_cursor.Width) / 2) + rec_cursor.Width, Position.Y + 4, _textColor, EngineFontStyle.Regular);
        }

        /// <summary>
        /// Desenha o texto no controle.
        /// </summary>
        public void DrawText() {
            DrawCursor();

            if (Password) {
                EngineFont.DrawText(PasswordText + Cursor, Position.X + 10, Position.Y + 4, _textColor, EngineFontStyle.Regular);
            }
            else {
                EngineFont.DrawText(Text + Cursor, Position.X + 10, Position.Y + 4, _textColor, EngineFontStyle.Regular);
            }
        }

        /// <summary>
        /// Desenha o cursor no controle.
        /// </summary>
        public void DrawCursor() {
            if (CursorEnabled) {
                if (Environment.TickCount >= varTime + 800) {
                    varTime = Environment.TickCount;

                    if (CursorState == 0) {
                        CursorState = 1;
                        Cursor = "|";
                    }
                    else {
                        CursorState = 0;
                        Cursor = " ";
                    }
                }
            }
            else {
                Cursor = " ";
            }
        }

        /// <summary>
        /// Adiciona um caractere quando pressionado o teclado.
        /// </summary>
        /// <param name="character">KeyChar</param>
        public void AddText(char character) {
            if (Text.Length < MaxLenght) {
                Text += character;
                if (Password) PasswordText += "*";
            }
        }

        /// <summary>
        /// Remove o último caractere quando pressionado backspace.
        /// </summary>
        public void RemoveText() {
            if (Text.Length == 0) return;

            Text = Text.Substring(0, Text.Length - 1);
            if (Password) PasswordText = PasswordText.Substring(0, PasswordText.Length - 1);
        }

        /// <summary>
        /// Limpa os dados do textbox.
        /// </summary>
        public void Clear() {
            Text = string.Empty;
            PasswordText = string.Empty;
        }
    }
}