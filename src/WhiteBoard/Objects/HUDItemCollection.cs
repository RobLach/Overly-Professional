#region Using Statements
using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WhiteBoard
{
    
    public class HUDItemCollection
    {
        ArrayList hudCollect;
        int size;

        public HUDItemCollection()
        {
            hudCollect = new ArrayList();
            size = 0;
        }

        public void add(HUDItem hItem)
        {
            hudCollect.Add(hItem);
            size++;
        }

        public int getSize()
        {
            return size;
        }

        public HUDItem get(int x)
        {
            return (HUDItem)hudCollect[x];
        }

        public void remove(HUDItem hItem)
        {
            hudCollect.Remove(hItem);
            size--;
        }
    }
}
