using System;
using System.IO;
using System.Collections.Generic;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.GameClient;
using Elysium_Diamond.Resource;
using Elysium_Diamond.Classes;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowTalent {
        /// <summary>
        /// Lista de todos os talentos.
        /// </summary>
        public static List<ClasseTalent> Talents { get; set; }

        /// <summary>
        /// Índice do talento.
        /// </summary>
        private static int classe_index = -1;

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

        public static int Level { get; set; }
        public static long Experience { get; set; }
        public static int Points { get; set; }

        /// <summary>
        /// Quantidade de caminhos talentos.
        /// </summary>
        const int MAX_TALENT_PATH = 4;

        /// <summary>
        /// Máximo de talentos por aba.
        /// </summary>
        const int MAX_TALENT = 24;

        private static EngineSlot[] balance = new EngineSlot[MAX_TALENT];
        private static EngineSlot[] physic = new EngineSlot[MAX_TALENT];
        private static EngineSlot[] magic = new EngineSlot[MAX_TALENT];
        private static EngineSlot[] restoration = new EngineSlot[MAX_TALENT];

        public static Talent[] Balance = new Talent[MAX_TALENT];
        public static Talent[] Physic = new Talent[MAX_TALENT];
        public static Talent[] Magic = new Talent[MAX_TALENT];
        public static Talent[] Restoration = new Talent[MAX_TALENT];

        private static EngineButton closebutton;
        private static EngineObject main_background;
        private static EngineObject slot_background;
        private static EngineExperienceBar experience;

        private static Texture slot;

        /// <summary>
        /// Inicializa as configurações da janela.
        /// </summary>
        public static void Initialize() {
            //posiciona a janela no centro
            Position = new Point(112, 152);

            Talents = new List<ClasseTalent>();

            main_background = new EngineObject();
            main_background.Position = Position;
            main_background.Size = new Size2(800, 416);
            main_background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/talent_back.png", 800, 416);
            main_background.SourceRect = new Rectangle(0, 0, 800, 416);

            slot_background = new EngineObject();
            slot_background.Size = new Size2(192, 288);
            slot_background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/talent_slot_back.png", 192, 288);
            slot_background.SourceRect = new Rectangle(0, 0, 192, 288);

            closebutton = new EngineButton("closex", 32, 32);
            closebutton.Position = new Point(Position.X + 758, Position.Y + 10);
            closebutton.BorderRect = new Rectangle(8, 8, 16, 16);
            closebutton.SourceRect = new Rectangle(0, 0, 32, 32);
            closebutton.Size = new Size2(32, 32);
            closebutton.MouseUp += CloseButton_MouseUp;

            experience = new EngineExperienceBar("bar", 519, 36);
            experience.Position = new Point(Position.X + 140, Position.Y + 35);
            experience.Percentage = 50;

            slot = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/slot.png", 40, 40);

            var new_y = 110;
            var new_x = 0;
            for (int n = 0; n < MAX_TALENT; n++) {
                balance[n] = new EngineSlot();
                physic[n] = new EngineSlot();
                magic[n] = new EngineSlot();
                restoration[n] = new EngineSlot();

                balance[n].Enabled = true;

                balance[n].Index = physic[n].Index = magic[n].Index = restoration[n].Index = n;
                balance[n].Size = physic[n].Size = magic[n].Size = restoration[n].Size = new Size2(40, 40);
                balance[n].SourceRect = physic[n].SourceRect = magic[n].SourceRect = restoration[n].SourceRect = new Rectangle(0, 0, 40, 40);
                balance[n].BorderRect = physic[n].BorderRect = magic[n].BorderRect = restoration[n].BorderRect = new Rectangle(8, 8, 32, 32);
                //balance[n].MouseUp += Numbers_MouseUp;
                //balance[n].MouseMove += Numbers_MouseMove;
                //balance[n].MouseLeave += Numbers_MouseLeave;

                if (n >= 4 & n < 8) { new_x = 4; new_y = 150; }
                if (n >= 8 & n < 12) { new_x = 8; new_y = 190; }    
                if (n >= 12 & n < 16) { new_x = 12; new_y = 230; }
                if (n >= 16 & n < 20) { new_x = 16; new_y = 270; }
                if (n >= 20 & n < 24) { new_x = 20; new_y = 310; }

                balance[n].IconTransparency = 120;
                physic[n].IconTransparency = 120;
                magic[n].IconTransparency = 120;
                restoration[n].IconTransparency = 120;

                balance[n].Position = new Point(Position.X + 35 + ((n - new_x) * 38), Position.Y + new_y);
                physic[n].Position = new Point(Position.X + 227 + ((n - new_x) * 38), Position.Y + new_y);
                magic[n].Position = new Point(Position.X + 419 + ((n - new_x) * 38), Position.Y + new_y);
                restoration[n].Position = new Point(Position.X + 611 + ((n - new_x) * 38), Position.Y + new_y);

                balance[n].MouseUp += Balance_MouseUp;
                balance[n].MouseMove += Balance_MouseMove;
                physic[n].MouseUp += Physic_MouseUp;
                physic[n].MouseMove += Physic_MouseMove;
                magic[n].MouseUp += Magic_MouseUp;
                magic[n].MouseMove += Magic_MouseMove;
                restoration[n].MouseUp += Restoration_MouseUp;
                restoration[n].MouseMove += Restoration_MouseMove;
            }

            OpenTalents();

            UpdateIcons();
        }

        /// <summary>
        /// Desenha a janela.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            main_background.Draw();

            for (int n = 0; n < MAX_TALENT_PATH; n++) {
                slot_background.Position = new Point(Position.X + 16 + (n * 192), Position.Y + 90);
                slot_background.Draw();
            }

            for (int n = 0; n < MAX_TALENT; n++) {
                balance[n].Draw(slot);
                balance[n].MouseButtons();

                physic[n].Draw(slot);
                physic[n].MouseButtons();

                magic[n].Draw(slot);
                magic[n].MouseButtons();

                restoration[n].Draw(slot);
                restoration[n].MouseButtons();
            }

            closebutton.Draw();
           // experience.Percentage =  Convert.ToInt32(((double)Experience / (double)ExperienceManager.Talent[Level + 1]) * 100);
            experience.Draw($"{Experience} / {ExperienceManager.Talent[Level + 1]}");
            EngineFont.DrawText($"Level {Level} Pontos {Points}", new Rectangle(Position.X + 140, Position.Y + 20, 519, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText("Equilíbrio 0", new Rectangle(Position.X + 16, Position.Y + 70, 192, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText("Físico 0", new Rectangle(Position.X + 208, Position.Y + 70, 192, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText("Mágico 0", new Rectangle(Position.X + 400, Position.Y + 70, 192, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText("Restauração 0", new Rectangle(Position.X + 592, Position.Y + 70, 192, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
        }

        public static int FindIndex(int id) {
            for(var n = 0; n < Talents.Count; n++) {
                if (Talents[n].ClasseID == id) return n;
            }

            return -1;
        }

        /// <summary>
        /// Atualiza os ícones nos slots.
        /// </summary>
        public static void UpdateIcons() {
           // if (Client.PlayerLocal.ClasseID == 0) return;
            TalentResource talent;

            var index = 1;

            // var index = FindIndex(Client.PlayerLocal.ClasseID);

            for (int n = 0; n < MAX_TALENT; n++) {
                talent = DataManager.FindTalentByID(Talents[index].Balance[n]);
                if (talent != null) {
                    balance[n].IconID = talent.IconID;
                }
                else {
                    balance[n].IconID = 0;
                }

                talent = DataManager.FindTalentByID(Talents[index].Physic[n]);
                if (talent != null) {
                    physic[n].IconID = talent.IconID;
                }
                else {
                    physic[n].IconID = 0;
                }

                talent = DataManager.FindTalentByID(Talents[index].Magic[n]);
                if (talent != null) {
                    magic[n].IconID = talent.IconID;
                }
                else {
                    magic[n].IconID = 0;
                };

                talent = DataManager.FindTalentByID(Talents[index].Restoration[n]);
                if (talent != null) {
                    restoration[n].IconID = talent.IconID;
                }
                else {
                    restoration[n].IconID = 0;
                }
            }
        }

        /// <summary>
        /// Fecha a janela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseButton_MouseUp(object sender, EngineEventArgs e) {
            Visible = false;
        }


        private static void Balance_MouseUp(object sender, EngineEventArgs e) {

        }

        private static void Balance_MouseMove(object sender, EngineEventArgs e) {
            WindowViewTalent.CopyTalent(1, 1);
        }

        private static void Physic_MouseUp(object sender, EngineEventArgs e) {

        }

        private static void Physic_MouseMove(object sender, EngineEventArgs e) {
            WindowViewTalent.CopyTalent(1, 1);
        }

        private static void Magic_MouseUp(object sender, EngineEventArgs e) {

        }

        private static void Magic_MouseMove(object sender, EngineEventArgs e) {
            WindowViewTalent.CopyTalent(1, 1);
        }


        private static void Restoration_MouseUp(object sender, EngineEventArgs e) {

        }

        private static void Restoration_MouseMove(object sender, EngineEventArgs e) {
            WindowViewTalent.CopyTalent(1, 1);
        }

        /// <summary>
        /// Carrega todos os dados.
        /// </summary>
        private static void OpenTalents() {
            if (!File.Exists($"{Common.Configuration.GamePath}/Data/Talent.bin")) {
                return;
            }

            using (FileStream file = new FileStream($"{Common.Configuration.GamePath}/Data/Talent.bin", FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new BinaryReader(file);

                //obtem a quantidade de classes
                var count = reader.ReadInt32();

                for (var n = 0; n < count; n++) {

                    Talents.Add(new ClasseTalent(reader.ReadInt32(), reader.ReadString()));

                    for (var i = 0; i < MAX_TALENT_PATH; i++) {
                        Talents[n].TalentName[i] = reader.ReadString();
                    }

                    for (var j = 0; j < MAX_TALENT; j++) {
                        Talents[n].Balance[j] = reader.ReadInt32();
                        Talents[n].Physic[j] = reader.ReadInt32();
                        Talents[n].Magic[j] = reader.ReadInt32();
                        Talents[n].Restoration[j] = reader.ReadInt32();
                    }
                }

                reader.Close();
            }
        }
    }
}