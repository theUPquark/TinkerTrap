using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EditorTile : MonoBehaviour
{
    public struct variables
    {
        public int x;
        public int y;
        public int tileType;
        public int obsType;
        public List<int> connections;
        public List<int> lockGroups;

        public variables(int a, int b)
        {
            x = a;
            y = b;
            tileType = 1;
            obsType = 0;
            connections = new List<int>();
            lockGroups = new List<int>();
        }
        public variables(variables source) //copy constructor
        {
            x = source.x;
            y = source.y;
            tileType = source.tileType;
            obsType = source.obsType;
            connections = new List<int>(source.connections);
            lockGroups = new List<int>(source.lockGroups);
        }
    }
	
    public variables tileData;

    public EditorTile() : this(0, 0) { } //Might not need after adding getter/setters

    public EditorTile(int a, int b) //: base()
    {
        tileData = new variables(a, b);

        /*this.SetBounds(25 + a * 25, 40 + b * 25, 25, 25); // Location on window and size
        this.Text = tileData.tileType.ToString();*/
    }

    public void setTileType(int t)   {
        tileData.tileType = t;
    }
    public int getTileType()   {
        return tileData.tileType;
    }

    public void setObsType(int t)
    {
        tileData.obsType = t;
    }
    public int getObsType()
    {
        return tileData.obsType;
    }
    public int getTileX()
    {
        return tileData.x;
    }
    public int getTileY()
    {
        return tileData.y;
    }

    public string getAllConnections()
    {
        string all = null;
        foreach (int s in tileData.connections)
            all = all + " " + Convert.ToString(s);
        return all;
    }
    public void setConnections(string text)
    {
        string[] splitText = text.Split(null);
        if (text.Length > 0)
            tileData.connections.Clear();
        foreach (string str in splitText)
            if (str.Length > 0)
                tileData.connections.Add(int.Parse(str));
    }

    public string getAllLockGroups()
    {
        string all = null;
        foreach (int s in tileData.lockGroups)
            all = all + " " + Convert.ToString(s);
        return all;
    }
    public void setLockGroups(string text)
    {
        string[] splitText = text.Split(null);
        if (text.Length > 0)
            tileData.lockGroups.Clear();
        foreach (string str in splitText)
            if (str.Length > 0)
                tileData.lockGroups.Add(int.Parse(str));
    }
}