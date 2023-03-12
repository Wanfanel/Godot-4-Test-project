using Godot;

namespace Moreus;

public partial class PlayerRaycast : Node2D
{
    [Export] public Player player;
    [Export] public float range = 10f;
    public override void _Draw()
    {
        if (player != null)
            DrawLine(Vector2.Zero, player.Face * range, new Color(Colors.Aqua));
    }
    public override void _Process(double delta)
    {
        if (!(player != null))
            return;

        if (player.input_action != Action.None)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + player.Face * range);
            query.CollideWithAreas = true;
            var result = spaceState.IntersectRay(query);
            if (result.Count > 0)
            {
                Node2D collider = (Node2D)result["collider"];
                if (player.input_action == Action.Action)
                    if (collider.GetOwnerOrNull<Node2D>() is IDamageable<float>)
                        ((IDamageable<float>)collider).Damage(1f);
            }

        }
        QueueRedraw();
    }
}
