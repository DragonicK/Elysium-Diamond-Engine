using System;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public static class EngineMessageBox {
        /// <summary>
        /// Obtem ou altera o valor de transparência do controle.
        /// </summary>
        public static byte Transparency { get; set; }

        /// <summary>
        /// Otem ou altera a visibilidade do controle.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        /// Obtem ou altera a cor.
        /// </summary>
        public static Color color { get; set; }

        /// <summary>
        /// Obtem ou altera a mensagem de texto.
        /// </summary>
        public static string Text { get; set; }

        /// <summary>
        /// Obtem ou altera a posição do controle.
        /// </summary>
        public static Point Position { get; set; }

        /// <summary>
        /// Ativa ou desativa o botão de OK.
        /// </summary>
        public static bool Enabled { get; set; }

        public static EngineMessageBoxState State { get; set; } = EngineMessageBoxState.None;

        private static int tick;
        private static EngineObject background;
        private static EngineButton button;

        /// <summary>
        /// Inicializa a caixa de mensagem carregando as texturas.
        /// </summary>
        public static void Initialize() {
            Visible = false;
            Text = string.Empty;
            Transparency = 255;
            Position = new Point(272, 15);
            Enabled = true;

            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            background = new EngineObject();
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/msgbox.png", 480, 128);
            background.Enabled = false;
            background.Size = new Size2(480, 128);
            background.SourceRect = new Rectangle(0, 0, 480, 120);
            background.Position = Position;
            background.Visible = true;

            button = new EngineButton("ok", 128, 32);
            button.Position = new Point(Position.X + 173, Position.Y + 55);
            button.Size = new Size2(128, 32);
            button.BorderRect = new Rectangle(20, 2, 86, 26);
            button.SourceRect = new Rectangle(0, 0, 128, 32);
            button.MouseUp += Button_MouseUp;
        }

        /// <summary>
        /// Abre a caixa de mensagem.
        /// </summary>
        /// <param name="text">Texto</param>
        public static void Show(string text) {
            tick = Environment.TickCount;
            Text = text;
            Visible = true;
        }

        /// <summary>
        /// Desenha a caixa de mensagem.
        /// </summary>
        public static void Draw() {
            if (!Visible) { return; }

            if (Enabled == false) {
                if (Environment.TickCount >= tick + 10000) {
                    Text = "Sem conexão";
                    Enabled = true;
                    Common.Configuration.HexID = "";
                    Common.Configuration.IPAddress[(int)SocketEnum.GameServer] = new IPAddress(string.Empty, 0);
                }
            }

            background.Transparency = Transparency;
            button.Transparency = Transparency;

            background.Draw();

            EngineFont.DrawText(Text, new Size2(480, 80), new Point(Position.X, Position.Y - 1), new Color(Color.White.R, Color.White.G, Color.White.B, Transparency), EngineFontStyle.Regular, FontDrawFlags.Left);
            button.Draw();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Button_MouseUp(object sender, EngineEventArgs e) {
            if (Enabled == false) { return; }

            EngineMultimedia.Play(EngineSoundEnum.Click);
            Visible = false;

            if (Common.Configuration.Disconnected) { Environment.Exit(0); }
        }
    }
}
