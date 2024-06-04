using Godot;

namespace Moreus;
public sealed partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 100.0f;
    [Export] public Sprite2D basicSprite;
    [Export] public Sprite2D actionSprite;
    [Export] public Tool activeTool = Tool.Scythe;

    private Vector2 direction;
    public Action input_action;
    public Item item_hand = null;
    [Export] public HUD hud;

    private int frame_X, frame_Y, frame_offset;

    public Vector2 Face => frame_Y switch
    {
        0 => Vector2.Down,
        1 => Vector2.Up,
        2 => Vector2.Left,
        3 => Vector2.Right,
        _ => Vector2.Zero,
    };

    public void GetInput()
    {
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        input_action = Input.IsAnythingPressed() switch
        {
            true when Input.IsActionPressed("ui_select") => Action.Action,
            true when Input.IsActionPressed("item") => Action.Item,
            true when Input.IsActionPressed("pickup") => Action.Pickup,
            _ => Action.None,
        };

        Velocity = direction != Vector2.Zero && input_action == Action.None ? direction * Speed : new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
    }

    public void Animate()
    {
        if (basicSprite == null || actionSprite == null) return;

        frame_X = (int)Time.GetTicksMsec() % 500 > 250 ? 1 : 0;
        basicSprite.Visible = input_action != Action.Action;

        if (direction != Vector2.Zero && input_action != Action.Action)
        {
            frame_X += 2;
            frame_Y = direction.X > 0.1 ? 3 : direction.X < -0.1 ? 2 : direction.Y > 0.1 ? 0 : 1;
        }

        if (actionSprite != null)
        {
            actionSprite.Visible = input_action == Action.Action;
            if (input_action == Action.Action)
            {
                frame_offset = activeTool switch
                {
                    Tool.Scythe => 0,
                    Tool.Axe => 4,
                    Tool.Watering_Can => 8,
                    _ => frame_offset,
                };
            }
            else
            {
                frame_offset = 0;
            }
        }
        (input_action == Action.Action ? actionSprite : basicSprite).FrameCoords = new Vector2I(frame_X, frame_Y + frame_offset);
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        Animate();
        MoveAndSlide();
    }
}
