/* Copyright (c) 2011 Chris Chen. 
 * All rights reserved. This program and the accompanying materials 
 * are made available under the terms of the Eclipse Public License v1.0 
 * which accompanies this distribution, and is available at 
 * http://www.eclipse.org/legal/epl-v10.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Solvet
{
    static class Utils
    {
        /// <summary>
        /// Extracts the string that is inbetween the start and end strings
        /// </summary>
        /// <param name="start">Start string</param>
        /// <param name="end">End string</param>
        /// <param name="input">The original string</param>
        /// <returns></returns>
        public static string ExtractString(string start, string end, string input)
        {
            string retVal = "";
            int index = input.IndexOf(start);
            if (index >= 0)
            {
                retVal = input.Remove(0, index + start.Length);
                index = retVal.IndexOf(end);
                if (index >= 0)
                {
                    retVal = retVal.Remove(index);
                    return retVal;
                }
                return "";
            }
            return "";
        }

        /// <summary>
        /// Removes all html characters from the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Clean string</returns>
        public static string CleanHTML(string input)
        {
            string retVal = input.Replace("\\x26#215;", "×");
            retVal = retVal.Replace("\\x3csup\\x3e", "^");
            retVal = retVal.Replace("\\x3c/sup\\x3e", " ");
            retVal = retVal.Replace("\\x22", "\"");
            retVal = retVal.Replace("\\x3d", "=");
            retVal = retVal.Replace("Â£", "£");
            retVal = System.Net.WebUtility.HtmlDecode(retVal);
            retVal = retVal.Replace("<br>", "");
            return retVal;
        }

        /// <summary>
        /// Removes spaces inbetween numbers. E.g 12 300 -> 12300 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveSpacesFromNumbers(string input)
        {
            string retVal = "";
            char previousChar = Char.MinValue;
            foreach (char item in input)
            {
                if (!Char.IsWhiteSpace(item))
                {
                    retVal += item;
                }
                else if (!Char.IsNumber(previousChar))
                {
                    retVal += " ";
                }
                previousChar = item;
            }
            return retVal;
        }
    }
}
