using System;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowLogin {
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

        /// <summary>
        /// Botões de Login e End
        /// </summary>
        private static EngineButton[] button = new EngineButton[2];

        /// <summary>
        /// Textbox de account e password
        /// </summary>
        public static EngineTextBox[] textbox = new EngineTextBox[2];

        /// <summary>
        /// Imagem de fundo
        /// </summary>
        private static EngineObject background;

        /// <summary>
        /// Inicia e configura os controles
        /// </summary>
        public static void Initialize() {
            Position = new Point(342, 270);
            Visible = true;

            background = new EngineObject();
            background.Position = new Point(Position.X - 140, Position.Y);
            background.Size = new Size2(608, 224);
            background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/window_login.png");
            background.SourceRect = new Rectangle(0, 0, 608, 224);
            background.Transparency = 255;

            textbox[0] = new EngineTextBox("textbox", 256, 32);
            textbox[0].Size = new Size2(256, 32);
            textbox[0].SourceRect = new Rectangle(0, 0, 256, 32);
            textbox[0].BorderRect = new Rectangle(0, 0, 256, 32);
            textbox[0].Position = new Point(Position.X + 32, Position.Y + 50);
            textbox[0].CursorEnabled = true;
            textbox[0].Transparency = 220;
            textbox[0].TextTransparency = 255;
            textbox[0].MouseUp += Textbox_1_MouseUp;
            textbox[0].TextFormat = FontDrawFlags.Center;

            textbox[1] = new EngineTextBox("textbox", 256, 32);
            textbox[1].Size = new Size2(256, 32);
            textbox[1].SourceRect = new Rectangle(0, 0, 256, 32);
            textbox[1].BorderRect = new Rectangle(0, 0, 256, 32);
            textbox[1].Position = new Point(Position.X + 32, Position.Y + 80);
            textbox[1].Password = true;
            textbox[1].Transparency = 220;
            textbox[1].TextTransparency = 255;
            textbox[1].MouseUp += Textbox_2_MouseUp;
            textbox[1].TextFormat = FontDrawFlags.Center;

            button[0] = new EngineButton("login", 128, 32);
            button[0].Position = new Point(Position.X + 42, Position.Y + 120);
            button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            button[0].Size = new Size2(128, 32);
            button[0].MouseUp += LoginButton_MouseDown;

            button[1] = new EngineButton("end", 128, 32);
            button[1].Position = new Point(Position.X + 152, Position.Y + 120);
            button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            button[1].Size = new Size2(128, 32);
            button[1].MouseUp += ExitButton_MouseDown;
        }

        /// <summary>
        /// Realiza o desenho da janela
        /// </summary>
        public static void Draw() {
            if (!Visible) { return; }

            background.Draw();
            textbox[0].MouseButtons();
            textbox[0].Draw();
            textbox[0].DrawTextMesured();
            textbox[1].MouseButtons();
            textbox[1].Draw();
            textbox[1].DrawTextMesured();

            if (textbox[0].Text.Length == 0) {
                if (textbox[0].CursorEnabled == false) {
                    EngineFont.DrawText("username", textbox[0].Size, new Point(textbox[0].Position.X, textbox[0].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                }
            }

            if (textbox[1].Text.Length == 0) {
                if (textbox[1].CursorEnabled == false) {
                    EngineFont.DrawText("password", textbox[1].Size, new Point(textbox[1].Position.X, textbox[1].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                }
            }

            button[0].Draw();
            button[1].Draw();
        }

        /// <summary>
        /// Executa o login.
        /// </summary>
        public static void Login() {
            if (Common.Configuration.Disconnected) { return; }
            if (EngineMessageBox.Visible) { return; }

            EngineMultimedia.Play(EngineSoundEnum.Click);

            if (NetworkSocket.Connected(SocketEnum.LoginServer) == false) {
                EngineMessageBox.Enabled = true;
                EngineMessageBox.Show("Sem conexão com o servidor");
                return;
            }

            if (textbox[0].Text.Length <= 4) {
                EngineMessageBox.Enabled = true;
                EngineMessageBox.Show("O nome não pode ser menor que 5 dígitos");
                return;
            }

            if (textbox[1].Text.Length <= 4) {
                EngineMessageBox.Enabled = true;
                EngineMessageBox.Show("A senha não pode ser menor que 5 dígitos");
                return;
            }

            LoginPacket.Login(textbox[0].Text.Trim(), textbox[1].Text.Trim());
        }

        #region Button Event
        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void LoginButton_MouseDown(object sender, EngineEventArgs e) {
            Login();
        }

        /// <summary>
        /// Fecha o programa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ExitButton_MouseDown(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) { return; }

            EngineMultimedia.Play(EngineSoundEnum.Click);

            EngineCore.GameRunning = false;
        }

        /// <summary>
        /// Seleciona o textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Textbox_1_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) { return; }
            textbox[0].CursorState = 0;
            textbox[0].CursorEnabled = true;
            textbox[1].CursorEnabled = false;
        }

        /// <summary>
        /// Seleciona o textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Textbox_2_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) { return; }
            textbox[1].CursorState = 0;
            textbox[0].CursorEnabled = false;
            textbox[1].CursorEnabled = true;
        }
        #endregion
    }
}
