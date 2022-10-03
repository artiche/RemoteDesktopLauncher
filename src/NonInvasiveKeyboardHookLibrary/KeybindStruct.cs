// https://github.com/kfirprods/NonInvasiveKeyboardHook
//
// MIT License
// 
// Copyright (c) 2018 Kfir Eichenblat
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.



using System;
using System.Collections.Generic;

namespace NonInvasiveKeyboardHookLibrary
{
    /// <summary>
    /// A struct to represent a keybind (key + modifiers)
    /// When the keybind struct is compared to another keybind struct, the equality is based on the
    /// modifiers and the virtual key code.
    /// </summary>
    internal class KeybindStruct : IEquatable<KeybindStruct>
    {
        public readonly int VirtualKeyCode;
        public readonly List<ModifierKeys> Modifiers;
        public readonly Guid? Identifier;

        public KeybindStruct(IEnumerable<ModifierKeys> modifiers, int virtualKeyCode, Guid? identifier = null)
        {
            this.VirtualKeyCode = virtualKeyCode;
            this.Modifiers = new List<ModifierKeys>(modifiers);
            this.Identifier = identifier;
        }

        public bool Equals(KeybindStruct other)
        {
            if (other == null)
                return false;

            if (this.VirtualKeyCode != other.VirtualKeyCode)
                return false;

            if (this.Modifiers.Count != other.Modifiers.Count)
                return false;

            foreach (var modifier in this.Modifiers)
            {
                if (!other.Modifiers.Contains(modifier))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((KeybindStruct)obj);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + VirtualKeyCode.GetHashCode();

            // Sum the modifiers separately, so that their order does not affect the final hash
            var modifiersHashSum = 0;
            foreach (var modifier in this.Modifiers)
            {
                modifiersHashSum += modifier.GetHashCode();
            }

            hash = (hash * 7) + modifiersHashSum;

            return hash;
        }
    }
}