using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace Elysium_Diamond.DirectX {
    public class EngineButton : EngineObject, IDisposable {
        /// <summary>
        /// Quantidade de texturas.
        /// </summary>
        const int TOTAL_FILE = 3; 

        /// <summary>
        /// Obtem ou altera o estado atual do botão.
        /// </summary>
        public EngineButtonStyle State { get; set; }

        /// <summary>
        /// texturas dos botões
        /// </summary>
        private Texture[] texture = new Texture[TOTAL_FILE];

        /// <summary>
        /// Carrega um novo arquivo com tamanho definido.
        /// </summary>
        /// <param name="lang">Idioma</param>
        /// <param name="file">Arquivo</param>
        /// <param name="name">Nome de Botão</param>
        public EngineButton(string name, int width, int height) : base() {
            Size = new Size2(width, height);

            string language = Enum.GetName(typeof(Language), Common.Configuration.Language);
            texture[(int)EngineButtonStyle.Inactive] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/{language}/{name}_inactive.png", width, height);
            texture[(int)EngineButtonStyle.Hover] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/{language}/{name}_hover.png", width, height);
            texture[(int)EngineButtonStyle.Active] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/{language}/{name}_active.png", width, height);
        }

        /// <summary>
        /// Destrutor.
        /// </summary>
        ~EngineButton() {
            Dispose(false);
        }

        /// <summary>
        /// Desenha o controle.
        /// </summary>
        public override void Draw() {
            if (!Visible) { return; }
            if (!Enabled) { return; }

            State = EngineButtonStyle.Inactive;

            if (Enabled) {
                if (InsideButton()) {
                    if (!move) {
                        move = true;
                        OnMouseMove(EngineEventArgs.Empty);
                    }

                    State = EngineButtonStyle.Hover;

                     //if (EngineCore.MouseLeft || EngineCore.MouseRight) {
                     if (EngineCore.MouseLeft) {
                        State = EngineButtonStyle.Active;
                        left = EngineCore.MouseLeft;
                        //right = EngineCore.MouseRight;

                        if (!click) {
                            OnMouseDown(new EngineEventArgs(left, right));
                            click = true;
                        }
                    }
                    else {
                        if (click) {
                            OnMouseUp(new EngineEventArgs(left, right));
                        }

                        left = false;
                        //right = false;
                        click = false;
                        State = EngineButtonStyle.Hover;
                    }
                }
                else {
                    if (move) {
                        move = false;
                        OnMouseLeave(EngineEventArgs.Empty);
                    }
                }
            }

            EngineCore.SpriteDevice.Begin(SpriteFlags);
            EngineCore.SpriteDevice.Draw(texture[(int)State], _color, SourceRect, null, _position);
            EngineCore.SpriteDevice.End();
        }
    }
}