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

        //
        // Menu buttons
        //

        Button btnHost = null;
        Button btnJoin = null;
        Button btnQuit = null;
        Button btnCredits = null;

        //
        // "Enter nickname" box and associated items
        //

        Box nameBox = null;
        TextBox nameBoxText = null;
        Button nameBoxCancel = null;
        Button nameBoxOK = null;
        bool nameBoxHasFocus = false;

        //
        // "Enter IP" box and associated items
        //

        Box ipBox = null;
        IPBox ipBoxText = null;
        Button ipBoxCancel = null;
        Button ipBoxOK = null;
        bool ipBoxHasFocus = false;

        //
        // Credits box and associated items
        //

        Box creditsBox = null;
        Button creditsBoxOK = null;
        bool creditsBoxHasFocus = false;

        //
        // Mouse-related globals
        //

        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();

        // Reference to the ScreenManager
        // which gives us access to everything
        ScreenManager mgr = null;

        // Font used for drawing strings
        SpriteFont font = null;

        //
        // Useful information: name and ip (for joining a game)
        //

        string nick = "PlayerName";
        IPAddress ipAddress = IPAddress.None;

        // Terrible hack - search this class for it
        bool joiningGame = false;

        public string Nick { get { return nick; } }
        public IPAddress IPAddress { get { return ipAddress; } }

        public MenuScreen(ScreenManager mgr) {

            // Create all the buttons
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

                //
                // OK button
                //

                if (HoveringOver(creditsBoxOK)) {
                    if (ClickedOn(creditsBoxOK)) {
                        // Clicked
                        creditsBoxHasFocus = false;
                    }
                    else {
                        // Hovering
                        creditsBoxOK.IsHover = true;
                    }
                }
                else {
                    // Neither clicking nor hovering
                    // so lets reset the hover state
                    creditsBoxOK.IsHover = false;
                }
            }

            //
            // "Enter IP" box has focus
            //

            else if (ipBoxHasFocus) {

                // Update box
                bool timeToCloseBox = ipBoxText.Update(gameTime);

                // This is true if:
                // - the Enter or the Escape key was pressed
                if (timeToCloseBox) {

                    // Was it the Enter key?
                    if (ipBoxText.EnterPressed) {

                        // Good IP address?
                        if (!IPAddress.TryParse(ipBoxText.GetText(), out ipAddress)) {
                            // Failure: bad IP
                            ipAddress = IPAddress.None;
                        }
                        // Success

                        // TODO: move forward!
                        
                        // Hide "Enter Nick" box
                        nameBoxHasFocus = false;
                    }
                    // Nope, it was the Escape key
                    else {
                        // Prepare "Enter Nick" box to be shown again
                        // (technicially, it's still visible, underneith)
                        nameBoxText.SetText(nick);
                    }
                    // Hide "Enter IP" box (to show the "Enter Nick" box)
                    ipBoxHasFocus = false;
                }

                //
                // Cancel button
                //

                else if (HoveringOver(ipBoxCancel)) {
                    if (ClickedOn(ipBoxCancel)) {
                        // Click
                        ipBoxHasFocus = false;
                    }
                    else {
                        // Hovering
                        ipBoxCancel.IsHover = true;
                    }
                }
                else if (HoveringOver(ipBoxOK)) {
                    if (ClickedOn(ipBoxOK)) {
                        // Clicked

                        // Get IP address

                        nameBoxHasFocus = false;
                        ipBoxHasFocus = false;
                    }
                    else {
                        // Hovering
                        ipBoxOK.IsHover = true;
                    }
                }
                else {
                    // Neither clicking nor hovering
                    // so lets reset the hover state
                    ipBoxCancel.IsHover = false;
                    ipBoxOK.IsHover = false;
                }
            }
            
            //
            // "Enter Nick" box has focus
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
                            mgr.ChangeToLobbyScreen();
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
                            mgr.ChangeToLobbyScreen();
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
