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

        //
        // Public information: name (and ip if joining a game)
        //

        string nick = "PlayerName";
        IPAddress ipAddress = IPAddress.None;

        //
        // Terrible hack - search this class for it
        //
        bool joiningGame = false;

        //
        // Properties
        //

        public string Nick { get { return nick; } }
        public IPAddress IPAddress { get { return ipAddress; } }

        //
        // Constructor
        //

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
        }

        //
        // Update
        //

        public void Update(GameTime gameTime) {

            // Save the mouse's state
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

                //
                // Else
                //

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
                // - the Enter or Escape key was pressed
                if (timeToCloseBox) {

                    // Was it the Enter key?
                    if (ipBoxText.EnterPressed) {

                        // Good IP address?
                        if (!IPAddress.TryParse(ipBoxText.GetText(), out ipAddress)) {
                            // Failure: bad IP
                            ipAddress = IPAddress.None;
                        }
                        // Success

                        // Hide "Enter Nick" box
                        // ("Enter IP" box is hidden next, jump down and see)
                        nameBoxHasFocus = false;
                        
                        //
                        // Join a game
                        //

                        mgr.ChangeToLobbyScreen(false);
                    }
                    // Escape key pressed
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
                        // Clicked

                        // Hide "Enter IP" box so the "Enter Nick" box will show again
                        // (because it's hidden while "Enter IP" box is visible)
                        ipBoxHasFocus = false;
                    }
                    else {
                        // Hovering
                        ipBoxCancel.IsHover = true;
                    }
                }

                //
                // OK button
                //

                else if (HoveringOver(ipBoxOK)) {
                    if (ClickedOn(ipBoxOK)) {
                        // Clicked

                        // Good IP address?
                        if (!IPAddress.TryParse(ipBoxText.GetText(), out ipAddress)) {
                            // Failure: bad IP
                            ipAddress = IPAddress.None;
                        }
                        // Success

                        // Hide "Enter IP" and "Enter Nick" boxes
                        nameBoxHasFocus = false;
                        ipBoxHasFocus = false;

                        //
                        // Join a game
                        //

                        mgr.ChangeToLobbyScreen(false);
                    }
                    else {
                        // Hovering
                        ipBoxOK.IsHover = true;
                    }
                }

                //
                // Else
                //

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

                // Update box
                bool timeToCloseBox = nameBoxText.Update(gameTime);

                // This is true if:
                // - the Enter or Escape key was pressed
                if (timeToCloseBox) {

                    // Was it the Enter key?
                    if (nameBoxText.EnterPressed) {

                        // Save new nick
                        nick = nameBoxText.GetText();

                        //
                        // Are we joining a game? Then show "Enter IP" box next
                        //

                        if (joiningGame) {

                            if (ipAddress == IPAddress.None) {
                                // Prepare "Enter IP" box to be shown next
                                ipBoxText.SetText("");
                            }
                            else {
                                // Prepare "Enter IP" box to be shown next
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            // Show "Enter IP" box
                            ipBoxHasFocus = true;

                            // Return so "Enter Nick" box remains visible
                            // but hidden by "Enter IP" box
                            return;
                        }

                        //
                        // Hosting a game
                        //

                        else {
                            // Connect to local server
                            ipAddress = IPAddress.Loopback;

                            mgr.ChangeToLobbyScreen(true);
                        }
                    }

                    // Escape key pressed so
                    // hide "Enter Nick" box
                    nameBoxHasFocus = false;
                }

                //
                // Cancel button
                //

                else if (HoveringOver(nameBoxCancel)) {
                    if (ClickedOn(nameBoxCancel)) {
                        // Clicked
                        nameBoxHasFocus = false;
                    }
                    else
                        // Hovering
                        nameBoxCancel.IsHover = true;
                }

                //
                // OK button
                //

                else if (HoveringOver(nameBoxOK)) {
                    if (ClickedOn(nameBoxOK)) {
                        // Clicked

                        // Save new nick
                        nick = nameBoxText.GetText();

                        //
                        // Are we joining a game? Then show "Enter IP" box next
                        //

                        if (joiningGame) {

                            if (ipAddress == IPAddress.None) {
                                // Prepare "Enter IP" box to be shown next
                                ipBoxText.SetText("");
                            }
                            else {
                                // Prepare "Enter IP" box to be shown next
                                ipBoxText.SetText(ipAddress.ToString());
                            }
                            // Show "Enter IP" box
                            ipBoxHasFocus = true;

                            // This is needed because the "Enter IP" box thinks
                            // it's being clicked as the "Accept" btn on the
                            // "Enter Nick" box is clicked (just another hack!)
                            ResetMouse();
                        }
                        
                        //
                        // Hosting a game
                        //

                        else {
                            // Hide "Enter Nick" box
                            nameBoxHasFocus = false;

                            // Connect to local server
                            ipAddress = IPAddress.Loopback;

                            mgr.ChangeToLobbyScreen(true);
                        }
                    }
                    else {
                        // Hovering
                        nameBoxOK.IsHover = true;
                    }
                }

                //
                // Else
                //

                else {
                    // Neither clicking nor hovering
                    // so lets reset the hover state
                    nameBoxCancel.IsHover = false;
                    nameBoxOK.IsHover = false;
                }
            }

            //
            // Menu has focus
            //

            else {

                //
                // Host button
                //

                if (HoveringOver(btnHost)) {
                    if (ClickedOn(btnHost)) {
                        // Clicked
                        nameBoxText.SetText(nick);
                        joiningGame = false;
                        nameBoxHasFocus = true;
                    }
                    else {
                        // Hovering
                        btnHost.IsHover = true;
                    }
                }

                //
                // Join button
                //

                else if (HoveringOver(btnJoin)) {
                    if (ClickedOn(btnJoin)) {
                        // Clicked
                        nameBoxText.SetText(nick);
                        joiningGame = true;
                        nameBoxHasFocus = true;
                    }
                    else {
                        // Hovering
                        btnJoin.IsHover = true;
                    }
                }

                //
                // Credits button
                //

                else if (HoveringOver(btnCredits)) {
                    if (ClickedOn(btnCredits)) {
                        // Clicked
                        creditsBoxHasFocus = true;
                    }
                    else {
                        // Hovering
                        btnCredits.IsHover = true;
                    }
                }

                //
                // Quit button
                //

                else if (HoveringOver(btnQuit)) {
                    if (ClickedOn(btnQuit)) {
                        // Clicked
                        mgr.Close();
                    }
                    else {
                        // Hovering
                        btnQuit.IsHover = true;
                    }
                }

                //
                // Else
                //

                else {
                    // Neither clicking nor hovering
                    // so lets reset the hover state
                    btnHost.IsHover = false;
                    btnJoin.IsHover = false;
                    btnCredits.IsHover = false;
                    btnQuit.IsHover = false;
                }
            }

            // Save the mouse's state for next Update()
            prevMouse = mouse;
        }

        //
        // Draw
        //

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
