using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeGame {

    static class TheMouse {

        static MouseState mouse = new MouseState();

        public delegate void MouseClickHandler();
        public delegate void MouseOverHandler();
        public delegate void MouseOutHandler();

        static List<Subscriber> subscribers = new List<Subscriber>(); 
        
        static bool clickIsLocked = false;

        public static void Update() {
            mouse = Mouse.GetState();
            bool lockClickAfterLoop = false;

            if (clickIsLocked) {
                if (mouse.LeftButton == ButtonState.Released) {
                    clickIsLocked = false;
                }
            }

            for (int i = 0; i < subscribers.Count; i++) {
                if (IsMouseOver(subscribers[i].Rectangle)) {
                    subscribers[i].MouseOver();

                    if (!clickIsLocked) {
                        if (mouse.LeftButton == ButtonState.Pressed) {
                            lockClickAfterLoop = true;
                            subscribers[i].MouseClick();
                        }
                    }
                }
                else {
                    subscribers[i].MouseOut();
                }
            }

            if (lockClickAfterLoop) {
                clickIsLocked = true;
            }
        }

        private static bool IsMouseOver(Rectangle rect) {

            if (mouse.X >= rect.Left && mouse.X <= rect.Right &&
                mouse.Y >= rect.Top && mouse.Y <= rect.Bottom) {
                return true;
            }
            else {
                return false;
            }
        }

        public static void SetRectangle(Object o, Rectangle rect) {
            for (int i = 0; i < subscribers.Count; i++) {
                if (subscribers[i].Object == o) {
                    subscribers[i].Rectangle = rect;
                }
            }
        }

        public static void Subscribe(Object o, Rectangle rect, MouseOverHandler mouseOver, MouseOutHandler mouseOut, MouseClickHandler mouseClick) {

            subscribers.Add(new Subscriber(o, rect, mouseOver, mouseOut, mouseClick));
        }

        public static void Unsubscribe(Object o) {

            for (int i = 0; i < subscribers.Count; i++) {
                if (subscribers[i].Object == o) {

                    subscribers.RemoveAt(i);
                    return;
                }
            }
        }

        class Subscriber {
            Object o;
            Rectangle rect;
            MouseOverHandler mouseOver;
            MouseOutHandler mouseOut;
            MouseClickHandler mouseClick;

            public Subscriber(Object o, Rectangle rect, MouseOverHandler mouseOver, MouseOutHandler mouseOut, MouseClickHandler mouseClick) {
                this.o = o;
                this.rect = rect;
                this.mouseOver = mouseOver;
                this.mouseOut = mouseOut;
                this.mouseClick = mouseClick;
            }

            public Object Object { get { return o; } }
            public Rectangle Rectangle { get { return rect; } set { rect = value; } }
            public MouseOverHandler MouseOver { get { return mouseOver; } }
            public MouseOutHandler MouseOut { get { return mouseOut; } }
            public MouseClickHandler MouseClick { get { return mouseClick; } }
        }
    }



   
}
