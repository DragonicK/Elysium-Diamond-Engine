using System;
using System.Collections.Generic;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.GameClient;
using Elysium_Diamond.Network;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowCash {
        /// <summary>
        /// Quantidade máxima de botões na janela de descrição.
        /// </summary>
        const byte MAX_BUY_BUTTON = 4;
        /// <summary>
        /// Quantidade máxima de item.
        /// </summary>
        const byte MAX_ITEM = 6;

        /// <summary>
        /// Quantidade máxima de botões.
        /// </summary>
        const byte MAX_BUTTON = 16;

        /// <summary>
        /// Items para exibição
        /// </summary>
        public class CashItem {
            public int CashItemID; //id da loja
            public int Price;
            public short Quantity;
            public short Enchant;
            public short Durability;
            public byte Tradeable;
            public byte SoulBound;
            public Item Item;
           //public string Name = string.Empty;

            public CashItem() {
                Item = new Item();
            }

            public void Clear() {
                CashItemID = 0;
                Item = null;
                Quantity = 0;
                Price = 0;
                //Name = string.Empty;
            }
        }
           
        /// <summary>
        /// Posição da janela.
        /// </summary>
        public static Point Position;

        /// <summary>
        /// Visibilidade da janela.
        /// </summary>
        public static bool Visible;

        /// <summary>
        /// Textura do slot do item.
        /// </summary>
        private static Texture slot;

        /// <summary>
        /// Seleção dos objetos.
        /// </summary>
        private static byte[] SelectedTexture = new byte[MAX_ITEM];

        /// <summary>
        /// Textura de fundo da janela do item.
        /// </summary>
        private static Texture[] cash_item_background = new Texture[2];
        private static EngineObject background;

        private static EngineObject[] cash_item_back = new EngineObject[MAX_ITEM];
        private static EngineObject[] cash_item_slot = new EngineObject[MAX_ITEM];
        private static EngineButton[] button = new EngineButton[MAX_BUTTON];

        /// <summary>
        /// items de exibição
        /// </summary>
        public static CashItem[] Items = new CashItem[MAX_ITEM];

        /// <summary>
        /// Página selecionada.
        /// </summary>
        public static byte Page = 1;

        /// <summary>
        /// Quantidade máxima de páginas.
        /// </summary>
        public static byte MaxPage = 1;

        /// <summary>
        /// Categoria selecionada.
        /// </summary>
        public static CashShopItemCategory Category = CashShopItemCategory.Promo;

        /// <summary>
        /// Bloqueia os controles na espera dos dados.
        /// </summary>
        public static bool WaitData = false;

        #region Buy Item 
        public static bool BuyItemVisible = false;
        private static Point BuyItemPosition;
        private static EngineButton[] buy_item_button = new EngineButton[MAX_BUY_BUTTON];
        /// <summary>
        /// Textura de fundo.
        /// </summary>
        private static EngineObject buy_item_background;
        /// <summary>
        /// Slot de item.
        /// </summary>
        private static EngineObject buy_item_slot;
        public static EngineTextBox textbox;
        public static short Quantity = 1;

        /// <summary>
        /// Item selecionado na loja.
        /// </summary>
        public static int SelectedItem;

        ///####### item detail ########
        public static byte Tradeable;
        public static byte SoulBound;
        public static bool GiftEnabled;
        public static short BuyLimit;
        public static int ExpireDays;
        //#############################

        /// <summary>
        /// Estrutura de textos de descrição.
        /// </summary>
        private struct TextData {
            public string Text;
            public Color Color;
        }

        //Lista de descrições.
        private static List<TextData> itemText = new List<TextData>();
        #endregion

        /// <summary>
        /// Inicializa as configurações.
        /// </summary>
        public static void Initialize() {
            //posicionado no centro da tela
            Position = new Point(224, 150);
            Visible = false;

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(576, 416);
            background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/window_cashshop.png");
            background.SourceRect = new Rectangle(0, 0, 576, 416);
    
            cash_item_background[0] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/selectchar_charback.png");
            cash_item_background[1] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/selectchar_selected.png");
            slot = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/slot.png");

            for (var n = 0; n < MAX_ITEM; n++) {
                Items[n] = new CashItem();

                cash_item_back[n] = new EngineObject();
                cash_item_back[n].Index = n;
                cash_item_back[n].Size = new Size2(127, 134);
                cash_item_back[n].SourceRect = new Rectangle(0, 0, 127, 134);
                cash_item_back[n].BorderRect = new Rectangle(0, 0, 127, 134);
                cash_item_back[n].MouseUp += CashItemBack_MouseUp;
                cash_item_back[n].MouseMove += CashItemBack_MouseMove;
                cash_item_back[n].MouseLeave += CashItemBack_MouseLeave;

                cash_item_slot[n] = new EngineObject();
                cash_item_slot[n].Index = n;
                cash_item_slot[n].Size = new Size2(40, 40);
                cash_item_slot[n].SourceRect = new Rectangle(0, 0, 40, 40);
                cash_item_slot[n].BorderRect = new Rectangle(0, 0, 40, 40);
                cash_item_slot[n].MouseMove += CashItemSlot_MouseMove;
                cash_item_slot[n].MouseLeave += CashItemSlot_MouseLeave;
            }

            cash_item_back[0].Position = new Point(Position.X + 160, Position.Y + 82);
            cash_item_back[1].Position = new Point(Position.X + 287, Position.Y + 82);
            cash_item_back[2].Position = new Point(Position.X + 414, Position.Y + 82);
            cash_item_back[3].Position = new Point(Position.X + 160, Position.Y + 216);
            cash_item_back[4].Position = new Point(Position.X + 287, Position.Y + 216);
            cash_item_back[5].Position = new Point(Position.X + 414, Position.Y + 216);

            cash_item_slot[0].Position = new Point(Position.X + 205, Position.Y + 112);
            cash_item_slot[1].Position = new Point(Position.X + 332, Position.Y + 112);
            cash_item_slot[2].Position = new Point(Position.X + 459, Position.Y + 112);
            cash_item_slot[3].Position = new Point(Position.X + 205, Position.Y + 246);
            cash_item_slot[4].Position = new Point(Position.X + 332, Position.Y + 246);
            cash_item_slot[5].Position = new Point(Position.X + 459, Position.Y + 246);

            #region Buttons
            button[0] = new EngineButton("promo_green", 128, 32);
            button[0].Position = new Point(Position.X + 133, Position.Y + 45);
            button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);

            button[1] = new EngineButton("boost_green", 128, 32);
            button[1].Position = new Point(Position.X + 235, Position.Y + 45);
            button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);

            button[2] = new EngineButton("style_green", 128, 32);
            button[2].Position = new Point(Position.X + 337, Position.Y + 45);
            button[2].BorderRect = new Rectangle(20, 2, 86, 26);
            button[2].SourceRect = new Rectangle(0, 0, 128, 32);

            button[3] = new EngineButton("supply_green", 128, 32);
            button[3].Position = new Point(Position.X + 439, Position.Y + 45);
            button[3].BorderRect = new Rectangle(20, 2, 86, 26);
            button[3].SourceRect = new Rectangle(0, 0, 128, 32);

            button[4] = new EngineButton("weapon", 128, 32);
            button[4].Position = new Point(Position.X + 25, Position.Y + 80);
            button[4].BorderRect = new Rectangle(7, 2, 112, 26);
            button[4].SourceRect = new Rectangle(0, 0, 128, 32);

            button[5] = new EngineButton("shield", 128, 32);
            button[5].Position = new Point(Position.X + 25, Position.Y + 110);
            button[5].BorderRect = new Rectangle(7, 2, 112, 26);
            button[5].SourceRect = new Rectangle(0, 0, 128, 32);

            button[6] = new EngineButton("gear", 128, 32);
            button[6].Position = new Point(Position.X + 25, Position.Y + 140);
            button[6].BorderRect = new Rectangle(7, 2, 112, 26);
            button[6].SourceRect = new Rectangle(0, 0, 128, 32);

            button[7] = new EngineButton("acessory", 128, 32);
            button[7].Position = new Point(Position.X + 25, Position.Y + 170);
            button[7].BorderRect = new Rectangle(7, 2, 112, 26);
            button[7].SourceRect = new Rectangle(0, 0, 128, 32);

            button[8] = new EngineButton("jewel", 128, 32);
            button[8].Position = new Point(Position.X + 25, Position.Y + 200);
            button[8].BorderRect = new Rectangle(7, 2, 112, 26);
            button[8].SourceRect = new Rectangle(0, 0, 128, 32);

            button[9] = new EngineButton("potion", 128, 32);
            button[9].Position = new Point(Position.X + 25, Position.Y + 230);
            button[9].BorderRect = new Rectangle(7, 2, 112, 26);
            button[9].SourceRect = new Rectangle(0, 0, 128, 32);

            button[10] = new EngineButton("food", 128, 32);
            button[10].Position = new Point(Position.X + 25, Position.Y + 260);
            button[10].BorderRect = new Rectangle(7, 2, 112, 26);
            button[10].SourceRect = new Rectangle(0, 0, 128, 32);

            button[11] = new EngineButton("scroll", 128, 32);
            button[11].Position = new Point(Position.X + 25, Position.Y + 290);
            button[11].BorderRect = new Rectangle(7, 2, 112, 26);
            button[11].SourceRect = new Rectangle(0, 0, 128, 32);

            button[12] = new EngineButton("service", 128, 32);
            button[12].Position = new Point(Position.X + 25, Position.Y + 320);
            button[12].BorderRect = new Rectangle(7, 2, 112, 26);
            button[12].SourceRect = new Rectangle(0, 0, 128, 32);

            button[13] = new EngineButton("previous", 128, 32);
            button[13].Position = new Point(Position.X + 195, Position.Y + 355);
            button[13].BorderRect = new Rectangle(20, 2, 86, 26);
            button[13].SourceRect = new Rectangle(0, 0, 128, 32);

            button[14] = new EngineButton("next", 128, 32);
            button[14].Position = new Point(Position.X + 380, Position.Y + 355);
            button[14].BorderRect = new Rectangle(20, 2, 86, 26);
            button[14].SourceRect = new Rectangle(0, 0, 128, 32);

            button[15] = new EngineButton("end_green", 128, 32);
            button[15].Position = new Point(Position.X + 25, Position.Y + 355);
            button[15].BorderRect = new Rectangle(20, 2, 86, 26);
            button[15].SourceRect = new Rectangle(0, 0, 128, 32);
            #endregion

            for (var n = 0; n < MAX_BUTTON; n++) {
                button[n].Index = n;
                button[n].MouseUp += Buttons_MouseUp;
            }

            InitializeBuyItem();
        }

        /// <summary>
        /// Inicializa as configurações da janela de item.
        /// </summary>
        private static void InitializeBuyItem() {
            BuyItemVisible = false;
            BuyItemPosition = new Point(Position.X + 128, Position.Y + 45); 

            buy_item_background = new EngineObject();
            buy_item_background.Position = BuyItemPosition;
            buy_item_background.Size = new Size2(320, 352);
            buy_item_background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/cash_buy.png");
            buy_item_background.SourceRect = new Rectangle(0, 0, 320, 352);

            buy_item_slot = new EngineObject();
            buy_item_slot.Size = new Size2(40, 40);
            buy_item_slot.Position = new Point(BuyItemPosition.X + 140, BuyItemPosition.Y + 40);
            buy_item_slot.SourceRect = new Rectangle(0, 0, 40, 40);
            buy_item_slot.BorderRect = new Rectangle(0, 0, 40, 40);
            buy_item_slot.MouseMove += BuyItemSlot_MouseMove;
            buy_item_slot.MouseLeave += BuyItemSlot_MouseLeave;

            buy_item_button[0] = new EngineButton("purchase", 128, 32);
            buy_item_button[0].Position = new Point(BuyItemPosition.X + 20, BuyItemPosition.Y + 275);
            buy_item_button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            buy_item_button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            buy_item_button[0].MouseUp += BuyButton_MouseUp;

            buy_item_button[1] = new EngineButton("cancel", 128, 32);
            buy_item_button[1].Position = new Point(BuyItemPosition.X + 180, BuyItemPosition.Y + 275);
            buy_item_button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            buy_item_button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            buy_item_button[1].MouseUp += CancelButton_MouseUp;

            buy_item_button[2] = new EngineButton("subtract", 64, 32);
            buy_item_button[2].Position = new Point(BuyItemPosition.X + 75, BuyItemPosition.Y + 75);
            buy_item_button[2].BorderRect = new Rectangle(15, 7, 32, 19);
            buy_item_button[2].SourceRect = new Rectangle(0, 0, 64, 32);
            buy_item_button[2].MouseUp += SubtractButton_MouseUp;

            buy_item_button[3] = new EngineButton("sum", 64, 32);
            buy_item_button[3].Position = new Point(BuyItemPosition.X + 183, BuyItemPosition.Y + 75);
            buy_item_button[3].BorderRect = new Rectangle(15, 7, 32, 19);
            buy_item_button[3].SourceRect = new Rectangle(0, 0, 64, 32);
            buy_item_button[3].MouseUp += SumButton_MouseUp;

            textbox = new EngineTextBox("textbox_2", 224, 32);
            textbox.Size = new Size2(224, 32);
            textbox.SourceRect = new Rectangle(0, 0, 224, 32);
            textbox.BorderRect = new Rectangle(3, 0, 221, 32);
            textbox.Position = new Point(BuyItemPosition.X + 48, BuyItemPosition.Y + 240);
            textbox.CursorEnabled = true;
            textbox.Transparency = 220;
            textbox.TextFormat = FontDrawFlags.Center;
        }

        /// <summary>
        /// Desenha os controles.
        /// </summary>
        public static void Draw() {
            if (!Visible) return;

            background.Draw();

            //desenha os botões
            for (var n = 0; n < MAX_BUTTON; n++) button[n].Draw();

            DrawItems();

            EngineFont.DrawText("Meu balanço", Position.X + 30, Position.Y + 30, Color.White, EngineFontStyle.Regular);
            EngineFont.DrawText("$: " + Client.PlayerLocal.Cash.ToString("n0"), Position.X + 30, Position.Y + 48, Color.DeepSkyBlue, EngineFontStyle.Regular);
            EngineFont.DrawText( Page + " / " + MaxPage, Position.X + 330, Position.Y + 355, Color.White, EngineFontStyle.Regular);

            DrawBuyItem();
        }

         /// <summary>
        /// Desenha os items de cash na tela.
        /// </summary>
        private static void DrawItems() {
            var icon = 0;
            for (var n = 0; n < MAX_ITEM; n++) {
                cash_item_back[n].Draw(cash_item_background[SelectedTexture[n]]);

                if (Items[n].CashItemID == 0) { continue; }

                cash_item_slot[n].Draw(slot);

                icon = Items[n].Item.IconID;

                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                EngineCore.SpriteDevice.Draw(EngineTexture.FindTextureByID(icon, EngineTextureType.Icons), Color.White, null, null, new Vector3(cash_item_slot[n].Position.X + 4, cash_item_slot[n].Position.Y + 4, 0));
                EngineCore.SpriteDevice.End();

                EngineFont.DrawText(Items[n].Item.Name, new Rectangle(cash_item_back[n].Position.X, cash_item_back[n].Position.Y + 5, 127, 25), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
                EngineFont.DrawText("x" + Items[n].Quantity, new Rectangle(cash_item_back[n].Position.X, cash_item_back[n].Position.Y + 70, 127, 25), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
                EngineFont.DrawText("$: " + Items[n].Price.ToString("n0"), new Rectangle(cash_item_back[n].Position.X, cash_item_back[n].Position.Y + 90, 127, 25), Color.DeepSkyBlue, EngineFontStyle.Regular, FontDrawFlags.Center);
            }
        }

        /// <summary>
        /// Desenha a janela de compra.
        /// </summary>
        private static void DrawBuyItem() {
            if (!BuyItemVisible) return;

            buy_item_background.Draw();
            buy_item_slot.Draw(slot);

            if (Items[SelectedItem].Item.IconID > 0) {
                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                EngineCore.SpriteDevice.Draw(EngineTexture.FindTextureByID(Items[SelectedItem].Item.IconID, EngineTextureType.Icons), Color.White, null, null, new Vector3(buy_item_slot.Position.X + 4, buy_item_slot.Position.Y + 4, 0));
                EngineCore.SpriteDevice.End();
            }

            textbox.MouseButtons();
            textbox.Draw();
            textbox.DrawTextMesured();

            EngineFont.DrawText(Items[SelectedItem].Item.Name, new Rectangle(BuyItemPosition.X, BuyItemPosition.Y + 15, 320, 25), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText(Quantity + " / " + BuyLimit, new Rectangle(BuyItemPosition.X, BuyItemPosition.Y + 80, 320, 25), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
            EngineFont.DrawText("$ "+ (Quantity * Items[SelectedItem].Price).ToString("n0"), new Rectangle(BuyItemPosition.X, BuyItemPosition.Y + 100, 320, 25), Color.DeepSkyBlue, EngineFontStyle.Regular, FontDrawFlags.Center);
            
            for(int n = 0; n < itemText.Count; n++) 
                EngineFont.DrawText(itemText[n].Text, new Rectangle(BuyItemPosition.X, BuyItemPosition.Y + (n * 20) + 130, 320, 25), itemText[n].Color, EngineFontStyle.Regular, FontDrawFlags.Center);
         
            EngineFont.DrawText("Insira o nome do personagem", new Rectangle(BuyItemPosition.X, BuyItemPosition.Y + 220, 320, 25), Color.Orange, EngineFontStyle.Regular, FontDrawFlags.Center);

            for (int n = 0; n < MAX_BUY_BUTTON; n++) buy_item_button[n].Draw(); 
        }

        /// <summary>
        /// Prepara a descrição do item para exibição.
        /// </summary>
        public static void PrepareItemText() {
            itemText.Clear();

            itemText.Add(new TextData() { Text = (Items[SelectedItem].Tradeable == 1) ? "Pode ser negociado" : "Não pode ser negociado", Color = Color.White });
            if (Items[SelectedItem].SoulBound == 1) itemText.Add(new TextData() { Text = "Vinculado ao personagem", Color = Color.White });
            if (ExpireDays > 0) itemText.Add(new TextData() { Text = $"Tempo de uso {ExpireDays} dias", Color = Color.White });
            itemText.Add(new TextData() { Text = (GiftEnabled ==true) ? "Pode ser enviado como presente" : "Não pode ser enviado como presente", Color = Color.GreenYellow });

            textbox.Text = Client.PlayerLocal.Name;
        }

        #region Cash Item Back
        /// <summary>
        /// Exibe a janela de compra do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CashItemBack_MouseUp(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            var index = ((EngineObject)sender).Index;
            SelectedTexture[index] = 0;

            if (Items[index].CashItemID == 0) { return; }

            WindowViewItem.Visible = false;
            WaitData = true;
            SelectedItem = index;

            WorldPacket.RequestItemInformation(Items[index].CashItemID);

            EngineMultimedia.Play(EngineSoundEnum.Close);
        }

        /// <summary>
        /// Altera a textura para o fundo selecionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CashItemBack_MouseMove(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            var index = ((EngineObject)sender).Index;
            SelectedTexture[index] = 1;
        }

        /// <summary>
        /// Altera a textura para o fundo normal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CashItemBack_MouseLeave(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            var index = ((EngineObject)sender).Index;
            SelectedTexture[index] = 0;
        }
        #endregion

        #region Cash Item Slot
        private static void CashItemSlot_MouseMove(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            var index = ((EngineObject)sender).Index;

            WindowViewItem.CopyItem(Items[index].Item);
            WindowViewItem.Visible = true;
        }

        private static void CashItemSlot_MouseLeave(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            WindowViewItem.Visible = false;
        }
        #endregion

        #region Buy Window Buttons
        private static void BuyItemSlot_MouseMove(object sender, EngineEventArgs e) {
            WindowViewItem.CopyItem(Items[SelectedItem].Item);
            WindowViewItem.Visible = true;
        }

        private static void BuyItemSlot_MouseLeave(object sender, EngineEventArgs e) {
            WindowViewItem.Visible = false;
        }

        /// <summary>
        /// Adquire o item selecionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BuyButton_MouseUp(object sender, EngineEventArgs e) {
            if (WaitData) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);
            WaitData = true;

            WorldPacket.RequestBuyItem(Items[SelectedItem].CashItemID, Quantity, textbox.Text.Trim());
        }

        /// <summary>
        /// Fecha a janela de compra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CancelButton_MouseUp(object sender, EngineEventArgs e) {
            if (WaitData) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);
            BuyItemVisible = false;
            Quantity = 1;
        }

        /// <summary>
        /// Aumenta a quantidade.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SumButton_MouseUp(object sender, EngineEventArgs e) {
            if (WaitData) return;

            if (BuyLimit == 0) return;

            if (Quantity < BuyLimit) {
                Quantity++;
                EngineMultimedia.Play(EngineSoundEnum.Click);
            }
        }

        /// <summary>
        /// Diminui a quantidade.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SubtractButton_MouseUp(object sender, EngineEventArgs e) {
            if (WaitData) return;

            if (BuyLimit == 0) return;

            if (Quantity > 1) {
                Quantity--;
                EngineMultimedia.Play(EngineSoundEnum.Click);
            }
        }

        #endregion

        /// <summary>
        /// Execução das catergorias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Buttons_MouseUp(object sender, EngineEventArgs e) {
            if (BuyItemVisible) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);

            //pega o indice do controle
            var index = ((EngineButton)sender).Index;

            //botão sair
            if (index == 15) {
                Visible = false;
                return;
            }

            //se os dados estiverem em transferencia, retorna
            if (WaitData) return;

            //botão anterior
            if (index == 13) {
                if (Page <= 1) { return; }

                Page--;
                WaitData = true;
                WorldPacket.RequestItems(Category, Page);
                return;
            }

            //botão próximo
            if (index == 14) {
                if (Page >= MaxPage) { return; }

                Page++;
                WaitData = true;
                WorldPacket.RequestItems(Category, Page);
                return;
            }

            //requisita os items da categoria
            //muda de categoria
            Category = (CashShopItemCategory)index;
            Page = 1;
            WorldPacket.RequestItems(Category, Page);
        }
    }
}
