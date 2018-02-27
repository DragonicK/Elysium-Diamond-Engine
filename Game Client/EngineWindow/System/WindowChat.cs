using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowChat {
        private static EngineObject background, scroll_background;
        public static EngineTextBox textbox;
        private static Point position;
        public static List<text_chat> chat_text;
        private static EngineButton[] button = new EngineButton[2];

        public static byte Transparency {
            set { background.Transparency = value; }
        }

        public struct text_chat {
            public string text;
            public Color color;

            public text_chat(string _text, Color _color) {
                text = _text;
                color = _color;
            }
        }

        private static int index = 0;
  
        public static void Initialize() {
            position = new Point(5, 435);

            background = new EngineObject();
            background.Position = position;
            background.Size = new Size2(277, 203);
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/window_chat.png");
            background.SourceRect = new Rectangle(0, 0, 277, 203);
            background.Transparency = 140;

            scroll_background = new EngineObject();
            scroll_background.Position = new Point(position.X + 255, position.Y + 10);
            scroll_background.Size = new Size2(10, 172);
            scroll_background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/scroll_background.png");
            scroll_background.SourceRect = new Rectangle(0, 0, 10, 172);
            scroll_background.Transparency = 255;

            textbox = new EngineTextBox("textbox", 256, 32);
            textbox.Size = new Size2(256, 32);
            textbox.SourceRect = new Rectangle(0, 0, 256, 32);
            textbox.BorderRect = new Rectangle(0, 0, 256, 32);
            textbox.Position = new Point(position.X + 11, position.Y + 155);
            textbox.CursorEnabled = true;
            textbox.Transparency = 220;
            textbox.MaxLenght = 500;
            textbox.TextTransparency = 255;
            textbox.MouseUp += Textbox_MouseUp;
            textbox.TextFormat = SharpDX.Direct3D9.FontDrawFlags.Left;

            button[0] = new EngineButton("arrow_up", 10, 10);
            button[0].Name = "up";
            button[0].Position = new Point(position.X + 255, position.Y + 10);
            button[0].BorderRect = new Rectangle(0, 0, 10, 10);
            button[0].SourceRect = new Rectangle(0, 0, 10, 10);
            button[0].Size = new Size2(10, 10);
            button[0].MouseUp += Up_Click;

            button[1] = new EngineButton("arrow_down", 10, 10);
            button[1].Name = "down";
            button[1].Position = new Point(position.X + 255, position.Y + 145);
            button[1].BorderRect = new Rectangle(0, 0, 10, 10);
            button[1].SourceRect = new Rectangle(0, 0, 10, 10);
            button[1].Size = new Size2(10, 10);
            button[1].MouseUp += Down_Click;

            chat_text = new List<text_chat>();
            AddText("小松さん: ", "チカラさんが、俺の友達だからさ！", MessageType.Server);
            AddText("小松さん: ", "你是我的朋友！", MessageType.Server);
            AddText("小松さん: "," あなたの名前何と言う何ですか。你是我的朋友 你是我的朋友！", MessageType.Server);
            AddText("小松さん: ", "今日は仕事の面接があるん！", MessageType.Server);
            AddText("小松さん: ", "チカラさんが、俺の友達だからさ！", MessageType.None);
            AddText("小松さん: ", "チカラさんが、俺の友達だからさ！", MessageType.None);
            AddText("小松さん: ", "你是我的朋友！", MessageType.None);
        }

        public static void Draw() {
            background.Draw();
            textbox.MouseButtons();
            textbox.Draw();
            textbox.DrawText();

            scroll_background.Draw();
            button[0].Draw();
            button[1].Draw();

            if (chat_text.Count == 0) return;

            var line = 0;

            for(var n = index; n < chat_text.Count; n++) {

                EngineFont.DrawText(chat_text[n].text, new Rectangle(position.X + 17, (position.Y + 8) + (line * 20), 250, 200), chat_text[n].color, EngineFontStyle.Regular, SharpDX.Direct3D9.FontDrawFlags.Left);

                line++;

                if (line>= 7) return;
            }
        }

        private static void Textbox_MouseUp(object sender, EngineEventArgs e) {
 

        }

        public static void AddText(string from, string _text, MessageType type) {
            var text = from + ": " +  _text;
            var rect = EngineFont.MeasureString(null, EngineFontStyle.Regular, text, new Rectangle(0, 0, 250, 200), SharpDX.Direct3D9.FontDrawFlags.WordBreak);

            var color = Color.White;

         //   if (type == MessageType.Admin) { color = Color.Yellow; }
        //    if (type == MessageType.Alert) { color = Color.Coral; }
            if (type == MessageType.Server) {
                color = Color.Yellow;
                text = text.Replace(": ", "");
            }

            if (type == MessageType.Guild) { color = Color.Green; }
            if (type == MessageType.Private) { color = Color.Blue; }
            if (type == MessageType.Party) { color = Color.Pink; }
            if (type == MessageType.Global) { color = Color.White ; }

            //   totalline.Add(rect.Height / 20);


            if ((rect.Height / 20) > 1) {

                var lenght = text.Length;

                var value = rect.Width / lenght;

                var _removeLenght = 0;

                if (value >= 3 && value <= 8) {
                    _removeLenght = 29;

                }
                else {
                    _removeLenght = 17;
                }

                if (rect.Width > 220) { 
                    var buffer = text.Substring(0, _removeLenght);
                    chat_text.Add(new text_chat(buffer, color));
                    chat_text.Add(new text_chat(text.Substring(_removeLenght, lenght - _removeLenght), color));
                }
                else {
                    chat_text.Add(new text_chat(text, color));
                }
          
            } else {
                chat_text.Add(new text_chat(text, color));
            }

            if (chat_text.Count > 7) index = chat_text.Count - 7;
            //10, 9, 8, 7, 
        //    for (int n = totalline.Count - 1; n > 0; n--) {
          //      linestoshow += totalline[n];

         //       if (linestoshow >= 6) {
          //       
         //           return;
         //       }
        //    }


            //        if (totalline.Count >= 7) {
            //           var last = totalline.Count;

            //           index = totalline.Count - 5;

            //     }


            //  if (totalline.Count >= 7) { index = totalline.Count - 5;
        }

        private static void Up_Click(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Close);

            if (index > 0) index--;
        }
        
        private static void Down_Click(object sender, EngineEventArgs e) {
            EngineMultimedia.Play(EngineSoundEnum.Close);

            if (index < chat_text.Count - 1) index++;

        }

        public static void SendChat() {
            if (string.IsNullOrEmpty(textbox.Text)) return;

            if (textbox.Text.Length > 1) {
                var text = textbox.Text.Trim().Substring(0, 1);
                var result = (text == "/") ? true : false;

                if (result) {
                    Administrator.Command.ParseCommand(textbox.Text);         
                    return;
                }
            }

            WorldPacket.GlobalChat(textbox.Text);
        }
    }
}
 