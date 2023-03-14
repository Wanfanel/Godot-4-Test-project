using Godot;

namespace Moreus;

public sealed partial class Apple_tree : Tree, IHarvestable
{
    public void Harvest()
    {
        GD.Print("DEBUG: Harvesting");
    }
}
