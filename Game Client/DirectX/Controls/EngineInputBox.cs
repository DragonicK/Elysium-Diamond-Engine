using System;
using Elysium_Diamond.Network;
using Elysium_Diamond.EngineWindow;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public static class EngineInputBox {
        public static EngineInputBoxAction InputBoxAction { get; set; }

        public static EngineTextBox TextBox { get; set; }

        /// <summary>
        /// Texto de mensagem.
        /// </summary>
        public static string Text { get; set; }

        /// <summary>
        /// Transparencia do texto.
        /// </summary>
        public static byte TextTransparency { get; set; }

        /// <summary>
        /// Visibilidade do controle.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        /// Posição do controle.
        /// </summary>
        public static Point Position { get; set; }

        static EngineObject background;
        static EngineButton[] button = new EngineButton[2]; // 2 botões

        public static void Initialize() {
            Position = new Point(295, 15);
            Visible = false;
            TextTransparency = 255;

            background = new EngineObject();
            background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/inputbox.png", 424, 163);
            background.Size = new Size2(424, 163);
            background.Position = Position;
            background.SourceRect = new Rectangle(0, 0, 424, 163);
            background.Visible = true;

            TextBox = new EngineTextBox("textbox", 256, 32);
            TextBox.Size = new Size2(256, 32);
            TextBox.Position = new Point(Position.X + 90, Position.Y + 50);
            TextBox.SourceRect = new Rectangle(0, 0, 256, 32);
            TextBox.CursorEnabled = true;
            TextBox.TextTransparency = 255;
            TextBox.MouseUp += Textbox_MouseUp;
            TextBox.TextFormat = FontDrawFlags.Center;

            button[0] = new EngineButton("ok", 128, 32);
            button[0].Position = new Point(Position.X + 90, Position.Y + 85);
            button[0].Size = new Size2(128, 32);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            button[0].MouseUp += Button_1_MouseUp;

            button[1] = new EngineButton("cancel", 128, 32);
            button[1].Position = new Point(Position.X + 205, Position.Y + 85);
            button[1].Size = new Size2(128, 32);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            button[1].MouseUp += Button_2_MouseUp;
        }

        /// <summary>
        /// Exibe o controle
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        public static void Show(string text, EngineInputBoxAction action) {
            Text = text;
            Visible = true;
            InputBoxAction = action;
            TextBox.CursorEnabled = true;
        }

        /// <summary>
        /// Limpa os dados e esconde o controle.
        /// </summary>
        public static void Hide() {
            Visible = false;
            Text = string.Empty;
            TextBox.Text = string.Empty;
            TextBox.CursorEnabled = false;
        }

        /// <summary>
        /// Executa a lógica e desenha o controle.
        /// </summary>
        public static void Draw() {
            if (!Visible) { return; }
            background.Draw();

            button[0].Draw();
            button[1].Draw();

            TextBox.Draw();
            TextBox.DrawTextMesured();

            EngineFont.DrawText(Text, new Size2(400, 100), new Point(Position.X, Position.Y - 15), new Color(Color.White.R, Color.White.G, Color.White.B, TextTransparency), EngineFontStyle.Regular, FontDrawFlags.Center, false);
        }

 
        /// <summary>
        /// Método do botão 1, mouse up. (OK)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Button_1_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);

            //delete
            if (InputBoxAction == EngineInputBoxAction.Delete) {
                if (TextBox.Text.CompareTo("deletar") == 0) {
                    WorldPacket.DeleteCharacter((byte)WindowCharacter.SelectedIndex);
                    Hide();
                }
                else {
                    TextBox.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Método do botão 2, mouse up. (CANCEL)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Button_2_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);

            TextBox.CursorEnabled = false;
            Visible = false;
        }

        /// <summary>
        /// Método textbox mouse up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Textbox_MouseUp(object sender, EngineEventArgs e) {
            TextBox.CursorEnabled = true;
        }

    }
}