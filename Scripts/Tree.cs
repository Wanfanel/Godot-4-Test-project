using Godot;

namespace Moreus;

public partial class Tree : Node2D, Moreus.IDamageable<float>
{
    public void Damage(float damageTaken)
    {
        GD.Print("DEBUG: Damage " + this.GetType().ToString());
    }

}