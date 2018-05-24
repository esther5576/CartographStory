using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeDraw
{
    // Helper methods used to set drawing settings
    public class DrawingSettings : MonoBehaviour
    {
        public static bool isCursorOverUI = false;


        // Changing pen settings is easy as changing the static properties Pen_Colour and Pen_Width
        public void SetMarkerColour(Color new_color)
        {
            Drawable.Pen_Colour = new_color;
        }
        // new_width is radius in pixels
        public void SetMarkerWidth(int new_width)
        {
            Drawable.Pen_Width = new_width;
        }
        public void SetMarkerWidth(float new_width)
        {
            SetMarkerWidth((int)new_width);
        }


        // Call these these to change the pen settings
        public void SetMarkerRed()
        {
            SetMarkerColour(Color.black);
        }
        public void SetMarkerGreen()
        {
            SetMarkerColour(Color.green);
        }
        public void SetMarkerBlue()
        {
            SetMarkerColour(Color.blue);
        }
        public void SetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 1f));
        }
    }
}