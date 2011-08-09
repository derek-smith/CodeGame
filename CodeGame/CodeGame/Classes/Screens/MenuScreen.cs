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
        Button btnCredits = null;

        Box nameBox = null;
        TextBox nameBoxText = null;
        Button nameBoxCancel = null;
        Button nameBoxOK = null;
        bool nameBoxHasFocus = false;

        Box ipBox = null;
        IPBox ipBoxText = null;
        Button ipBoxCancel = null;
        Button ipBoxOK = null;
        bool ipBoxHasFocus = false;

        Box creditsBox = null;
        Button creditsBoxOK = null;
        bool creditsBoxHasFocus = false;

        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();

        ScreenManager mgr = null;
        SpriteFont font = null;

        string nick = "PlayerName";
        IPAddress ipAddress = IPAddress.None;

        bool joiningGame = false;

        public MenuScreen(ScreenManager mgr) {

            btnHost = new Button(mgr, new Vector2(40, 80), "Host");
            btnJoin = new Button(mgr, new Vector2(40, 140), "Join");
            btnCredits = new Button(mgr, new Vector2(40, 200), "Credits");
            btnQuit = new Button(mgr, new Vector2(40, 600 - 40 - 49), "Quit"); 

            //string boxText = "I know what you're thinking. \"Did he fire six shots or only five?\" Well, to tell you the truth, in all this excitement I kind of lost track myself. But being as this is a .44 Magnum, the most powerful handgun in the world, and would blow your head clean off, you've got to ask yourself one question: Do I feel lucky? Well, do ya, punk?";
            string boxText = "What would you like for your nickname?";
            int boxWidth = 500;
            nameBoxCancel = new Button(mgr, "Back");
            nameBoxOK = new Button(mgr, "Accept");
            Button[] btns = new Button[] { nameBoxOK, nameBoxCancel };
            nameBoxText = new TextBox(mgr, boxWidth - 40);
            // 
            nameBox = new Box(mgr, boxText, boxWidth, btns, nameBoxText);

            boxText = "What is your host's IP address?";
            ipBoxCancel = new Button(mgr, "Back");
            ipBoxOK = new Button(mgr, "Join");
            btns = new Button[] { ipBoxOK, ipBoxCancel };
            ipBoxText = new IPBox(mgr, boxWidth - 40);
            //
            ipBox = new Box(mgr, boxText, boxWidth, btns, ipBoxText);

            boxText = "Created by Derek, of course!";
            creditsBoxOK = new Button(mgr, "Okay");
            btns = new Button[] { creditsBoxOK };
            //
            creditsBox = new Box(mgr, boxText, boxWidth, btns);

            this.mgr = mgr;      
            //font = mgr.Content.Load<SpriteFont>("MenuFont");
        }

        public void Update(GameTime gameTime) {

            mouse = Mouse.GetState();

            //
            // Credits box has focus
            //

            if (creditsBoxHasFocus) {
                if (HoveringOver(creditsBoxOK)) {
                    if (ClickedOn(creditsBoxOK)) {
                        creditsBoxHasFocus = false;
                    }
                    else {
                        creditsBoxOK.IsHover = true;
                    }
                }
                else {
                    creditsBoxOK.IsHover = false;
                }
            }

            //
            // "Enter your IP" box has focus
            //

            else if (ipBoxHasFocus) {

                bool timeToCloseBox = ipBoxText.Update(gameTime);

                if (timeToCloseBox) {
                    if (ipBoxText.EnterPressed) {
                        // Get IP address
                        if (!IPAddress.TryParse(ipBoxText.GetText(), out ipAddress)) {
                            // invalid IP
                            ipAddress = IPAddress.None;
                        }
                        // Close both boxes, starting with this one
                        nameBoxHasFocus = false;
                    }
                    // Escape was pressed so don't grab ip
                    else {
                        nameBoxText.SetText(nick);
                    }
                    // Close box
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
            
            //
            // "Enter your nickname" box has focus
            //

            else if (nameBoxHasFocus) {

                bool timeToCloseBox = nameBoxText.Update(gameTime);

                if (timeToCloseBox) {
                    if (nameBoxText.EnterPressed) {
                        nick = nameBoxText.GetText();
                        // Does the "Enter IP" box need displaying?
                        // aka Joining a game
                        if (joiningGame) {
                            if (ipAddress == IPAddress.None) {
                                ipBoxText.SetText("");
                            }
                            else {
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            //ClearMouse();
                            ipBoxHasFocus = true;
                            return;
                        }
                        else {
                            // Hosting a game
                            mgr.ChangeToLobbyScreen(nick);
                        }
                    }
                    // Escape pressed
                    nameBoxHasFocus = false;
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
                        nick = nameBoxText.GetText();
                        // Does the "Enter IP" box need displaying?
                        // aka Joining a game
                        if (joiningGame) {
                            if (ipAddress == IPAddress.None) {
                                ipBoxText.SetText("");
                            }
                            else {
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            ipBoxHasFocus = true;
                            // This is needed because the "Enter IP" box thinks
                            // it's being clicked as the "Accept" btn on the
                            // "Enter nick" box is clicked
                            ResetMouse();
                            return;
                        }
                        else {
                            // Hosting a game
                            mgr.ChangeToLobbyScreen(nick);
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

            //
            // Menu has focus
            //

            else {

                // Host button
                if (HoveringOver(btnHost)) {
                    if (ClickedOn(btnHost)) {
                        nameBoxText.SetText(nick);
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
                        nameBoxText.SetText(nick);
                        joiningGame = true;
                        nameBoxHasFocus = true;
                    }
                    else {
                        btnJoin.IsHover = true;
                    }
                }

                // Credits button
                else if (HoveringOver(btnCredits)) {
                    if (ClickedOn(btnCredits)) {
                        creditsBoxHasFocus = true;
                    }
                    else {
                        btnCredits.IsHover = true;
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
                    btnCredits.IsHover = false;
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
            btnCredits.Draw(batch);
            btnQuit.Draw(batch);

            if (creditsBoxHasFocus) {
                creditsBox.Draw(batch);
            }
            else if (ipBoxHasFocus) {
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
