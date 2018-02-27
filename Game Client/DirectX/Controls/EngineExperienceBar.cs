using SharpDX;
using SharpDX.Direct3D9;
using System;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public class EngineExperienceBar : EngineObject {
        /// <summary>
        /// Texturas de fundo e progressão da barra.
        /// </summary>
        private Texture textureColor, textureBack;

        /// <summary>
        /// Porcentagem da barra.
        /// </summary>
        public int Percentage { get; set; }
    
        public bool DrawText { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EngineExperienceBar(string name, int width, int height) {
            Name = string.Empty;
            Size = new Size2(width, height);
            SourceRect = new Rectangle(0, 0, width, height);
            Transparency = 255;
            Position = new Point(0, 0);
            Color = Color.White;
            Visible = true;
            Enabled = true;
            DrawText = true;
            SpriteFlags = SpriteFlags.AlphaBlend;
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            textureBack = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/{name}_back.png", width, height);
            textureColor = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/{name}_color.png", width, height);
            Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/{name}_border.png", width, height);                
        }

        /// <summary>
        /// Desenha o controle na tela.
        /// </summary>
        /// <param name="text"></param>
        public void Draw(string text) {
            if (!Visible) { return; }
            if (Transparency == 0) { return; }

            MouseButtons();
       
            EngineCore.SpriteDevice.Begin(SpriteFlags);
            //Desenha o fundo
            EngineCore.SpriteDevice.Draw(textureBack, _color, SourceRect, null, _position);
            //Desenha o progresso
            EngineCore.SpriteDevice.Draw(textureColor, _color, new Rectangle(0, 0, Size.Width * Percentage / 100, Size.Height), null, _position);
            //Desenha o restante
            EngineCore.SpriteDevice.Draw(Texture, _color, SourceRect, null, _position);
            EngineCore.SpriteDevice.End();

            if (DrawText == false) { return; }

            EngineFont.DrawText(text, new Size2(Size.Width, 0), new Point(Position.X, Position.Y + 22), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
        }
    }
}