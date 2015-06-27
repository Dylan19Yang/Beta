using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Beta.Model
{
    class HotKeyModel
    {
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public bool Win { get; set; }
        public bool Ctrl { get; set; }
        public Key CharKey { get; set; }

        public ModifierKeys ModifierKeys
        {
            get
            {
                ModifierKeys modifierKeys = ModifierKeys.None;
                if (Alt)
                {
                    modifierKeys = ModifierKeys.Alt;
                }
                if (Shift)
                {
                    modifierKeys = modifierKeys | ModifierKeys.Shift;
                }
                if (Win)
                {
                    modifierKeys = modifierKeys | ModifierKeys.Windows;
                }
                if (Ctrl)
                {
                    modifierKeys = modifierKeys | ModifierKeys.Control;
                }
                return modifierKeys;
            }
        }

        public HotKeyModel() { }

        public HotKeyModel(string hotkeyString)
        {
            ParseHotKey(hotkeyString);
        }

        private void ParseHotKey(string hotKeyStr)
        {
            if (!string.IsNullOrEmpty(hotKeyStr))
            {
                List<string> keys = hotKeyStr.Replace(" ", "").Split('+').ToList();
                if (keys.Contains("Alt"))
                {
                    Alt = true;
                    keys.Remove("Alt");
                }
                if (keys.Contains("Shift"))
                {
                    Shift = true;
                    keys.Remove("Shift");
                }
                if (keys.Contains("Win"))
                {
                    Win = true;
                    keys.Remove("Win");
                }
                if (keys.Contains("Ctrl"))
                {
                    Ctrl = true;
                    keys.Remove("Ctrl");
                }
                if (keys.Count > 0)
                {
                    string charKey = keys[0];
                    try
                    {
                        CharKey = (Key)Enum.Parse(typeof(Key), charKey);
                    }
                    catch
                    {

                    }
                }
            }
        }


        public HotKeyModel(bool alt, bool shift, bool win, bool ctrl, Key key)
        {
            Alt = alt;
            Shift = shift;
            Win = win;
            Ctrl = ctrl;
            CharKey = key;
        }

        public override string ToString()
        {
            string text = string.Empty;
            if (Ctrl)
            {
                text += "Ctrl";
            }
            if (Alt)
            {
                text += string.IsNullOrEmpty(text) ? "Alt" : " + Alt";
            }
            if (Shift)
            {
                text += string.IsNullOrEmpty(text) ? "Shift" : " + Shift";
            }
            if (Win)
            {
                text += string.IsNullOrEmpty(text) ? "Win" : " + Win";
            }
            if (!string.IsNullOrEmpty(CharKey.ToString()))
            {
                text += string.IsNullOrEmpty(text) ? CharKey.ToString() : " + " + CharKey;
            }

            return text;
        }

    }
}
