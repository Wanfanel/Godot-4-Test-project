using Godot;

public partial class PlayerRaycast : Node2D
{
    public override void _Draw()
    {
        DrawLine(Vector2.Zero, Vector2.Up * 40, new Color(Colors.Aqua));
    }
    public override void _Process(double delta)
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + Vector2.Up * 40);
        query.CollideWithAreas = true;
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            Node2D collider = (Node2D)result["collider"];
            if (collider.GetOwnerOrNull<Node2D>() is Moreus.IDamageable<float>)
                GD.Print("Hit a IDamageable");
        }
        QueueRedraw();
    }
}
