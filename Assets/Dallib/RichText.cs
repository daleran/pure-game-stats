﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaleranGames.UI
{
    public static class RichText 
    {
        public static string BoldString (string str)
        {
            return "<b>" + str + "</b>";
        }

        public static string ItalicString (string str)
        {
            return "<i>" + str + "</i>";
        }

        public static string SetStringSize (string str, int size)
        {
            return "<size="+size.ToString()+">" + str + "</size>";
        }

        public static string SetStringColor (string str, Color textColor)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(textColor) + ">" + str + "</color>";
        }

    }

}
