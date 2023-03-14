using Godot;
using System;

public sealed partial class HUD : Control
{
     [Export]
    public Texture2D ItemIcon
    {
        get
        {
            var childlist = GetChildren(true);
            foreach (Node node in childlist)
                if (node is TextureRect)
                    return ((TextureRect)node).Texture;
            return null;
        }
        set
        {

            var childlist = GetChildren(true);
            foreach (Node node in childlist)
                if (node is TextureRect)
                    ((TextureRect)node).Texture = value;

        }
    }
}
