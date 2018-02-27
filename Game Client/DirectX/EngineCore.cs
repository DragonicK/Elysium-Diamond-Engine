using System;
using System.Windows.Forms;
using Elysium_Diamond.Resource;
using Elysium_Diamond.EngineWindow;
using Elysium_Diamond.Network;
using Elysium_Diamond.Classes;
using Elysium_Diamond.Npcs;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.DirectX {
    public class EngineCore {
        /// <summary>
        /// Dispostivo de directx.
        /// </summary>
        public static Device Device { get; set; }

        /// <summary>
        /// Dispositivo de sprite.
        /// </summary>
        public static Sprite SpriteDevice { get; set; }

        /// <summary>
        /// Etapa de desenho.
        /// </summary>
        public static byte GameState { get; set; }

        /// <summary>
        /// Quando o botão esquerdo é pressionado.
        /// </summary>
        public static bool MouseLeft { get; set; }

        /// <summary>
        /// Quando o botão direito é pressionado.
        /// </summary>
        public static bool MouseRight { get; set; }

        /// <summary>
        /// Coordenadas de mouse.
        /// </summary>
        public static Point MousePosition { get; set; }

        public static bool GameRunning { get; set; } = true;

        public static int FPS { get; set; }

        /// <summary>
        /// Controle de FPS.
        /// </summary>
        private static int pingTick, pFPS, tickFPS, tcpTick;

        /// <summary>
        /// Imagem de fundo para testes com transparência.
        /// </summary>
        private static EngineObject background;

        public static bool InitializeDirectX() {
            try {
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.BackBufferCount = 1;
                presentParams.SwapEffect = SwapEffect.Discard;
                presentParams.PresentationInterval = PresentInterval.Default;

                Device = new Device(new Direct3D(), 0, DeviceType.Hardware, Program.GraphicsDisplay.Handle, CreateFlags.SoftwareVertexProcessing, presentParams);
                SpriteDevice = new Sprite(Device);

                Device.SetRenderState(RenderState.SourceBlendAlpha, true);
                Device.SetRenderState(RenderState.DestinationBlendAlpha, true);

                return true;
            }
            catch (Exception ex) {
                throw ex;
     
            }
        }

        public static bool InitializeEngine() {
            NetworkSocket.Initialize();

            try {
                string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                background = new EngineObject($"{AppData}/Elysium/Data/background.png", 1024, 768);
                background.Size = new Size2(1024, 768);
                background.SourceRect = new Rectangle(0, 0, 1024, 720);


                DataManager.Initialize();

                WindowTalent.Initialize();
                WindowSkill.Initialize();
                WindowPin.Initialize();
                WindowCash.Initialize();
                WindowMail.Initialize();
                WindowSelectedItem.Initialize();
                WindowViewTalent.Initialize();

                //Carrega os dados de classe.
                ClasseManager.Initialize();

                //Carrega os dados de npc.
                NpcManager.OpenData();

                //Carrega os dados de experiencia
                ExperienceManager.Read();

                EngineFont.Initialize();
                EngineMessageBox.Initialize();
                EngineInputBox.Initialize();
                EngineMultimedia.Initialize();

                WindowLogin.Initialize();
                WindowServer.Initialize();
                WindowCharacter.Initialize();
                WindowNewCharacter.Initialize();

                WindowGame.Initialize();

                WindowViewItem.Initialize();

                //    EngineMultimedia.PlayMusic(0, true);

                GameState = 1;
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void Render() {
            if (Device == null) { return; }

            Device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
            Device.BeginScene();

            background.Draw();

            if (GameState == 1) {
             //   WindowTalent.Visible = true;
             //   WindowTalent.Draw();
               // WindowViewTalent.Draw();
              WindowLogin.Draw();
            }
            if (GameState == 2) { WindowServer.Draw(); }
            if (GameState == 3) {

                 WindowCharacter.Draw(); WindowPin.Draw();
            }
            if (GameState == 4) { WindowNewCharacter.Draw(); }
            if (GameState == 6) {
                WindowGame.Draw();
            }

            EngineInputBox.Draw();
            EngineMessageBox.Draw();

            EngineFont.DrawText("FPS: " + FPS, 925, 0, Color.Coral, EngineFontStyle.Bold);
            EngineFont.DrawText("Ping: " + Common.Configuration.Latency, 5, 0, Color.Coral, EngineFontStyle.Bold);

            Device.EndScene();
            Device.Present();
        }

        static public void Update() {
            NetworkSocket.ReceiveData();

            if (Environment.TickCount >= tcpTick + 1000) {
                if (!Common.Configuration.Disconnected) {
                    NetworkSocket.DiscoverServer(SocketEnum.LoginServer);
                    NetworkSocket.DiscoverServer(SocketEnum.WorldServer);
                    NetworkSocket.DiscoverServer(SocketEnum.GameServer);
                }

                tcpTick = Environment.TickCount;
            }

            //ping
            if (GameState == 6) {
                if (Environment.TickCount >= pingTick + 1000) {
                    GamePacket.RequestPing();
                    pingTick = Environment.TickCount;
                }
            }

            if (Environment.TickCount >= tickFPS + 1000) {
                FPS = pFPS;
                pFPS = 0;

                tickFPS = Environment.TickCount;
            }
            else {
                pFPS++;
            }
        }

        public static void Exit() {
            EngineMultimedia.StopMultimedia();

            //limpa o endereço do servidor
            if (NetworkSocket.Connected(SocketEnum.GameServer)) {
                Common.Configuration.IPAddress[(int)SocketEnum.GameServer].Clear();
                NetworkSocket.Disconnect(SocketEnum.GameServer);
            }

            Application.Exit();
        }
    }
}