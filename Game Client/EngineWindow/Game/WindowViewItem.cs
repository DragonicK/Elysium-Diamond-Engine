using System;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowViewItem {
        private static EngineObject background;
        public static Point Position;

        public static int ObjectID { get; set; }
        /// <summary>
        /// Textura de Slot.
        /// </summary>
        private static Texture slot;

        /// <summary>
        /// Visibilidade da janela.
        /// </summary>
        public static bool Visible { get; set; } = false;

        /// <summary>
        /// Referência do item a ser mostrado.
        /// </summary>
        private static Item _item;

        #region Textos
        private static string type;
        private static string hand;
        private static string rarity_text;
        private static string tradeable;
        private static string soulbound;
        private static Color rarity_color;
        private static string level;

        private static byte Expire { get; set; }
        private static DateTime ExpireDate { get; set; }
        private static short Durability { get; set; }
        #endregion

        /// <summary>
        /// Inicializa os controles.
        /// </summary>
        public static void Initialize() {
            Position = new Point(322,  88);

            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            background = new EngineObject($"{AppData}/Elysium/Data/Graphics/stats_back.png");
            background.Size = new Size2(380, 544);
            background.Position = Position;
            background.SourceRect = new Rectangle(0, 0, 369, 536);

            slot = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot.png");
        }

         /// <summary>
        /// Copia a referência do item.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="item_durability">Durabilidade do item a ser exibido.</param>
        public static void CopyItem(Item item) { //int itemID, short durability = -1, short enchant = 0, byte ) {
            _item = DataManager.FindItemByID(item.ID);

            ObjectID = item.ID;

            Durability = item.Durability;
            Expire = item.Expire;
            ExpireDate = item.ExpireDate;

            if (_item.Type == ItemType.Weapon) {
                hand = (_item.Hand == 0) ? "Item de uma mão" : "Item de duas mãos";
            } else {
                hand = string.Empty;
            }

            if (item.Enchant > 0) {
                level = " +" + item.Enchant.ToString();
            } else {
                level = string.Empty;
            }
   
            soulbound = (item.SoulBound == 1) ? "Item ligado ao personagem" : "Item não ligado ao personagem";
            tradeable = (item.Tradeable == 1) ? "Negociável" : "Inegociável";
        
            #region Rarity
            switch (_item.Rarity) {
                case ItemRarity.Poor:
                    rarity_text = "Baixa qualidade";
                    rarity_color = Color.DimGray;
                    break;
                case ItemRarity.Common:
                    rarity_text = "Comum";
                    rarity_color = Color.White;
                    break;
                case ItemRarity.Uncommon:
                    rarity_text = "Incomum";
                    rarity_color = Color.SpringGreen;
                    break;
                case ItemRarity.Rare:
                    rarity_text = "Raro";
                    rarity_color = Color.RoyalBlue;
                    break;
                case ItemRarity.Epic:
                    rarity_text = "Épico";
                    rarity_color = Color.MediumVioletRed;
                    break;
                case ItemRarity.Legendary:
                    rarity_text = "Lendário";
                    rarity_color = Color.Orange;
                    break;
                case ItemRarity.Mythic:
                    rarity_text = "Mítico";
                    rarity_color = Color.BlueViolet;
                    break;
                case ItemRarity.Artifact:
                    rarity_text = "Artefato";
                    rarity_color = Color.Salmon;
                    break;
                case ItemRarity.Ethereal:
                    rarity_text = "Etéreo";
                    rarity_color = Color.Crimson;
                    break;
            }
            #endregion

            #region Type
            switch (_item.Type) {
                case ItemType.Weapon:
                    type = "Arma";
                    break;
                case ItemType.Shield:
                    type = "Escudo";
                    break;
                case ItemType.Head:
                    type = "Cabeça";
                    break;
                case ItemType.Chest:
                    type = "Peito";
                    break;
                case ItemType.Gloves:
                    type = "Luvas";
                    break;
                case ItemType.Shoulder:
                    type = "Ombro";
                    break;
                case ItemType.Pants:
                    type = "Calça";
                    break;
                case ItemType.Legs:
                    type = "Bota";
                    break;
                case ItemType.Belt:
                    type = "Cinto";
                    break;
                case ItemType.Earring:
                    type = "Brinco";
                    break;
                case ItemType.Ring:
                    type = "Anel";
                    break;
                case ItemType.Necklace:
                    type = "Colar";
                    break;
            }
            #endregion
        }

        /// <summary>
        /// Desenha os controles.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            background.Draw();

            EngineFont.DrawText(_item.Name + level, new Rectangle(Position.X, Position.Y + 15, 380, 20), rarity_color, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText(type, new Rectangle(Position.X + 25, Position.Y + 35, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText(rarity_text, new Rectangle(Position.X, Position.Y + 35, 350, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Right);
            EngineFont.DrawText("Durabilidade " + Durability + " / " + _item.Durability, new Rectangle(Position.X + 25, Position.Y + 55, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText(hand, new Rectangle(Position.X, Position.Y + 55, 350, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Right);
            EngineFont.DrawText("Pode ser usado no level " + _item.Level, new Rectangle(Position.X + 25, Position.Y + 75, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText(soulbound, new Rectangle(Position.X + 25, Position.Y + 95, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            EngineFont.DrawText(tradeable, new Rectangle(Position.X, Position.Y + 95, 350, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Right);
            if (Expire > 1) EngineFont.DrawText(ExpireDate.ToString("dd/MM/yyyy hh:mm:ss"), new Rectangle(Position.X, Position.Y + 500, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);

            //Desenha todos os stats do item
            for (var n = 1; n <= 16; n++) {
                EngineFont.DrawText(_item.Text[n - 1], new Rectangle(Position.X + 25, Position.Y + ((n - 1) * 20) + 135, 380, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Left);
            }

            DrawSlots();
        }
        
        /// <summary>
        /// Desenha os slots do item.
        /// </summary>
        public static void DrawSlots() {
            for (var n = 0; n < 9; n++) {
                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                EngineCore.SpriteDevice.Draw(slot, Color.White, null, null, new Vector3(Position.X + 20 + (n * 36), Position.Y + 455, 0));
                EngineCore.SpriteDevice.End();
            }
        }
    }
}