using Elysium_Diamond.DirectX;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowInventory {
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
        /// Item do usuário.
        /// </summary>
        public static Item[] Inventory { get; set; } = new Item[MAX_SLOT];

        /// <summary>
        /// Quantidade de slot na horizontal.
        /// </summary>
        private const int MAX_X = 8;

        /// <summary>
        /// Quantidade de slot na vertical.
        /// </summary>
        private const int MAX_Y = 7;

        /// <summary>
        /// Quantitade total de slots.
        /// </summary>
        private const int MAX_SLOT = 56;

        private static EngineSlot[] slots = new EngineSlot[MAX_SLOT];
        private static EngineObject background;
        private static EngineButton closebutton;
        private static Texture slot;

        /// <summary>
        /// Inicializa as configurações da janela.
        /// </summary>
        public static void Initialize() {
            Position = new Point(680, 170);

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(320, 352);
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/inventory.png", 320, 352);
            background.SourceRect = new Rectangle(0, 0, 320, 352);

            closebutton = new EngineButton("closex", 32, 32);
            closebutton.Position = new Point(Position.X + 277, Position.Y + 8);
            closebutton.BorderRect = new Rectangle(8, 8, 16, 16);
            closebutton.SourceRect = new Rectangle(0, 0, 32, 32);
            closebutton.Size = new Size2(32, 32);
            closebutton.MouseUp += CloseButton_MouseUp;

            slot = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot.png", 40, 40);

            var n = 0;
            for (int y = 0; y < MAX_Y; y++) {
                for (int x = 0; x < MAX_X; x++) {
                    Inventory[n] = new Item();

                    slots[n] = new EngineSlot();
                    slots[n].Index = n;
                    slots[n].MouseRight = true;
                    slots[n].BorderRect = new Rectangle(6, 6, 34, 34);
                    slots[n].SourceRect = new Rectangle(0, 0, 40, 40);
                    slots[n].Position = new Point(Position.X + 13 + (x * 36), Position.Y + 35 + (y * 36));
                    slots[n].MouseMove += Slot_MouseMove;
                    slots[n].MouseLeave += Slot_MouseLeave;
                    slots[n].MouseUp += Slot_MouseUp;

                    n++;
                }
            }
        }

        /// <summary>
        /// Desenha a janela.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            background.Draw();
            closebutton.Draw();

            for (var n = 0; n < MAX_SLOT; n++) {
                slots[n].MouseButtons();
                slots[n].Draw(slot);
            }

            EngineFont.DrawText("$: " + GameClient.Client.PlayerLocal.Currency.ToString("n0"), new Rectangle(Position.X + 15, Position.Y + 310, 280, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Right, true);
        }

        /// <summary>
        /// Atualiza os ícones nos slots.
        /// </summary>
        public static void UpdateSlotIcons() {
            for (var n = 0; n < MAX_SLOT; n++) {
                if (Inventory[n].ID > 0) {
                    slots[n].IconID = DataManager.FindItemByID(Inventory[n].ID).IconID;
                } 
                else {
                    slots[n].IconID = 0;
                }
            }
        }

        /// <summary>
        /// Utiliza um item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slot_MouseUp(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (e.Left) {
                //se já houver algum item selecionado, swapinvitems
                if (WindowSelectedItem.ObjectID > 0) {

                    #region Inventory -> Inventory
                    if (WindowSelectedItem.ObjectSource == Window.Inventory) {
                        InventoryPacket.SwapInventoryItem((short)WindowSelectedItem.InventorySlot, (short)index);
                        //fecha o item
                        WindowSelectedItem.Close();
                        return;
                    }
                    #endregion

                    #region CharacterStatus -> Inventory 
                    if (WindowSelectedItem.ObjectSource == Window.CharacterStatus) {

                        //se o slot estiver vazio, desequipa
                        if (Inventory[index].ID == 0) {
                            InventoryPacket.UnequipItem(WindowSelectedItem.ObjectSlot, (short)index);
                        }
                        else {
                            //se não estiver vazio, e o conteúdo for do mesmo tipo, equipa
                            if (WindowSelectedItem.ObjectType == Inventory[index].Type) {
                                InventoryPacket.UseInventoryItem((short)index);
                            }
                            //se não for do mesmo tipo, desequipa e coloca em algum slot
                            else {
                               InventoryPacket.UnequipItem(WindowSelectedItem.ObjectSlot, (short)index);
                            }
                        }

                        WindowSelectedItem.Close();
                        return;
                    }
                    #endregion
                }
                //se não houver nada, pega um item                     
                else {
                    if (Inventory[index].ID > 0) {
                        ItemType type = DataManager.FindItemByID(Inventory[index].ID).Type;
                        WindowSelectedItem.Show(Inventory[index].ID, type, Window.Inventory, index, slots[index].IconID);

                        //remove o ícone
                        slots[index].IconID = 0;
                    }
                }
            }         

            //usa o item, botão direito
            if (e.Right) {
                //se um item estiver selecionado, retorna
                if (WindowSelectedItem.ObjectID > 0) { return; }

                if (Inventory[index].ID > 0) InventoryPacket.UseInventoryItem((short)index);
                WindowViewItem.Visible = false;
            }
        }

        /// <summary>
        /// Abre a janela de exibição do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slot_MouseMove(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (Inventory[index].ID > 0) {
                WindowViewItem.CopyItem(Inventory[index]);
                WindowViewItem.Visible = true;
            }
        }

        /// <summary>
        /// Esconde a janela de exibição do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Slot_MouseLeave(object sender, EngineEventArgs e) {
            var index = ((EngineSlot)sender).Index;

            if (Inventory[index].ID == WindowViewItem.ObjectID) {
                WindowViewItem.Visible = false;
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
    }
}