using Elysium_Diamond.GameClient;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowStatus {
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
        /// Items equipados no personagem.
        /// </summary>
        public static Item[] EquippedItem { get; set; } = new Item[MAX_EQUIPPED_ITEM];

        /// <summary>
        /// Máximo de items que podem ser equipados.
        /// </summary>
        const int MAX_EQUIPPED_ITEM = 14;

        /// <summary>
        /// Máximo de slots do personagem.
        /// </summary>
        const int MAX_SLOT = 14;

        /// <summary>
        /// Quantidade máxima de atributos.
        /// </summary>
        const int MAX_LABEL = 7;

        /// <summary>
        /// Botões de adição.
        /// </summary>
        const int MAX_BUTTON = 8;

        /// <summary>
        /// Textura do slot.
        /// </summary>
        private static Texture slot;

        /// <summary>
        /// Slots de items.
        /// </summary>
        private static EngineSlot[] slots = new EngineSlot[MAX_SLOT];

        /// <summary>
        /// Botões de stats.
        /// </summary>
        private static EngineLabel[] labels = new EngineLabel[MAX_LABEL];

        /// <summary>
        /// Botão de adição.
        /// </summary>
        private static EngineButton[] buttons = new EngineButton[MAX_BUTTON];

        /// <summary>
        /// Ícone dos items.
        /// </summary>
        private static int[] Icons = new int[MAX_EQUIPPED_ITEM];

        /// <summary>
        /// Aba selecionada.
        /// </summary>
        private static int SelectedPage = 0;

        /// <summary>
        /// Botão de fechar.
        /// </summary>
        private static EngineButton closebutton;
        private static EngineObject background;

        /// <summary>
        /// Inicializa as configurações do controle.
        /// </summary>
        public static void Initialize() {
            Position = new Point(50, 100);
            slot = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/slot.png", 40, 40);

            background = new EngineObject($"{Common.Configuration.GamePath}/Data/Graphics/inventory.png");
            background.Size = new Size2(320, 352);
            background.Position = Position;
            background.SourceRect = new Rectangle(0, 0, 320, 352);

            closebutton = new EngineButton("closex", 32, 32);
            closebutton.Position = new Point(Position.X + 281, Position.Y + 5);
            closebutton.BorderRect = new Rectangle(8, 8, 16, 16);
            closebutton.SourceRect = new Rectangle(0, 0, 32, 32);
            closebutton.Size = new Size2(32, 32);
            closebutton.MouseUp += CloseButton_MouseUp;

            for (var n = 0; n < MAX_BUTTON; n++) {
                buttons[n] = new EngineButton("sumtype2", 32, 32);
                buttons[n].Index = n;
                buttons[n].Position = new Point(Position.X + 65, Position.Y + 65 + (n * 20));
                buttons[n].BorderRect = new Rectangle(8, 8, 16, 16);
                buttons[n].SourceRect = new Rectangle(0, 0, 32, 32);
                buttons[n].Size = new Size2(32, 32);
                buttons[n].MouseUp += StatButtons_MouseUp;
            }

            for (var n = 0; n < MAX_LABEL; n++) {
                labels[n] = new EngineLabel();
                labels[n].Index = n;
                labels[n].Text = (n + 1) + "";
                labels[n].Size = new Size2(40, 40);
                labels[n].SourceRect = new Rectangle(0, 0, 40, 40);
                labels[n].BorderRect = new Rectangle(10, 10, 30, 30);
                labels[n].Transparency = 255;
                labels[n].Position = new Point(Position.X + 17 + (n * 40), Position.Y + 290);
                labels[n].MouseUp += Labels_MouseUp;
            }

            labels[0].TextFontStyle = EngineFontStyle.Bold;

            for (var n = 0; n < MAX_SLOT; n++) {
                slots[n] = new EngineSlot();
                slots[n].Size = new Size2(40, 40);
                slots[n].MouseRight = true;
                slots[n].SourceRect = new Rectangle(0, 0, 40, 40);
                slots[n].BorderRect = new Rectangle(8, 8, 32, 32);
                slots[n].MouseUp += Slots_MouseUp;
                slots[n].MouseMove += Slots_MouseMove;
                slots[n].MouseLeave += Slots_MouseLeave;

                if (n >= 0 && n < 7) slots[n].Position = new Point(Position.X + 22, Position.Y + 35 + (n * 36));
                if (n >= 7 && n < MAX_SLOT) slots[n].Position = new Point(Position.X + 256, Position.Y + 35 + ((n - MAX_SLOT / 2) * 36));
            }

            //adiciona o index manulamente
            slots[0].Index = (int)EquipSlotType.Weapon;
            slots[1].Index = (int)EquipSlotType.Head;
            slots[2].Index = (int)EquipSlotType.EarringLeft;
            slots[3].Index = (int)EquipSlotType.Chest;
            slots[4].Index = (int)EquipSlotType.Pants;
            slots[5].Index = (int)EquipSlotType.RingLeft;
            slots[6].Index = (int)EquipSlotType.Legs;
            slots[7].Index = (int)EquipSlotType.Shield;
            slots[8].Index = (int)EquipSlotType.Necklace;
            slots[9].Index = (int)EquipSlotType.EarringRight;
            slots[10].Index = (int)EquipSlotType.Shoulder;
            slots[11].Index = (int)EquipSlotType.Gloves;
            slots[12].Index = (int)EquipSlotType.RingRight;
            slots[13].Index = (int)EquipSlotType.Belt;

            for (var n = 0; n < MAX_EQUIPPED_ITEM; n++) EquippedItem[n] = new Item();
        }

        /// <summary>
        /// Desenha o controle.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            background.Draw();

            EngineFont.DrawText(Client.PlayerLocal.Name + " Lv. " + Client.PlayerLocal.Level, new Size2(320, 20), new Point(Position.X, Position.Y + 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center, true);

            closebutton.Draw();

            DrawSlots();

            for (var n = 0; n < MAX_LABEL; n++) {
                labels[n].DrawTextCenter();
                labels[n].MouseButtons();
            }

            switch (SelectedPage) {
                case 0:
                    DrawStats();
                    break;
                case 1:
                    ShowVitalStats();
                    break;
                case 2:
                    ShowPhysicalStats();
                    break;
                case 3:
                    ShowMagicStats();
                    break;
                case 4:
                    ShowElementalStats();
                    break;
                case 5:
                    ShowUniqueStats();
                    break;
                case 6:
                    ShowResistStats();
                    break;
            }
        }

        private static void DrawSlots() {
            for (int n = 0; n < MAX_SLOT; n++) {
                slots[n].MouseButtons();
                slots[n].Draw(slot);

                if (Icons[n] > 0) {
                    EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                    EngineCore.SpriteDevice.Draw(EngineTexture.FindTextureByID(Icons[n], EngineTextureType.Icons), Color.White, null, null, new Vector3(slots[n].Position.X + 4, slots[n].Position.Y + 4, 0));
                    EngineCore.SpriteDevice.End();
                }
            }
        }

        private static void DrawStats() {
            for (var n = 0; n < MAX_BUTTON; n++) {
                buttons[n].Draw();
            }

            EngineFont.DrawText("Força", Position.X + 100, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Strenght + "", Position.X + 210, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Destreza", Position.X + 100, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Dexterity + "", Position.X + 210, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Agilidade", Position.X + 100, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Agility + "", Position.X + 210, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Constituição", Position.X + 100, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Constitution + "", Position.X + 210, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Inteligência", Position.X + 100, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Intelligence + "", Position.X + 210, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Sabedoria", Position.X + 100, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Wisdom + "", Position.X + 210, Position.Y + 170, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Vontade", Position.X + 100, Position.Y + 190, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Will + "", Position.X + 210, Position.Y + 190, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Mente", Position.X + 100, Position.Y + 210, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Mind + "", Position.X + 210, Position.Y + 210, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Points", Position.X + 100, Position.Y + 250, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Points + "", Position.X + 210, Position.Y + 250, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowVitalStats() {
            EngineFont.DrawText("HP", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MaxHP + " / " + Client.PlayerLocal.MaxHP, Position.X + 120, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("MP", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MP + " / " + Client.PlayerLocal.MaxMP, Position.X + 120, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("SP", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.SP + " / " + Client.PlayerLocal.MaxSP, Position.X + 120, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Regen HP", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.RegenHP + "", Position.X + 170, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Regen MP", Position.X + 70, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.RegenMP + "", Position.X + 170, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Regen SP", Position.X + 70, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.RegenSP + "", Position.X + 170, Position.Y + 170, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowPhysicalStats() {
            EngineFont.DrawText("Ataque", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Attack + "", Position.X + 205, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Precisão", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Accuracy + "", Position.X + 205, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Defesa", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Defense + "", Position.X + 205, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Evasão", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Evasion + "", Position.X + 205, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Bloqueio (Escudo)", Position.X + 70, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Block + "", Position.X + 205, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Bloqueio (Arma)", Position.X + 70, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Parry + "", Position.X + 205, Position.Y + 170, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Taxa Crítica", Position.X + 70, Position.Y + 190, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.CriticalRate + "", Position.X + 205, Position.Y + 190, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Dano Crítico", Position.X + 70, Position.Y + 210, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.CriticalDamage + "", Position.X + 205, Position.Y + 210, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Vel. Ataque", Position.X + 70, Position.Y + 230, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttackSpeed + "", Position.X + 205, Position.Y + 230, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowMagicStats() {
            EngineFont.DrawText("Ataque Mágico", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicAttack + "", Position.X + 205, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Precisão Mágica", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicAccuracy + "", Position.X + 205, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Defesa Mágica", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicDefense + "", Position.X + 205, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Resistência Mág.", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicResist + "", Position.X + 205, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Taxa Crítica Mág.", Position.X + 70, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicCriticalRate + "", Position.X + 205, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Dano Crítico Mág.", Position.X + 70, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.MagicCriticalDamage + "", Position.X + 205, Position.Y + 170, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Vel. Conjuração", Position.X + 70, Position.Y + 190, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.CastSpeed + "", Position.X + 205, Position.Y + 190, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowElementalStats() {
            EngineFont.DrawText("Elemento Fogo", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeFire + "", Position.X + 205, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Elemento Água", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeWater + "", Position.X + 205, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Elemento Terra", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeEarth + "", Position.X + 205, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Elemento Ar", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeWind + "", Position.X + 205, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Elemento Luz", Position.X + 70, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeLight + "", Position.X + 205, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Elemento Trevas", Position.X + 70, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.AttributeDark + "", Position.X + 205, Position.Y + 170, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowUniqueStats() {
            EngineFont.DrawText("Concentração", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Concentration + "", Position.X + 205, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Poder de Cura", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.HealingPower + "", Position.X + 205, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Inimizade", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.Enmity + "", Position.X + 205, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Supressão de Dano", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.DamageSuppression + "", Position.X + 205, Position.Y + 130, Color.White, EngineFontStyle.Regular);
        }

        private static void ShowResistStats() {
            EngineFont.DrawText("Res. Atordoamento", Position.X + 70, Position.Y + 70, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistStun + "", Position.X + 205, Position.Y + 70, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Paralisia", Position.X + 70, Position.Y + 90, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistParalysis + "", Position.X + 205, Position.Y + 90, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Silêncio", Position.X + 70, Position.Y + 110, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistSilence + "", Position.X + 205, Position.Y + 110, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Cegueira", Position.X + 70, Position.Y + 130, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistBlind + "", Position.X + 205, Position.Y + 130, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Taxa Crítica", Position.X + 70, Position.Y + 150, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistCriticalRate + "", Position.X + 205, Position.Y + 150, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Dano Crítico", Position.X + 70, Position.Y + 170, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistCriticalDamage + "", Position.X + 205, Position.Y + 170, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Taxa Crít. Mág.", Position.X + 70, Position.Y + 190, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistMagicCriticalRate + "", Position.X + 205, Position.Y + 190, Color.White, EngineFontStyle.Regular);

            EngineFont.DrawText("Res. Dano Crít. Mág.", Position.X + 70, Position.Y + 210, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText(Client.PlayerLocal.ResistMagicCriticalDamage + "", Position.X + 205, Position.Y + 210, Color.White, EngineFontStyle.Regular);
        }

        /// <summary>
        /// Atualiza os ícones nos slots.
        /// </summary>
        public static void UpdateSlotIcons() {
            var inv_slot = 0;

            for (var n = 0; n < MAX_EQUIPPED_ITEM; n++) {
                inv_slot = GetSlotIndex((EquipSlotType)n);

                if (EquippedItem[n].ID > 0) {
                    slots[inv_slot].IconID = DataManager.FindItemByID(EquippedItem[n].ID).IconID;
                }
                else {
                    slots[inv_slot].IconID = 0;
                }
            }
        }

        /// <summary>
        /// Obtem o índice do slot pelo tipo de item.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetSlotIndex(EquipSlotType type) {
            if (type == EquipSlotType.Weapon) return 0;
            if (type == EquipSlotType.Head) return 1;
            if (type == EquipSlotType.EarringLeft) return 2;
            if (type == EquipSlotType.Chest) return 3;
            if (type == EquipSlotType.Pants) return 4;
            if (type == EquipSlotType.RingLeft) return 5;
            if (type == EquipSlotType.Legs) return 6;
            if (type == EquipSlotType.Shield) return 7;
            if (type == EquipSlotType.Necklace) return 8;
            if (type == EquipSlotType.EarringRight) return 9;
            if (type == EquipSlotType.Shoulder) return 10;
            if (type == EquipSlotType.Gloves) return 11;
            if (type == EquipSlotType.RingRight) return 12;
            if (type == EquipSlotType.Belt) return 13;

            return -1;
        }

        /// <summary>
        /// Fecha a janela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseButton_MouseUp(object sender, EngineEventArgs e) {
            Visible = false;
        }

        /// <summary>
        /// Seleciona o stat de acordo com a página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Labels_MouseUp(object sender, EngineEventArgs e) {
            SelectedPage = ((EngineLabel)sender).Index;

            ((EngineLabel)sender).TextFontStyle = EngineFontStyle.Bold;

            for (int n = 0; n < MAX_LABEL; n++) {
                if (SelectedPage == n) continue;
                labels[n].TextFontStyle = EngineFontStyle.Regular;
            }
        }

        /// <summary>
        /// Remove um item do slot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slots_MouseUp(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (e.Left) {
                //se já houver algum item selecionado
                if (WindowSelectedItem.ObjectID > 0) {

                    #region Inventory -> CharacterStatus
                    //se o item vier do inventario
                    if (WindowSelectedItem.ObjectSource == Window.Inventory) {

                        var result = false;

                        if ((EquipSlotType)index <= EquipSlotType.Legs) {
                            if (WindowSelectedItem.ObjectType <= ItemType.Legs) result = true;
                        }

                        if ((EquipSlotType)index == EquipSlotType.EarringLeft || (EquipSlotType)index == EquipSlotType.EarringRight) {
                            if (WindowSelectedItem.ObjectType == ItemType.Earring) result = true;
                        }

                        if ((EquipSlotType)index == EquipSlotType.RingLeft || (EquipSlotType)index == EquipSlotType.RingRight) {
                            if (WindowSelectedItem.ObjectType == ItemType.Ring) result = true;
                        }

                        if ((EquipSlotType)index == EquipSlotType.Belt) {
                            if (WindowSelectedItem.ObjectType == ItemType.Belt) result = true;
                        }

                        if (result) {
                            //swap items from inv, to equip   
                            InventoryPacket.UseInventoryItem((short)WindowSelectedItem.InventorySlot);
                        }
                        else {
                            //devolve o ícone para o inventário
                            WindowInventory.Inventory[WindowSelectedItem.InventorySlot].IconID = WindowSelectedItem.ObjectIcon;
                            WindowInventory.UpdateSlotIcons();
                        }

                        WindowSelectedItem.Close();
                    }
                    #endregion

                    #region CharacterStatus -> CharacterStatus
                    //se vier do mesmo lugar, não faz nada, apenas muda o ícone
                    if (WindowSelectedItem.ObjectSource == Window.CharacterStatus) {

                        if (WindowSelectedItem.ObjectSlot == (EquipSlotType)index) {
                            slots[GetSlotIndex((EquipSlotType)index)].IconID = WindowSelectedItem.ObjectIcon;
                        }
                        else {
                            slots[GetSlotIndex(WindowSelectedItem.ObjectSlot)].IconID = WindowSelectedItem.ObjectIcon;
                        }

                        WindowSelectedItem.Close();
                    }
                    #endregion
                }
                else {
                    //se não houver nada, pega um item  
                    if (EquippedItem[index].ID > 0) {
                        WindowSelectedItem.Show(EquippedItem[index].ID, (EquipSlotType)index, Window.CharacterStatus, index, slots[GetSlotIndex((EquipSlotType)index)].IconID);

                        //remove o ícone
                        slots[GetSlotIndex((EquipSlotType)index)].IconID = 0;
                    }
                }
            }

            //usa o item, botão direito
            if (e.Right) {
                //se um item estiver selecionado, retorna
                if (WindowSelectedItem.ObjectID > 0) { return; }

                //quando o botão direito é usado, sempre move para o slot número 0 do inventário
                if (EquippedItem[index].ID > 0) InventoryPacket.UnequipItem((EquipSlotType)index, 0);
                WindowViewItem.Visible = false;
            }
        }

        /// <summary>
        /// Abre a janela de exibição do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slots_MouseMove(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (EquippedItem[index].ID == 0) return;

            WindowViewItem.CopyItem(EquippedItem[index]);
            WindowViewItem.Visible = true;
        }

        /// <summary>
        /// Esconde a janela de exibição do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slots_MouseLeave(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (EquippedItem[index].ID == WindowViewItem.ObjectID) {
                WindowViewItem.Visible = false;
            }
        }

        /// <summary>
        /// Aumenta o atributo selecionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void StatButtons_MouseUp(object sender, EngineEventArgs e) {
            var index = ((EngineButton)sender).Index;
            GamePacket.IncrementStat((byte)index);
        }
    }
}