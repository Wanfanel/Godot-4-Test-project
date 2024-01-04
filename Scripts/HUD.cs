using Godot;
namespace Moreus;
public sealed partial class HUD : Control
{
    [Export]
    public Texture2D ItemIcon
    {
        get
        {
            var childlist = GetChildren(true);
            foreach (Node node in childlist)
                if (node is TextureRect textureRect)
                    return textureRect.Texture;
            return null;
        }
        set
        {

            var childlist = GetChildren(true);
            foreach (Node node in childlist)
                if (node is TextureRect textureRect)
                    textureRect.Texture = value;



        }
    }
}
