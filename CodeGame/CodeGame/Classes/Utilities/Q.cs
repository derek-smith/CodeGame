using System;
using System.Collections.Generic;

namespace CodeGame.Classes.Utilities {
    class Q<T> {
        // Internal collection
        Queue<T> _items = new Queue<T>();
        // Multi-thread lock object
        Object _theLock = new object();

        public Q() {
        }

        public void Push(T item) {
            lock (_theLock) {
                _items.Enqueue(item);
            }
        }

        public bool Pop(out T item) {
            item = default(T);
            lock (_theLock) {
                if (_items.Count > 0) {
                    item = _items.Dequeue();
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }
}
