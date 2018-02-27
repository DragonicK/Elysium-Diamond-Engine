using System;
using SharpDX;
using SharpDX.Direct3D9;
using Elysium_Diamond.DirectX;
using Elysium_Diamond.Network;

namespace Elysium_Diamond.EngineWindow {
    public static class WindowPin {
        /// <summary>
        /// Posição da janela.
        /// </summary>
        private static Point Position;
        public static bool Visible;

        #region Texture
        /// <summary>
        /// Textura inativa.
        /// </summary>
        const byte INACTIVE = 0;

        /// <summary>
        /// Textura de seleção.
        /// </summary>
        const byte HOVER = 1;

        /// <summary>
        /// Quantidade máxima de textura.
        /// </summary>
        const byte MAX_TEXTURE = 2;

        /// <summary>
        /// Texturas de botão.
        /// </summary>
        private static Texture[] texture = new Texture[MAX_TEXTURE];
        #endregion

        const byte MAX_BUTTON = 3;
        const byte MAX_TEXTBOX = 3;
        const byte MAX_NUMBER = 12;

        /// <summary>
        /// Etapas do processo.
        /// </summary>
        private static PinState State = PinState.Login;

        private static bool[] SelectedNumber = new bool[MAX_NUMBER];
        private static EngineLabel[] numbers = new EngineLabel[MAX_NUMBER];
        private static EngineTextBox[] textbox = new EngineTextBox[MAX_TEXTBOX];
        private static EngineButton[] button = new EngineButton[MAX_BUTTON];

        /// <summary>
        /// Textbox selecionado.
        /// </summary>
        private static byte SelectedTextbox = 0;

        /// <summary>
        /// Plano de fundo.
        /// </summary>
        private static EngineObject background;

        public static void Initialize() {
            //posicionado no centro da tela
            Position = new Point(368, 170);
            Visible = false;
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            texture[INACTIVE] = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot.png", 40, 40);
            texture[HOVER] = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/slot_hover.png", 40, 40);

            background = new EngineObject();
            background.Position = Position;
            background.Size = new Size2(288, 352);
            background.Texture = EngineTexture.TextureFromFile($"{AppData}/Elysium/Data/Graphics/window_pin.png");
            background.SourceRect = new Rectangle(0, 0, 288, 352);
            background.Transparency = 255;

            textbox[0] = new EngineTextBox("textbox_2", 224, 32);
            textbox[0].Size = new Size2(224, 32);
            textbox[0].SourceRect = new Rectangle(0, 0, 224, 32);
            textbox[0].BorderRect = new Rectangle(3, 0, 221, 32);
            textbox[0].Position = new Point(Position.X + 32, Position.Y + 190);
            textbox[0].CursorEnabled = true;
            textbox[0].Enabled = true;
            textbox[0].Password = true;
            textbox[0].Transparency = 220;
            textbox[0].TextTransparency = 255;
            textbox[0].MouseUp += Textbox1_MouseUp;
            textbox[0].TextFormat = FontDrawFlags.Center;

            textbox[1] = new EngineTextBox("textbox_2", 224, 32);
            textbox[1].Size = new Size2(224, 32);
            textbox[1].SourceRect = new Rectangle(0, 0, 224, 32);
            textbox[1].BorderRect = new Rectangle(3, 0, 221, 32);
            textbox[1].Position = new Point(Position.X + 32, Position.Y + 222);
            textbox[1].CursorEnabled = false;
            textbox[1].Enabled = false;
            textbox[1].Visible = false;
            textbox[1].Password = true;
            textbox[1].Transparency = 220;
            textbox[1].TextTransparency = 255;
            textbox[1].MouseUp += Textbox2_MouseUp;
            textbox[1].TextFormat = FontDrawFlags.Center;

            textbox[2] = new EngineTextBox("textbox_2", 224, 32);
            textbox[2].Size = new Size2(224, 32);
            textbox[2].SourceRect = new Rectangle(0, 0, 224, 32);
            textbox[2].BorderRect = new Rectangle(3, 0, 221, 32);
            textbox[2].Position = new Point(Position.X + 32, Position.Y + 254);
            textbox[2].CursorEnabled = false;
            textbox[2].Enabled = false;
            textbox[2].Visible = false;
            textbox[2].Password = true;
            textbox[2].Transparency = 220;
            textbox[2].TextTransparency = 255;
            textbox[2].MouseUp += Textbox3_MouseUp;
            textbox[2].TextFormat = FontDrawFlags.Center;

            button[0] = new EngineButton("ok", 128, 32);
            button[0].Position = new Point(Position.X + 25, Position.Y + 293);
            button[0].BorderRect = new Rectangle(20, 2, 86, 26);
            button[0].SourceRect = new Rectangle(0, 0, 128, 32);
            button[0].Size = new Size2(128, 32);
            button[0].MouseUp += OkButton_MouseDown;

            button[1] = new EngineButton("cancel", 128, 32);
            button[1].Position = new Point(Position.X + 137, Position.Y + 293);
            button[1].BorderRect = new Rectangle(20, 2, 86, 26);
            button[1].SourceRect = new Rectangle(0, 0, 128, 32);
            button[1].Size = new Size2(128, 32);
            button[1].MouseUp += CancelButton_MouseUp;

            button[2] = new EngineButton("change", 128, 32);
            button[2].Position = new Point(Position.X + 80, Position.Y + 25);
            button[2].BorderRect = new Rectangle(20, 2, 86, 26);
            button[2].SourceRect = new Rectangle(0, 0, 128, 32);
            button[2].Size = new Size2(128, 32);
            button[2].MouseUp += ChangeButton_MouseUp;

            for (var n = 0; n < MAX_NUMBER; n++) {
                numbers[n] = new EngineLabel();
                numbers[n].Index = n;
                numbers[n].Size = new Size2(40, 40);
                numbers[n].Text = n + "";
                numbers[n].TextFontStyle = EngineFontStyle.Regular;
                numbers[n].SourceRect = new Rectangle(0, 0, 40, 40);
                numbers[n].BorderRect = new Rectangle(0, 0, 40, 40);
                numbers[n].Transparency = 255;
                numbers[n].MouseUp += Numbers_MouseUp;
                numbers[n].MouseMove += Numbers_MouseMove;
                numbers[n].MouseLeave += Numbers_MouseLeave;
            }

            numbers[0].Position = new Point(Position.X + 178, Position.Y + 137);
            numbers[1].Position = new Point(Position.X + 70, Position.Y + 65);
            numbers[2].Position = new Point(Position.X + 106, Position.Y + 65);
            numbers[3].Position = new Point(Position.X + 142, Position.Y + 65);
            numbers[4].Position = new Point(Position.X + 70, Position.Y + 101);
            numbers[5].Position = new Point(Position.X + 106, Position.Y + 101);
            numbers[6].Position = new Point(Position.X + 142, Position.Y + 101);
            numbers[7].Position = new Point(Position.X + 70, Position.Y + 137);
            numbers[8].Position = new Point(Position.X + 106, Position.Y + 137);
            numbers[9].Position = new Point(Position.X + 142, Position.Y + 137);

            //back
            numbers[10].Position = new Point(Position.X + 178, Position.Y + 65);
            numbers[10].Text = "B";

            //clear
            numbers[11].Position = new Point(Position.X + 178, Position.Y + 101);
            numbers[11].Text = "C";
        }

        public static void Draw() {
            if (!Visible) return;

            background.Draw();

            //desenha o textbox
            for (var n = 0; n < MAX_TEXTBOX; n++) {
                textbox[n].MouseButtons();
                textbox[n].Draw();
                textbox[n].DrawTextMesured();
            }

            //desenha os botões
            for (var n = 0; n < MAX_BUTTON; n++) button[n].Draw();

            //desenha os números
            for (var n = 0; n < MAX_NUMBER; n++) {
                numbers[n].MouseButtons();

                if (SelectedNumber[n])
                    numbers[n].Draw(texture[HOVER]);
                else
                    numbers[n].Draw(texture[INACTIVE]);

                numbers[n].DrawTextCenter();
            }

            //desenha os textos explicativos
            DrawTexts();
        }

        /// <summary>
        /// Altera o modo do funcionamento da janela.
        /// </summary>
        /// <param name="state"></param>
        public static void ChangeState(PinState state) {
            State = state;
            SelectedTextbox = 0;

            textbox[0].CursorEnabled = true;
            textbox[1].CursorEnabled = false;
            textbox[2].CursorEnabled = false;

            textbox[0].Clear();
            textbox[1].Clear();
            textbox[2].Clear();

            if (State == PinState.Login) {
                textbox[0].Enabled = true;
                textbox[1].Visible = false;
                textbox[2].Visible = false;
                textbox[1].Enabled = true;
                textbox[2].Enabled = true;
            }

            if (State == PinState.Change) {
                textbox[0].Visible = true;
                textbox[1].Visible = true;
                textbox[2].Visible = true;
                textbox[0].Enabled = true;
                textbox[1].Enabled = true;
                textbox[2].Enabled = true;
            }

            if (State == PinState.Initialize) {
                textbox[0].Visible = true;
                textbox[0].Enabled = true;
                textbox[1].Visible = true;
                textbox[1].Enabled = true;
                textbox[2].Visible = false;
                textbox[2].Enabled = false;
            }
        }

        /// <summary>
        /// Desenha os textos explicativos.
        /// </summary>
        private static void DrawTexts() {
            #region Initialize
            if (State == PinState.Initialize) {
                if (textbox[0].Text.Length == 0) {
                    if (textbox[0].CursorEnabled == false) {
                        EngineFont.DrawText("pin", textbox[0].Size, new Point(textbox[0].Position.X, textbox[0].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                    }
                }

                if (textbox[1].Text.Length == 0) {
                    if (textbox[1].CursorEnabled == false) {
                        EngineFont.DrawText("confirmação", textbox[1].Size, new Point(textbox[1].Position.X, textbox[1].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                    }
                }
            }
            #endregion

            #region Change
            if (State == PinState.Change) {
                if (textbox[0].Text.Length == 0) {
                    if (textbox[0].CursorEnabled == false) {
                        EngineFont.DrawText("pin atual", textbox[0].Size, new Point(textbox[0].Position.X, textbox[0].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                    }
                }

                if (textbox[1].Text.Length == 0) {
                    if (textbox[1].CursorEnabled == false) {
                        EngineFont.DrawText("novo pin", textbox[1].Size, new Point(textbox[1].Position.X, textbox[1].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                    }
                }

                if (textbox[2].Text.Length == 0) {
                    if (textbox[2].CursorEnabled == false) {
                        EngineFont.DrawText("confirmação", textbox[2].Size, new Point(textbox[2].Position.X, textbox[2].Position.Y + 4), new Color(255, 255, 255, 120), EngineFontStyle.Regular, FontDrawFlags.Center, false);
                    }
                }
            }
            #endregion
        }

        #region Event Buttons
        private static void Numbers_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            var index = ((EngineLabel)sender).Index;

            EngineMultimedia.Play(EngineSoundEnum.Close);

            if (index >= 0 & index <= 9) {
                if (textbox[SelectedTextbox].Text.Length < 12)
                    textbox[SelectedTextbox].AddText(char.Parse(index + ""));
            }

            //back
            if (index == 10) textbox[SelectedTextbox].RemoveText();
            //clear
            if (index == 11) textbox[SelectedTextbox].Clear();
        }

        private static void Numbers_MouseMove(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            var index = ((EngineLabel)sender).Index;
            SelectedNumber[index] = true;
        }

        private static void Numbers_MouseLeave(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            var index = ((EngineLabel)sender).Index;
            SelectedNumber[index] = false;
        }

        private static void Textbox1_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            if (textbox[0].Enabled) {
                textbox[0].CursorEnabled = true;
                textbox[1].CursorEnabled = false;
                textbox[2].CursorEnabled = false;

                SelectedTextbox = 0;
            }
        }

        private static void Textbox2_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            if (textbox[1].Enabled) {
                textbox[0].CursorEnabled = false;
                textbox[1].CursorEnabled = true;
                textbox[2].CursorEnabled = false;

                SelectedTextbox = 1;
            }
        }

        private static void Textbox3_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            if (textbox[2].Enabled) {
                textbox[0].CursorEnabled = false;
                textbox[1].CursorEnabled = false;
                textbox[2].CursorEnabled = true;

                SelectedTextbox = 2;
            }
        }

        private static void OkButton_MouseDown(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);

            #region PinState.Login 
            if (State == PinState.Login) {
                if (textbox[0].Text.Length < 6) {
                    EngineMessageBox.Enabled = true;
                    EngineMessageBox.Show("O PIN deve conter pelo menos 6 dígitos.");
                    return;
                }

                WorldPacket.SendPin((byte)State, textbox[0].Text);
                textbox[0].Clear();
            }
            #endregion

            #region PinState.Initialize 
            if (State == PinState.Initialize) {
                if (textbox[0].Text.Length < 6 || textbox[1].Text.Length < 6) {
                    EngineMessageBox.Enabled = true;
                    EngineMessageBox.Show("O PIN deve conter pelo menos 6 dígitos.");
                    return;
                }

                var result = textbox[0].Text.CompareTo(textbox[1].Text);

                if (result != 0) {
                    EngineMessageBox.Enabled = true;
                    EngineMessageBox.Show("O PIN não é idêntico nos dois campos.");
                    return;
                }

                WorldPacket.SendPin((byte)State, textbox[0].Text);

                textbox[0].Clear();
                textbox[1].Clear();
            }
            #endregion

            #region PinState.Change
            if (State == PinState.Change) {
                if (textbox[0].Text.Length < 6 || textbox[1].Text.Length < 6 || textbox[2].Text.Length < 6) {
                    EngineMessageBox.Enabled = true;
                    EngineMessageBox.Show("O PIN deve conter pelo menos 6 dígitos.");
                    return;
                }

                var result = textbox[1].Text.CompareTo(textbox[2].Text);

                if (result != 0) {
                    EngineMessageBox.Enabled = true;
                    EngineMessageBox.Show("O novo PIN não é idêntico nos dois campos.");
                    return;
                }

                WorldPacket.SendPin((byte)State, textbox[0].Text, textbox[1].Text);

                textbox[0].Clear();
                textbox[1].Clear();
                textbox[2].Clear();
            }
            #endregion
        }

        private static void CancelButton_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);

            Visible = false;

            if (State == PinState.Change) {
                ChangeState(PinState.Login);
                return;
            }
        }

        private static void ChangeButton_MouseUp(object sender, EngineEventArgs e) {
            if (EngineMessageBox.Visible) return;

            EngineMultimedia.Play(EngineSoundEnum.Click);

            if (State == PinState.Initialize) {
                EngineMessageBox.Enabled = true;
                EngineMessageBox.Show("Você precisa registrar um PIN primeiro.");
                return;
            }

            ChangeState(PinState.Change);
            EngineMessageBox.Enabled = true;
            EngineMessageBox.Show("Insira os dados para a alteração do PIN.");
        }
        #endregion

        /// <summary>
        /// Move entre os textos usando TAB.
        /// </summary>
        public static void SelectTextbox() {
            SelectedTextbox++;

            if (SelectedTextbox > 2) {
                SelectedTextbox = 0;
            }

            if (SelectedTextbox == 0) {
                textbox[0].CursorEnabled = true;

                textbox[1].CursorEnabled = false;
                textbox[1].CursorState = 0;
                textbox[2].CursorEnabled = false;
                textbox[2].CursorState = 0;
                SelectedTextbox = 0;
            }

            if (SelectedTextbox == 1) {
                if (textbox[1].Visible) {
                    textbox[1].CursorEnabled = true;

                    textbox[0].CursorEnabled = false;
                    textbox[0].CursorState = 0;
                    textbox[2].CursorEnabled = false;
                    textbox[2].CursorState = 0;
                    SelectedTextbox = 1;
                }
                else {
                    SelectedTextbox++;
                }
            }

            if (SelectedTextbox == 2) {
                if (textbox[2].Visible) {
                    textbox[2].CursorEnabled = true;

                    textbox[1].CursorEnabled = false;
                    textbox[1].CursorState = 0;
                    textbox[0].CursorEnabled = false;
                    textbox[0].CursorState = 0;
                    SelectedTextbox = 2;
                }
                else {
                    SelectedTextbox++;
                }
            }
        }
    }
}
