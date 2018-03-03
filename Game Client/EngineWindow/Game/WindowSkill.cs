using System;
using Elysium_Diamond.DirectX;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowSkill {
        /// <summary>
        /// Índice do controle.
        /// </summary>
        public static int Index { get; set; }

        /// <summary>
        /// Posição da janela.
        /// </summary>
        public static Point Position { get; set; }

        /// <summary>
        /// Mostra ou esconde a janela.
        /// </summary>
        public static bool Visible { get; set; } 

        /// <summary>
        /// Quantidade máxima de slots de skills.
        /// </summary>
        const int MAX_SKILL = 7;
        const int MAX_BUTTON = 5;

        private static EngineLabel[] labels = new EngineLabel[MAX_SKILL];
        private static EngineButton[] buttons = new EngineButton[MAX_BUTTON];

        private static EngineObject background;
        private static EngineExperienceBar experience;

        private static Texture slot, text, desc;

        /// <summary>
        /// Inicializa as configurações da janela.
        /// </summary>
        public static void Initialize() {
            Position = new Point(208, 168);

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(608, 384);
            background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/skill_back.png", 608, 384);
            background.SourceRect = new Rectangle(0, 0, 608, 384);

            buttons[0] = new EngineButton("active", 128, 32);
            buttons[0].Position = new Point(Position.X + 140, Position.Y + 305);
            buttons[0].BorderRect = new Rectangle(20, 2, 86, 26);
            buttons[0].SourceRect = new Rectangle(0, 0, 128, 32);
            buttons[0].Size = new Size2(32, 32);
            // buttons[0].MouseUp += CloseButton_MouseUp;

            buttons[1] = new EngineButton("passive", 128, 32);
            buttons[1].Position = new Point(Position.X + 340, Position.Y + 305);
            buttons[1].BorderRect = new Rectangle(20, 2, 86, 26);
            buttons[1].SourceRect = new Rectangle(0, 0, 128, 32);
            buttons[1].Size = new Size2(32, 32);
            //buttons[1].MouseUp += CloseButton_MouseUp;

            buttons[2] = new EngineButton("closex", 32, 32);
            buttons[2].Position = new Point(Position.X + 550, Position.Y + 10);
            buttons[2].BorderRect = new Rectangle(8, 8, 16, 16);
            buttons[2].SourceRect = new Rectangle(0, 0, 32, 32);
            buttons[2].Size = new Size2(32, 32);
            buttons[2].MouseUp += CloseButton_MouseUp;

            buttons[3] = new EngineButton("subtracttype2", 32, 32);
            buttons[3].Position = new Point(Position.X + 255, Position.Y + 305);
            buttons[3].BorderRect = new Rectangle(8, 8, 16, 16);
            buttons[3].SourceRect = new Rectangle(0, 0, 32, 32);
            buttons[3].Size = new Size2(32, 32);
            // buttons[2].MouseUp += CloseButton_MouseUp;

            buttons[4] = new EngineButton("sumtype2", 32, 32);
            buttons[4].Position = new Point(Position.X + 320, Position.Y + 305);
            buttons[4].BorderRect = new Rectangle(8, 8, 16, 16);
            buttons[4].SourceRect = new Rectangle(0, 0, 32, 32);
            buttons[4].Size = new Size2(32, 32);
            // buttons[2].MouseUp += CloseButton_MouseUp;

            slot = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/slot.png", 40, 40);
            text = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/textbox_2.png", 224, 32);
            desc = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/textbox_230.png", 288, 256);

            for (var n = 0; n < MAX_SKILL; n++) {
                labels[n] = new EngineLabel();
                labels[n].Index = n;
                labels[n].Size = new Size2(224, 32);
                labels[n].SourceRect = new Rectangle(0, 0, 224, 32);
                labels[n].BorderRect = new Rectangle(6, 2, 218, 30);
                labels[n].Position = new Point(Position.X + 60, Position.Y + 25 + (n * 40));
                labels[n].TextPosition = new Point(10, 5);
            }

            experience = new EngineExperienceBar("bar260", 288, 36);
            experience.Position = new Point(Position.X + 287, Position.Y + 30);
            experience.Percentage = 100;
        }

        /// <summary>
        /// Desenha a janela.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            background.Draw();

            experience.Draw("0 / 0");

            for (var n = 0; n < MAX_SKILL; n++) {
                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                EngineCore.SpriteDevice.Draw(slot, Color.White, new Rectangle(0, 0, 40, 40), null, new Vector3(Position.X + 25, Position.Y + 20 + (n * 40), 0));
                EngineCore.SpriteDevice.End();

                labels[n].Draw(text);
                labels[n].DrawText();

                if (n < 5) {
                    buttons[n].Draw();
                }
            }

            //desenha o nome e a skill selecionada
            EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
            EngineCore.SpriteDevice.Draw(desc, Color.White, new Rectangle(0, 0, 288, 256), null, new Vector3(Position.X + 302, Position.Y + 65, 0));
            EngineCore.SpriteDevice.End();

            EngineFont.DrawText("Level 1", new Rectangle(Position.X + 287, Position.Y + 17, 288, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);

            EngineFont.DrawText("1/1", new Rectangle(Position.X, Position.Y + 310, 608, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);


            // EngineFont.DrawText("Forms a protective shield that can absorb up to 927 damage for 20s, and boosts the effects of healing-type skills by 100%. The protective shield remains effective until it absorbs a total of 927 damage.", new Rectangle(Position.X + 308, Position.Y + 155, 250, 160), Color.White, EngineFontStyle.Regular, FontDrawFlags.WordBreak);
            var text_ = "Inflict 532 earth damage to a target within a 25m radius, and deals periodic 507 damage every 12s for 2m.In addition, reduces the target's Magic Resist by 400. If the target dies while the skill is in effect, it is ressurected at its registered Kisk or Obelisk";
            EngineFont.DrawText(text_, new Rectangle(Position.X + 309, Position.Y + 65, 255, 230), Color.White, EngineFontStyle.Regular, FontDrawFlags.WordBreak);
            EngineFont.DrawText("Uso de MP 950", new Rectangle(Position.X + 308, Position.Y + 235, 290, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText("Conjuração 10 seg", new Rectangle(Position.X + 308, Position.Y + 255, 290, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText("Resfriamento 2 min", new Rectangle(Position.X + 308, Position.Y + 275, 290, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
        }

        /// <summary>
        /// Fecha a janela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseButton_MouseUp(object sender, EngineEventArgs e) {
            Visible = false;
        }
    }
}