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

namespace CodeGame.Classes.Screens {
    class MenuScreen {
        Button btnHost = null;
        Button btnJoin = null;
        Button btnQuit = null;

        Box nameBox = null;
        TextBox nameBoxText = null;
        Button nameBoxCancel = null;
        Button nameBoxOK = null;

        Box ipBox = null;

        bool boxHasFocus = false;

        MouseState mouse = new MouseState();

        ScreenManager mgr;
        SpriteFont font;

        // 
        //
        //
        string _name;
        string _ipAddress;

        public string PlayerName { get { return _name; } set { _name = value; } }
        public string IPAddress { get { return _ipAddress; } set { _ipAddress = value; } }

        public MenuScreen(ScreenManager mgr) {

            btnHost = new Button(mgr, new Vector2(20, 80), "Host");
            btnJoin = new Button(mgr, new Vector2(20, 160), "Join");
            btnQuit = new Button(mgr, new Vector2(20, 510), "Quit");

            string boxText = "I know what you're thinking. \"Did he fire six shots or only five?\" Well, to tell you the truth, in all this excitement I kind of lost track myself. But being as this is a .44 Magnum, the most powerful handgun in the world, and would blow your head clean off, you've got to ask yourself one question: Do I feel lucky? Well, do ya, punk?";
            int boxWidth = 500;
            nameBoxCancel = new Button(mgr, "Back");
            nameBoxOK = new Button(mgr, "Accept");
            Button[] btns = new Button[] { nameBoxOK, nameBoxCancel };
            nameBoxText = new TextBox(mgr, "Testing", boxWidth - 40);
            // 
            nameBox = new Box(mgr, boxText, boxWidth, btns, nameBoxText);

            PlayerName = "dsmith";
            IPAddress = "127.0.0.1";

            this.mgr = mgr;
          
            font = mgr.Content.Load<SpriteFont>("MenuFont");
        }

        public void Update(GameTime gameTime) {

            mouse = Mouse.GetState();




            if (boxHasFocus) {

                nameBoxText.Update(gameTime);

                if (HoveringOver(nameBoxCancel)) {
                    if (ClickingOn(nameBoxCancel))
                        boxHasFocus = false;
                    else
                        nameBoxCancel.IsHover = true;
                }
                else if (HoveringOver(nameBoxOK)) {
                    if (ClickingOn(nameBoxOK))
                        boxHasFocus = false;
                    else
                        nameBoxOK.IsHover = true;
                }
                else {
                    nameBoxCancel.IsHover = false;
                    nameBoxOK.IsHover = false;
                }


            }
            else {

                // Host button
                if (HoveringOver(btnHost)) {
                    if (ClickingOn(btnHost))
                        boxHasFocus = true;
                    else
                        btnHost.IsHover = true;
                }
                // Join button
                else if (HoveringOver(btnJoin)) {
                    if (ClickingOn(btnJoin))
                        ;// mouse click
                    else
                        btnJoin.IsHover = true;
                }
                // Quit button
                else if (HoveringOver(btnQuit)) {
                    if (ClickingOn(btnQuit))
                        mgr.Close();
                    else
                        btnQuit.IsHover = true;
                }
                // Reset when not hovering
                else {
                    btnHost.IsHover = false;
                    btnJoin.IsHover = false;
                    btnQuit.IsHover = false;
                }

            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {

            graphics.Clear(Color.Black);
            batch.Begin();

            btnHost.Draw(batch);
            btnJoin.Draw(batch);
            btnQuit.Draw(batch);

            if (boxHasFocus) {
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

        private bool ClickingOn(Button btn) {
            if (mouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }
    }
}
