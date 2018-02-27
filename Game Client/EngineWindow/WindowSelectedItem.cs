using Elysium_Diamond.DirectX;
using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowSelectedItem {
        /// <summary>
        /// Mostra ou esconde a janela.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        /// Obtem ou altera o ID do item selecionado.
        /// </summary>
        public static int ObjectID { get; set; }

        /// <summary>
        /// Obtem ou altera o tipo de item selecionado.
        /// </summary>
        public static ItemType ObjectType { get; set; }

        /// <summary>
        /// Slot de equipamento.
        /// </summary>
        public static EquipSlotType ObjectSlot { get; set; }

        /// <summary>
        /// Obtem ou altera a origem do objeto (inventário, etc ...)
        /// </summary>
        public static Window ObjectSource { get; set; }

        /// <summary>
        /// Obtem ou altera o ícone do item selecionado.
        /// </summary>
        public static int ObjectIcon {
            get {
                return slot.IconID;
            }
            set {
                slot.IconID = value;
            }
        }

        /// <summary>
        /// Obtem ou altera o slot do inventário do item.
        /// </summary>
        public static int InventorySlot { get; set; }

        /// <summary>
        /// Posição do controle na tela.
        /// </summary>
        public static Point Position {
            get {
                return slot.Position;
            }
            set {
                slot.Position = value;
            }
        }

        //slot de fundo
        private static EngineSlot slot;
        private static Texture texture;

        /// <summary>
        /// Inicializa as configurações do controle.
        /// </summary>
        public static void Initialize() {
            slot = new EngineSlot();
            slot.Size = new Size2(40, 40);
            slot.BorderRect = new Rectangle(6, 6, 34, 34);
            slot.SourceRect = new Rectangle(0, 0, 40, 40);
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot.png", 40, 40);
        }

        /// <summary>
        /// Desenha o controle.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            slot.Draw(texture);
        }

        /// <summary>
        /// Exibe o controle.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="objectIcon"></param>
        public static void Show(int objectID, ItemType objectType, Window source, int invslot, int objectIcon = 0) {
            ObjectID = objectID;
            ObjectType = objectType;
            ObjectSource = source;
            ObjectIcon = objectIcon;
            InventorySlot = invslot;
            Visible = true;
        }

        /// <summary>
        /// Exibe o controle.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="objectIcon"></param>
        public static void Show(int objectID, EquipSlotType slotType, Window source, int invslot, int objectIcon = 0) {
            ObjectID = objectID;
            ObjectSlot = slotType;
            ObjectSource = source;
            ObjectIcon = objectIcon;
            InventorySlot = invslot;
            Visible = true;
        }

        /// <summary>
        /// Exibe o controle.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="objectIcon"></param>
        public static void Show(int objectID, ItemType objectType, EquipSlotType slotType, Window source, int invslot, int objectIcon = 0) {
            ObjectID = objectID;
            ObjectType = objectType;
            ObjectSlot = slotType;
            ObjectSource = source;
            ObjectIcon = objectIcon;
            InventorySlot = invslot;
            Visible = true;
        }

        /// <summary>
        /// Fecha o controle.
        /// </summary>
        public static void Close() {
            ObjectIcon = 0;
            ObjectID = 0;           
            InventorySlot = 0;
            Visible = false;
        }
    }
}