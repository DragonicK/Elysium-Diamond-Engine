using System;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public class EngineLabel : EngineObject {
        private Color _textColor;
        private byte _textTransparency;

        /// <summary>
        /// Obtem ou altera o texto.
        /// </summary>
        public string Text { get; set; }

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
        /// Obtem ou altera a transparência da cor de texto.
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
        /// Posição do texto dentro do controle.
        /// </summary>
        public Point TextPosition { get; set; }

        /// <summary>
        /// Obtem ou altera o estilo de fonte.
        /// </summary>
        public EngineFontStyle TextFontStyle { get; set; }

        public FontDrawFlags FontDrawFlags { get; set; }

        /// <summary>
        /// Instancia a classe.
        /// </summary>
        public EngineLabel() : base() {       
            Text = string.Empty;
            TextColor = Color.White;
            TextVisible = true;
            TextTransparency = byte.MaxValue;
            TextFontStyle = EngineFontStyle.Regular;            
        }

        /// <summary>
        /// Carrega um novo arquivo com tamanho definido.
        /// </summary>
        /// <param name="file">Arquivo</param>
        /// <param name="width">Comprimento</param>
        /// <param name="height">Altura</param>
        public EngineLabel(string name, int width, int height) : base() {
            var language = Enum.GetName(typeof(Language), Common.Configuration.Language);

            Text = string.Empty;
            TextColor = Color.White;
            TextVisible = true;
            TextTransparency = byte.MaxValue;
            TextFontStyle = EngineFontStyle.Regular;
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/{name}.png", width, height);
            Size = new Size2(width, height);
            SourceRect = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Destrutor
        /// </summary>
        ~EngineLabel() {
            Dispose(false);
        }   

        /// <summary>
        /// Desenha o texto no centro do controle.
        /// </summary>
        public void DrawTextCenter() {
            if (!TextVisible) { return; }

            EngineFont.DrawText(Text, Size, new Point(Position.X + TextPosition.X, Position.Y + TextPosition.Y), new Color(TextColor.R, TextColor.G, TextColor.B, TextTransparency), TextFontStyle, FontDrawFlags.Left, false);
        }


        /// <summary>
        /// Desenha o texto.
        /// </summary>
        public void DrawText() {
            if (!TextVisible) { return; }

            EngineFont.DrawText(Text, Position.X + TextPosition.X, Position.Y + TextPosition.Y, _textColor, TextFontStyle);
        }

        /// <summary>
        /// Desenha o texto.
        /// </summary>
        /// <param name="text"></param>
        public void DrawText(string text) {
            if (!TextVisible) { return; }

            EngineFont.DrawText(text, new Rectangle(Position.X + TextPosition.X, Position.Y + TextPosition.Y, Size.Width, Size.Height), _textColor, TextFontStyle, FontDrawFlags);
        }
    }
}