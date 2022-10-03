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

namespace NonInvasiveKeyboardHookLibrary
{
    [Flags]
    public enum ModifierKeys
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        WindowsKey = 8,
    }

    public static class ModifierKeysUtilities
    {
        public static ModifierKeys? GetModifierKeyFromCode(int keyCode)
        {
            switch (keyCode)
            {
                case 0xA0:
                case 0xA1:
                case 0x10:
                    return ModifierKeys.Shift;

                case 0xA2:
                case 0xA3:
                case 0x11:
                    return ModifierKeys.Control;

                case 0x12:
                case 0xA4:
                case 0xA5:
                    return ModifierKeys.Alt;

                case 0x5B:
                case 0x5C:
                    return ModifierKeys.WindowsKey;

                default:
                    return null;
            }
        }
    }
}