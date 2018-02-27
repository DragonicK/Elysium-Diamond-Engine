using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowViewTalent {
        /// <summary>
        /// Índice da janela.
        /// </summary>
        public static int Index { get; set; }

        /// <summary>
        /// Mostra ou esconde a janela.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        /// Posição do controle na tela.
        /// </summary>
        public static Point Position { get; set; }

        private static TalentResource talent;
        private static EngineObject background;
        private static int talentID, talentLevel;
        private static string text;

        /// <summary>
        /// Inicializa a configuração dos controles.
        /// </summary>
        public static void Initialize() {
            Position = new Point(352, 270);

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(320, 224);
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/talent_view.png");
            background.SourceRect = new Rectangle(0, 0, 320, 224);
        }

        public static void CopyTalent(int id, int level = 1) {
            talent = DataManager.FindTalentByID(id);

            talentID = id;
            talentLevel = level;
            Visible = true;

            text = talent.Description;

            if (talent.Effect.Count > 0) {
                for (var n = 0; n < talent.Effect.Count; n++) {
                    text = text.Replace($"'effect{n}'", (talent.Effect[n] * talentLevel) + "");
                }
            }
        }

        /// <summary>
        /// Desenha o controle.
        /// </summary>
        public static void Draw() {
           if (!Visible) return;

            background.Draw();

            DrawText();
        }

        private static void DrawText() {
            EngineFont.DrawText($"{talent.Title} Lv. {talentLevel} / {talent.MaxLevel}", new Rectangle(Position.X, Position.Y + 10, 320, 20), Color.GreenYellow, EngineFontStyle.Regular, FontDrawFlags.Center);
         
            EngineFont.DrawText(text, new Rectangle(Position.X + 20, Position.Y + 40, 290, 120), Color.White, EngineFontStyle.Regular, FontDrawFlags.WordBreak);

            EngineFont.DrawText("Requerimento", new Rectangle(Position.X + 20, Position.Y + 140, 290, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText($"Esplendor da Recuperação Level {talent.ReqTalentLevel}", new Rectangle(Position.X + 20, Position.Y + 160, 290, 20), Color.Crimson, EngineFontStyle.Regular, FontDrawFlags.Left);
            //Color.Crimson
        }
    }
}
