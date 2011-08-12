using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CodeGame {
    static class Globals {

        static Dictionary<string, object> globals = null;

        public static Dictionary<string, object> Get() {
            if (globals == null)
                globals = new Dictionary<string, object>();

            return globals;
        }        
    }
}
