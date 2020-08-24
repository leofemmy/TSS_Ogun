using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Drawing;

namespace ColorAsKnownColor {
    class ColorHelper {
        static ColorHelper() {
            knownColorsHash = InitKnownColorsHash();
        }
        private ColorHelper() { }

        private static Hashtable knownColorsHash;

        private static Hashtable InitKnownColorsHash() {
            System.Array knownColors = System.Enum.GetValues(typeof(KnownColor));
            Hashtable hashtable = new Hashtable(knownColors.Length);
            foreach(KnownColor knownColorObject in knownColors) {
                Color color = Color.FromKnownColor(knownColorObject);
                int argbValue = color.ToArgb();
                if(color.IsSystemColor && hashtable.ContainsKey(argbValue))
                    continue;
                hashtable[argbValue] = knownColorObject;
            }
            return hashtable;
        }

        public static bool TryConvertToKnownColor(ref object color) {
            KnownColor knownColor = GetKnownColor(color);
            if(knownColor != 0) {
                color = Color.FromKnownColor(knownColor);
                return true;
            }
            return false;
        }

        public static KnownColor GetKnownColor(Color color) {
            KnownColor knownColor = color.ToKnownColor();
            if(knownColor == 0)
                knownColor = GetKnownColor(color.ToArgb());
            return knownColor;
        }
        public static KnownColor GetKnownColor(int argb) {
            object result = knownColorsHash[argb];
            if(result == null)
                return 0;
            return (KnownColor)result;
        }

        public static KnownColor GetKnownColor(object color) {
            if(color is KnownColor)
                return (KnownColor)color;
            if(color is Color)
                return GetKnownColor((Color)color);
            if(color is int)
                return GetKnownColor((int)color);
            return 0;
        }
    }
}
