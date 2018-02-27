using SharpDX;
using System;

namespace Elysium_Diamond.DirectX {
    public class EngineShortcut : EngineObject{
        /// <summary>
        /// Mostra ou esconde a janela.
        /// </summary>
        public bool Visibile { get; set; }

        /// <summary>
        /// Máximo de slots por barra.
        /// </summary>
        private const int MAX_SLOT = 12;

        /// <summary>
        /// Posição dos controles na janela.
        /// </summary>
        public new Point Position {
            get {
                return new Point((int)_position.X, (int)_position.Y);
            }
            set {
                _position = new Vector3(value.X, value.Y, 0);

                for (var n = 0; n < MAX_SLOT; n++) {
                    slots[n].Position = new Point((int)_position.X + (n * 36), (int)_position.Y);
                }
            }
        }

        /// <summary>
        /// Slots
        /// </summary>
        private EngineSlot[] slots = new EngineSlot[MAX_SLOT];

        /// <summary>
        /// Inicializa as configurações da janela.
        /// </summary>
        public EngineShortcut() {
            for (var n = 0; n < MAX_SLOT; n++) {
                slots[n] = new EngineSlot();
                slots[n].BorderRect = new Rectangle(6, 6, 34, 34);
                slots[n].SourceRect = new Rectangle(0, 0, 40, 40);
            }
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot.png", 40, 40);
        }
        
        /// <summary>
        /// Desenha a janela.
        /// </summary>
        public override void Draw() {
            for (var n = 0; n < MAX_SLOT; n++) {
                slots[n].Draw(Texture);
            }
        }
    }
}