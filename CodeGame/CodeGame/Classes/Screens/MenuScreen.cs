using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CodeGame.Classes.Input;
using CodeGame.Classes.Network.Client;
using CodeGame.Classes.Network.Server;
using System.Net;

namespace CodeGame.Classes.Screens {
    class MenuScreen {
        Button btnHost = null;
        Button btnJoin = null;
        Button btnQuit = null;

        Box nameBox = null;
        TextBox nameBoxText = null;
        Button nameBoxCancel = null;
        Button nameBoxOK = null;
        bool nameBoxHasFocus = false;

        Box ipBox = null;
        IPBox2 ipBoxText = null;
        Button ipBoxCancel = null;
        Button ipBoxOK = null;
        bool ipBoxHasFocus = false;
        
        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();

        ScreenManager mgr = null;
        SpriteFont font = null;

        string name = "PlayerName";
        IPAddress ipAddress = IPAddress.None;

        bool joiningGame = false;

        public MenuScreen(ScreenManager mgr) {

            btnHost = new Button(mgr, new Vector2(20, 80), "Host");
            btnJoin = new Button(mgr, new Vector2(20, 160), "Join");
            btnQuit = new Button(mgr, new Vector2(20, 510), "Quit"); 

            //string boxText = "I know what you're thinking. \"Did he fire six shots or only five?\" Well, to tell you the truth, in all this excitement I kind of lost track myself. But being as this is a .44 Magnum, the most powerful handgun in the world, and would blow your head clean off, you've got to ask yourself one question: Do I feel lucky? Well, do ya, punk?";
            string boxText = "Enter your nickname";
            int boxWidth = 500;
            nameBoxCancel = new Button(mgr, "Back");
            nameBoxOK = new Button(mgr, "Accept");
            Button[] btns = new Button[] { nameBoxOK, nameBoxCancel };
            nameBoxText = new TextBox(mgr, boxWidth - 40);
            // 
            nameBox = new Box(mgr, boxText, boxWidth, btns, nameBoxText);

            boxText = "Enter your host's IP address";
            ipBoxCancel = new Button(mgr, "Back");
            ipBoxOK = new Button(mgr, "Join");
            btns = new Button[] { ipBoxOK, ipBoxCancel };
            ipBoxText = new IPBox2(mgr, boxWidth - 40);
            //
            ipBox = new Box(mgr, boxText, boxWidth, btns, ipBoxText);

            this.mgr = mgr;      
            //font = mgr.Content.Load<SpriteFont>("MenuFont");
        }

        public void Update(GameTime gameTime) {

            mouse = Mouse.GetState();

            if (ipBoxHasFocus) {

                bool timeToCloseBox = ipBoxText.Update(gameTime);

                if (timeToCloseBox) {
                    if (ipBoxText.EnterPressed) {
                        // Get IP address
                        nameBoxHasFocus = false;
                    }
                    // Escape was pressed so don't grab ip
                    // Go back to name box
                    ipBoxHasFocus = false;
                }
                else if (HoveringOver(ipBoxCancel)) {
                    if (ClickedOn(ipBoxCancel)) {
                        ipBoxHasFocus = false;
                    }
                    else {
                        ipBoxCancel.IsHover = true;
                    }
                }
                else if (HoveringOver(ipBoxOK)) {
                    if (ClickedOn(ipBoxOK)) {
                        // Get IP address

                        nameBoxHasFocus = false;
                        ipBoxHasFocus = false;
                    }
                    else {
                        ipBoxOK.IsHover = true;
                    }
                }
                else {
                    ipBoxCancel.IsHover = false;
                    ipBoxOK.IsHover = false;
                }
            }
            
            else if (nameBoxHasFocus) {

                bool timeToCloseBox = nameBoxText.Update(gameTime);

                if (timeToCloseBox) {
                    if (nameBoxText.EnterPressed) {
                        name = nameBoxText.GetText();
                        // Does the "Enter IP" box need displaying?
                        if (joiningGame) {
                            if (ipAddress == IPAddress.None) {
                                ipBoxText.SetText("");
                            }
                            else {
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            //ClearMouse();
                            ipBoxHasFocus = true;
                        }
                    }
                    else {
                        // Else: Escape was pressed so don't grab text
                        nameBoxHasFocus = false;
                    }
                }
                else if (HoveringOver(nameBoxCancel)) {
                    if (ClickedOn(nameBoxCancel)) {
                        nameBoxHasFocus = false;
                    }
                    else
                        nameBoxCancel.IsHover = true;
                }
                else if (HoveringOver(nameBoxOK)) {
                    if (ClickedOn(nameBoxOK)) {
                        name = nameBoxText.GetText();
                        // Does the "Enter IP" box need displaying?
                        if (joiningGame) {
                            if (ipAddress == IPAddress.None) {
                                ipBoxText.SetText("");
                            }
                            else {
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            ipBoxHasFocus = true;
                            ResetMouse();
                            return;
                        }
                        nameBoxHasFocus = false;
                    }
                    else {
                        nameBoxOK.IsHover = true;
                    }
                }
                else {
                    nameBoxCancel.IsHover = false;
                    nameBoxOK.IsHover = false;
                }
            }
            else {

                // Host button
                if (HoveringOver(btnHost)) {
                    if (ClickedOn(btnHost)) {
                        nameBoxText.SetText(name);
                        joiningGame = false;
                        nameBoxHasFocus = true;
                    }
                    else {
                        btnHost.IsHover = true;
                    }
                }

                // Join button
                else if (HoveringOver(btnJoin)) {
                    if (ClickedOn(btnJoin)) {
                        nameBoxText.SetText(name);
                        joiningGame = true;
                        nameBoxHasFocus = true;
                    }
                    else {
                        btnJoin.IsHover = true;
                    }
                }

                // Quit button
                else if (HoveringOver(btnQuit)) {
                    if (ClickedOn(btnQuit)) {
                        mgr.Close();
                    }
                    else {
                        btnQuit.IsHover = true;
                    }
                }

                // Reset when not hovering
                else {
                    btnHost.IsHover = false;
                    btnJoin.IsHover = false;
                    btnQuit.IsHover = false;
                }
            }

            prevMouse = mouse;
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {

            graphics.Clear(Color.Black);
            batch.Begin();

            btnHost.Draw(batch);
            btnJoin.Draw(batch);
            btnQuit.Draw(batch);

            if (ipBoxHasFocus) {
                ipBox.Draw(batch);
            }
            else if (nameBoxHasFocus) {
                nameBox.Draw(batch);
            }

            batch.End();

        }



        private bool HoveringOver(Button btn) {
            if (mouse.X >= btn.X1 && mouse.X <= btn.X2 && mouse.Y >= btn.Y1 && mouse.Y <= btn.Y2)
                return true;
            else
                return false;
        }

        private bool ClickedOn(Button btn) {
            //if (mouse.LeftButton == ButtonState.Pressed &&
            //    prevMouse.LeftButton == ButtonState.Released)
            //    return true;
            //else
            //    return false;

            if (prevMouse.LeftButton == ButtonState.Pressed &&
                mouse.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        private void ResetMouse() {
            prevMouse = mouse = new MouseState();
        }
    }
}
