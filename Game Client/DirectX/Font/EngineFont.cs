using System;
using System.IO;
using System.Drawing.Text;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;
using SD = System.Drawing;

namespace Elysium_Diamond.DirectX {
    public static class EngineFont {
        private static Font regular, bold, italic;
        private static Rectangle rect;

        /// <summary>
        /// Inicialização da font.
        /// </summary>
        /// <param name="fontName">Nome</param>
        /// <param name="size">Tamanho</param>
        public static void Initialize() {
            var fCollection = new PrivateFontCollection();

         //   var fontName = "Georgia";
            var size = 14f;

            #region Multi Font
        //    if (File.Exists(fontName)) {
        //        fCollection.AddFontFile(fontName); 
       //     }
        //    else { 
             fCollection.AddFontFile($"{Common.Configuration.GamePath}/Data/Font/OpenSans-Light.ttf"); 
        //    } 

            //  fCollection.Families[0] 
            //  
            #endregion

            var font = new SD.Font(fCollection.Families[0], size, SD.FontStyle.Regular, SD.GraphicsUnit.Pixel);
            regular = new Font(EngineCore.Device, font);

            font = new SD.Font(fCollection.Families[0], size, SD.FontStyle.Bold, SD.GraphicsUnit.Pixel);
            bold = new Font(EngineCore.Device, font);

            font = new SD.Font(fCollection.Families[0], size, SD.FontStyle.Italic, SD.GraphicsUnit.Pixel);
            italic = new Font(EngineCore.Device, font);

            font.Dispose();
            font = null;
        }

        /// <summary>
        /// Desenha o texto centralizado.
        /// </summary>
        /// <param name="text">Texto</param>
        /// <param name="size">Tamanho</param>
        /// <param name="point">Posição</param>
        /// <param name="color">Cor</param>
        /// <param name="style">Estilo</param>
        /// <param name="textformat">Formato</param>
        /// <param name="preload">Pré Carregamento</param>
        public static void DrawText(string text, Size2 size, Point point, Color color, EngineFontStyle style, FontDrawFlags textformat, bool preload = false) {
            if (string.IsNullOrEmpty(text)) { return; }

            if (style == EngineFontStyle.Regular) {
                rect = regular.MeasureText(null, text, textformat);
                if (preload) { regular.PreloadText(text); }
                regular.DrawText(null, text, point.X + (size.Width - rect.Width) / 2, (point.Y - 5) + (size.Height - rect.Height) / 2, color);
                return;
            }

            if (style == EngineFontStyle.Bold) {
                rect = bold.MeasureText(null, text, textformat);
                if (preload) { bold.PreloadText(text); }
                bold.DrawText(null, text, point.X + (size.Width - rect.Width) / 2, (point.Y - 5) + (size.Height - rect.Height) / 2, color);
                return;
            }

            if (style == EngineFontStyle.Italic) {
                rect = italic.MeasureText(null, text, textformat);
                if (preload) { italic.PreloadText(text); }
                italic.DrawText(null, text, point.X + (size.Width - rect.Width) / 2, (point.Y - 5) + (size.Height - rect.Height) / 2, color);
            }
        }

        /// <summary>
        /// Mede e retorna o comprimento e altura de um texto.
        /// </summary>
        /// <param name="style">Estilo</param>
        /// <param name="text">Texto</param>
        /// <param name="textformat">Formato</param>
        /// <returns></returns>
        public static Rectangle MeasureString(EngineFontStyle style, string text, FontDrawFlags textformat) {
            if (style == EngineFontStyle.Regular) {
                return regular.MeasureText(null, text, textformat);
            }

            if (style == EngineFontStyle.Bold) {
                return bold.MeasureText(null, text, textformat);
            }

            if (style == EngineFontStyle.Italic) {
                return italic.MeasureText(null, text, textformat);
            }

            return new Rectangle();
        }

        /// <summary>
        /// Mede e retorna o comprimento e altura de um texto.
        /// </summary>
        /// <param name="style">Estilo</param>
        /// <param name="text">Texto</param>
        /// <param name="rect">Retangulo</param>
        /// <param name="textformat">Formato</param>
        /// <returns></returns>
        public static Rectangle MeasureString(Sprite sprite, EngineFontStyle style, string text, Rectangle rect, FontDrawFlags textformat) {
            if (style == EngineFontStyle.Regular) {
                return regular.MeasureText(null, text, rect, textformat);
            }

            if (style == EngineFontStyle.Bold) {
                return bold.MeasureText(null, text, rect, textformat);
            }

            if (style == EngineFontStyle.Italic) {
                return italic.MeasureText(null, text, rect, textformat);
            }

            return new Rectangle();
        }

        /// <summary>
        /// Desenha o texto na coordenada especificada.
        /// </summary>
        /// <param name="sprite">Dispositivo</param>
        /// <param name="text">Texto</param>
        /// <param name="x">Coordenada X</param>
        /// <param name="y">Coordenada Y</param>
        /// <param name="color">Cor</param>
        /// <param name="style">Estilo</param>
        /// <param name="preload">Pré Carregamento</param>
        public static void DrawText(string text, int x, int y, Color color, EngineFontStyle style, bool preload = false) {
            if (string.IsNullOrEmpty(text)) { return; }

            if (style == EngineFontStyle.Regular) {
                if (preload) {
                    regular.PreloadText(text);
                }

                regular.DrawText(null, text, x, y, color);
                return;
            }

            if (style == EngineFontStyle.Bold) {
                if (preload) {
                    bold.PreloadText(text);
                }

                bold.DrawText(null, text, x, y, color);
                return;
            }

            if (style == EngineFontStyle.Italic) {
                if (preload) {
                    italic.PreloadText(text);
                }

                italic.DrawText(null, text, x, y, color);
            }
        }

        /// <summary>
        /// Desenha o texto na coordenada com quebra de linha.
        /// </summary>
        /// <param name="text">Texto</param>
        /// <param name="rec">Retângulo</param>
        /// <param name="color">Cor</param>
        /// <param name="style">Estilo</param>
        /// <param name="textformat">Formato</param>
        /// <param name="preload">Pré Carregamento</param>
        public static void DrawText(string text, Rectangle rec, Color color, EngineFontStyle style, FontDrawFlags textformat, bool preload = false) {
            if (string.IsNullOrEmpty(text)) { return; }

            if (style == EngineFontStyle.Regular) {
                if (preload) {
                    regular.PreloadText(text);
                }

                regular.DrawText(null, text, rec, textformat, color);
                return;
            }

            if (style == EngineFontStyle.Bold) {
                if (preload) {
                    bold.PreloadText(text);
                }

                bold.DrawText(null, text, rec, textformat, color);
                return;
            }

            if (style == EngineFontStyle.Italic) {
                if (preload) {
                    italic.PreloadText(text);
                }

                italic.DrawText(null, text, rec, textformat, color);
            }
        }
    }
}