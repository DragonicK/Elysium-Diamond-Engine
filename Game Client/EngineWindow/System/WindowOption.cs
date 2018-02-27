using System;
using Elysium_Diamond.Maps;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;
using SharpDX;

namespace Elysium_Diamond.EngineWindow {
    public class WindowOption {
        private const int MAX_BUTTON = 4;

        /// <summary>
        /// Obtem ou altera a visibilidade do controle.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        /// Botões, sair, personagem, opção 
        /// </summary>
        private static EngineButton[] button = new EngineButton[MAX_BUTTON];

        /// <summary>
        /// Imagem de fundo.
        /// </summary>
        private static EngineObject background = new EngineObject();

        /// <summary>
        /// Posição dos controles.
        /// </summary>
        private static Point position;
       
        /// <summary>
        /// Inicializa as configurações.
        /// </summary>
        public static void Initialize() {
            Visible = false;

            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            position = new Point(412, 200);
            background = new EngineObject();
            background.Position = position;
            background.Size = new Size2(167, 200);
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/option.png");
            background.SourceRect = new Rectangle(0, 0, 167, 200);
            background.Transparency = 255;

            button[0] = new EngineButton("close", 128, 32);
            button[0].Position = new Point(position.X + 20, position.Y + 35);
            button[0].BorderRect = new Rectangle(7, 2, 112, 26);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            button[0].Size = new Size2(128, 32);
            button[0].MouseUp += EndButton_MouseUp;

            button[1] = new EngineButton("char", 128, 32);
            button[1].Position = new Point(position.X + 20, position.Y + 70);
            button[1].BorderRect = new Rectangle(7, 2, 112, 26);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            button[1].Size = new Size2(128, 32);
            button[1].MouseUp += CharacterButton_MouseUp;

            button[2] = new EngineButton("option", 128, 32);
            button[2].Position = new Point(position.X + 20, position.Y + 105);
            button[2].BorderRect = new Rectangle(7, 2, 112, 26);
            button[2].SourceRect = new Rectangle(0, 0, 128, 32);
            button[2].Size = new Size2(128, 32);
            button[2].MouseUp += OptionButton_MouseUp;

            button[3] = new EngineButton("cancel_128", 128, 32);
            button[3].Position = new Point(position.X + 20, position.Y + 140);
            button[3].BorderRect = new Rectangle(7, 2, 112, 26);
            button[3].SourceRect = new Rectangle(0, 0, 128, 32);
            button[3].Size = new Size2(128, 32);
            button[3].MouseUp += CancelButton_MouseUp;
        }

        /// <summary>
        /// Desenha os controles.
        /// </summary>
        public static void Draw() {
            if (!Visible) { return; }

            background.Draw();
            button[0].Draw();
            button[1].Draw();
            button[2].Draw();
            button[3].Draw();
        }

        /// <summary>
        /// Sai do jogo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EndButton_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);

            //sai do jogo
            EngineCore.GameRunning = false;
        }

        /// <summary>
        /// Esconde a janela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CancelButton_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);
            Visible = false;
        }

        /// <summary>
        /// Abre a janela de opções
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OptionButton_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);

            Visible = false;
        }

        /// <summary>
        /// Volta a tela de seleção de personagem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CharacterButton_MouseUp(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Click);  

            //pede a lista de personagens
            WorldPacket.RequestPreLoad();

            //muda para seleção de personagem
            EngineCore.GameState = 3;
            WindowCharacter.SelectedIndex = 0;

            Visible = false;

            //limpa o endereço do servidor para uma nova conexão
            Common.Configuration.IPAddress[(int)SocketEnum.GameServer].Clear();
            NetworkSocket.Disconnect(SocketEnum.GameServer);

            //limpa a lista de jogadores.
            MapManager.Player.Clear();
            MapManager.Npc.Clear();

            //limpa as informações do jogador local.
            GameClient.Client.PlayerLocal.Character.Clear();
        }
    }
}