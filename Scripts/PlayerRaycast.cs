using Godot;

namespace Moreus;

public sealed partial class PlayerRaycast : Node2D
{
    [Export] public Player player;
    [Export] public float range = 16f;

    private PhysicsDirectSpaceState2D spaceState;
    private PhysicsRayQueryParameters2D queryParameters2D;
    private float offset_X, offset_Y;
    void UpdateOffset()
    {
        offset_X = GlobalPosition.X % 16;
        offset_Y = GlobalPosition.Y % 16;
        if (offset_X < 0f)
            offset_X += 16f;

        if (offset_Y < 0f)
            offset_Y += 16;

    }
    public override void _Draw()
    {
        if (player != null)
        {
            DrawLine(Vector2.Zero, player.Face * range, new Color(Colors.Aqua));
            DrawCircle(Vector2.Zero - new Vector2(offset_X - 8, offset_Y - 8), 2f, new Color(Colors.Red));
            DrawRect(new Rect2(Vector2.Zero - new Vector2(offset_X, offset_Y), 16f, 16f), new Color(Colors.Coral), false, 2f);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        spaceState = GetWorld2D().DirectSpaceState;

    }
    public override void _Process(double delta)
    {
        if (!(player != null))
            return;
        UpdateOffset();
        if (player.item_hand != null && player.input_action == Action.Item)
        {
            var result = ShapeCast();
            if (result.Count > 0)
            {
                // string message = "Shape Collide with: ";
                // Node2D collider;
                // for (int i = 0; i < result.Count; i++)
                // {
                //     collider = (Node2D)result[i]["collider"];
                //     message += collider.GetOwnerOrNull<Node>().Name + ", ";
                // }

                // GD.Print(message);
            }
            else if (result.Count == 0)
            {
                player.item_hand.Show();
                player.item_hand.Transform = new Transform2D(0f, new Vector2(player.GlobalPosition.X - offset_X + 8, player.GlobalPosition.Y + 13 - offset_Y));
                player.item_hand.DisableCollisions = false;
                player.item_hand = null;
                player.hud.ItemIcon = null;
            }
        }
        Raycast();
        QueueRedraw();
    }

    private Godot.Collections.Array<Godot.Collections.Dictionary> ShapeCast()
    {
        var query = new PhysicsShapeQueryParameters2D
        {
            Shape = new RectangleShape2D { Size = new Vector2(14, 14) },
            Transform = new Transform2D(0f, new Vector2(player.GlobalPosition.X - offset_X + 8, player.GlobalPosition.Y + 13 - offset_Y)),
            CollideWithAreas = true,
            Exclude = [player.GetRid()]
        };

        return spaceState.IntersectShape(query);
    }
    private void Raycast()
    {
        if (player.input_action != Action.None)
        {
            queryParameters2D = new()
            {
                From = GlobalPosition,
                To = GlobalPosition + player.Face * range,
                CollisionMask = 4294967295,
                CollideWithAreas = true,
                CollideWithBodies = true,
                HitFromInside = true,
                Exclude = [player.GetRid()]

            };
            var result = spaceState.IntersectRay(queryParameters2D);
            if (result.Count > 0)
            {
                Node2D collider = (Node2D)result["collider"];
                if (player.input_action == Action.Action)
                    if (collider.GetOwnerOrNull<Node>() is IDamageable<float> damageable)
                        damageable.Damage(1f);
                    else if (collider.GetOwnerOrNull<Node>() is Item item)
                    {
                        if (player.item_hand == null)
                        {
                            player.item_hand = item;
                            player.hud.ItemIcon = player.item_hand.ItemIcon;
                            player.item_hand.Hide();

                            player.item_hand.DisableCollisions = true;
                        }
                    }
            }
        }
    }
}
