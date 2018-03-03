using System;
using System.Collections.Generic;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;
using Elysium_Diamond.Resource;
using SharpDX;
using SharpDX.Direct3D9;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowMail {
        public class Mail {
            public int ID;
            public bool Read;
            public string Text { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Sender { get; set; } = string.Empty;
            public long Currency { get; set; }
        }

        public static Item Item { get; set; }
        public static int IconID {get;set;}


        /// <summary>
        /// Quantidade máxima de botões.
        /// </summary>
        private const byte MAX_BUTTON = 3;

        /// <summary>
        /// Quantidade máxima de labels.
        /// </summary>
        private const byte MAX_LABEL = 6;

        /// <summary>
        /// Posição da janela.
        /// </summary>
        public static Point Position { get; set; }

        /// <summary>
        /// Lista de titulos de mensagens.
        /// </summary>
        public static List<Mail> Messages { get; set; }

        public static int SelectedMail { get; set; }

        /// <summary>
        /// Plano de fundo.
        /// </summary>
        private static EngineObject background;

        /// <summary>
        /// Indice de mensagens.
        /// </summary>
        private static int message_index;

        /// <summary>
        /// Usado somente para troca de textura no evento label_mousemove.
        /// </summary>
        private static byte[] selected_row = new byte[MAX_LABEL];

        /// <summary>
        /// Botão novo, voltar e deletar.
        /// </summary>
        private static EngineButton[] button = new EngineButton[MAX_BUTTON];

        /// <summary>
        /// Lista dos títulos exibidos.
        /// </summary>
        private static EngineLabel[] labels = new EngineLabel[MAX_LABEL];

        /// <summary>
        /// Botão para cima e baixo, barra de rolagem.
        /// </summary>
        private static EngineButton[] arrows = new EngineButton[2];

        private static Texture[] texture_mailrow = new Texture[2];

        private static Texture scroll;

        #region ReadMail 
        private static EngineObject item_slot;
        public static bool ReadMail { get; set; }
        private static Texture texture_slot;
        private static Texture texture_money;
        private static Texture texture_message;
        private static Texture texture_title;
        #endregion

        public static void Initialize() {
            Position = new Point(350, 200);

            Messages = new List<Mail>();
            Item = new Item();

            texture_mailrow[0] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/mail_row_normal.png", 288, 64);
            texture_mailrow[1] = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/mail_row_selected.png", 288, 64);

            scroll = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/scrollbar_198.png", 10, 218);

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(320, 352);
            background.Texture = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/cash_buy.png");
            background.SourceRect = new Rectangle(0, 0, 320, 352);

            button[0] = new EngineButton("new", 128, 32);
            button[0].Position = new Point(Position.X + 20, Position.Y + 270);
            button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            button[0].MouseUp += NewButton_MouseUp;

            button[1] = new EngineButton("back", 128, 32);
            button[1].Position = new Point(Position.X + 175, Position.Y + 270);
            button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            button[1].MouseUp += BackButton_MouseUp;

            button[2] = new EngineButton("delete", 128, 32);
            button[2].Position = new Point(Position.X + 20, Position.Y + 270);
            button[2].BorderRect = new Rectangle(20, 2, 86, 26);
            button[2].SourceRect = new Rectangle(0, 0, 128, 32);
            button[2].MouseUp += DeleteButton_MouseUp;

            for (var n = 0; n < MAX_LABEL; n++) {
                labels[n] = new EngineLabel();
                labels[n].Index = n;
                labels[n].Position = new Point(Position.X + 20, Position.Y + (n * 38) + 30);
                labels[n].Size = new Size2(265, 40);
                labels[n].SourceRect = new Rectangle(0, 0, 265, 40);
                labels[n].BorderRect = new Rectangle(3, 5, 258, 32);
                labels[n].TextPosition = new Point(0, 8);
                labels[n].TextColor = Color.White;
                labels[n].TextTransparency = 255;
                labels[n].FontDrawFlags = FontDrawFlags.Center;
                labels[n].MouseMove += Label_MouseMove;
                labels[n].MouseLeave += Label_MouseLeave;
                labels[n].MouseUp += Label_MouseUp;
            }

            arrows[0] = new EngineButton("arrow_up", 10, 10);
            arrows[0].Name = "up";
            arrows[0].Position = new Point(Position.X + 290, Position.Y + 35);
            arrows[0].BorderRect = new Rectangle(0, 0, 10, 10);
            arrows[0].SourceRect = new Rectangle(0, 0, 10, 10);
            arrows[0].Size = new Size2(10, 10);
            arrows[0].MouseUp += ArrowUp_MouseUp;

            arrows[1] = new EngineButton("arrow_down", 10, 10);
            arrows[1].Name = "down";
            arrows[1].Position = new Point(Position.X + 290, Position.Y + 245);
            arrows[1].BorderRect = new Rectangle(0, 0, 10, 10);
            arrows[1].SourceRect = new Rectangle(0, 0, 10, 10);
            arrows[1].Size = new Size2(10, 10);
            arrows[1].MouseUp += ArrowDown_MouseUp;

            #region Read Mail 
            texture_title = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/textbox.png", 256, 32);
            texture_money = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/textbox_2.png", 224, 32);
            texture_message = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/text_message.png", 256, 128);
            texture_slot = EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Graphics/slot.png", 288, 64);

            item_slot = new EngineObject();
            item_slot.Position = new Point(Position.X + 247, Position.Y + 225);
            item_slot.Size = new Size2(40, 40);
            item_slot.SourceRect = new Rectangle(0, 0, 40, 40);
            item_slot.BorderRect = new Rectangle(2, 2, 38, 38);
            item_slot.MouseUp += ItemSlot_MouseUp;
            item_slot.MouseMove += ItemSlot_MouseMove;
            item_slot.MouseLeave += ItemSlot_MouseLeave;
            #endregion
        }

        public static void Draw() {
            background.Draw();

            #region Mail List
            if (!ReadMail) {
                button[0].Draw();
                button[1].Draw();

                for (var n = 0; n < MAX_LABEL; n++) {
                    labels[n].Draw(texture_mailrow[selected_row[n]]);

                    if ((Messages.Count - 1) <= n - 1) continue;

                    labels[n].TextTransparency = (Messages[n + message_index].Read == false) ? (byte)255 : (byte)120;
                    labels[n].DrawText(Messages[n + message_index].Title);
                }

                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                EngineCore.SpriteDevice.Draw(scroll, Color.White, null, null, new Vector3(Position.X + 290, Position.Y + 37, 0));
                EngineCore.SpriteDevice.End();

                arrows[0].Draw();
                arrows[1].Draw();
            }
            #endregion

            #region Read Mail
            if (ReadMail) {
                button[1].Draw();
                button[2].Draw();

                EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                //desenha o campo do remetente
                EngineCore.SpriteDevice.Draw(texture_title, Color.White, null, null, new Vector3(Position.X + 30, Position.Y + 25, 0));

                //desenha o campo do título
                EngineCore.SpriteDevice.Draw(texture_title, Color.White, null, null, new Vector3(Position.X + 30, Position.Y + 60, 0));

                //desenha a textura de mensagem, 256, 128 size
                EngineCore.SpriteDevice.Draw(texture_message, Color.White, null, null, new Vector3(Position.X + 30, Position.Y + 95, 0));

                //desenha o campo do dinheiro
                EngineCore.SpriteDevice.Draw(texture_money, Color.White, null, null, new Vector3(Position.X + 25, Position.Y + 230, 0));
                EngineCore.SpriteDevice.End();

                //desenha o slot do item
                item_slot.Draw(texture_slot);

                if (IconID > 0) {
                    EngineCore.SpriteDevice.Begin(SpriteFlags.AlphaBlend);
                    EngineCore.SpriteDevice.Draw(EngineTexture.FindTextureByID(IconID, EngineTextureType.Icons), Color.White, null, null, new Vector3(item_slot.Position.X + 4, item_slot.Position.Y + 4, 0));
                    EngineCore.SpriteDevice.End();
                }

                if (Item.Quantity > 0) EngineFont.DrawText("" + Item.Quantity, new Rectangle(item_slot.Position.X + 5, item_slot.Position.Y + 17, 30, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Right);

                EngineFont.DrawText(Messages[SelectedMail].Sender, new Rectangle(Position.X + 30, Position.Y + 30, 256, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
                EngineFont.DrawText(Messages[SelectedMail].Title, new Rectangle(Position.X + 30, Position.Y + 65, 256, 20), Color.White, EngineFontStyle.Regular, FontDrawFlags.Center);
                EngineFont.DrawText(Messages[SelectedMail].Text, new Rectangle(Position.X + 35, Position.Y + 97, 250, 125), Color.White, EngineFontStyle.Regular, FontDrawFlags.WordBreak);

                EngineFont.DrawText("$ " + Messages[SelectedMail].Currency.ToString("n0"), Position.X + 37, Position.Y + 235, Color.White, EngineFontStyle.Regular);
            }
            #endregion
        }

        #region Arrows
        /// <summary>
        /// Desce a rolagem mostrando os items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ArrowDown_MouseUp(object sender, EngineEventArgs e) {
            if (Messages.Count <= MAX_LABEL) return;
            if (message_index == Messages.Count - MAX_LABEL) return;
            message_index++;
        }

        /// <summary>
        /// Sobe a rolagem mostrando os items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ArrowUp_MouseUp(object sender, EngineEventArgs e) {
            if (message_index == 0) return;
            message_index--;
        }
        #endregion

        #region Buttons 
        private static void NewButton_MouseUp(object sender, EngineEventArgs e) {
           // Messages.Add(new Mail(1, false, "seu cuca é eu"));
            WorldPacket.RequestMailTitle();
        }

        private static void BackButton_MouseUp(object sender, EngineEventArgs e) {
            ReadMail = false;
        }

        private static void DeleteButton_MouseUp(object sender, EngineEventArgs e) {

        }
        #endregion

        #region Labels 
        /// <summary>
        /// Exibe a janela mostrando o correio recebido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Label_MouseUp(object sender, EngineEventArgs e) {
            var index = ((EngineLabel)sender).Index;
            
            if (Messages.Count - 1 < index) return;

            SelectedMail = index + message_index;

            WorldPacket.RequestMail(Messages[SelectedMail].ID);
        }

        private static void Label_MouseMove(object sender, EngineEventArgs e) {
            var index = ((EngineLabel)sender).Index;
            selected_row[index] = 1;
        }

        private static void Label_MouseLeave(object sender, EngineEventArgs e) {
            var index = ((EngineLabel)sender).Index;
            selected_row[index] = 0;
        }
        #endregion

        #region Item Slot
        /// <summary>
        /// Move o item recebido para o inventário.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ItemSlot_MouseUp(object sender, EngineEventArgs e) {

        }

        /// <summary>
        /// Exibe as informações do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ItemSlot_MouseMove(object sender, EngineEventArgs e) {
            if (Item.ID > 0) {
                WindowViewItem.CopyItem(Item);
                WindowViewItem.Visible = true;
            }
        }

        /// <summary>
        /// Esconde as informações do item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ItemSlot_MouseLeave(object sender, EngineEventArgs e) {
                WindowViewItem.Visible = false;
        }

        #endregion
    }
}
