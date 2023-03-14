using Godot;
using System;

namespace Moreus;
public sealed partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 100.0f;
    [Export] public Sprite2D basicSprite;
    [Export] public Sprite2D actionSprite;
    [Export] public Tool activeTool = Tool.Scythe;

    private Sprite2D activeSprite;
    private Vector2 direction;
    public Action input_action;
    public Item item_hand = null;
    [Export] public HUD hud;

    private int frame_X, frame_Y, frame_offset;

    public Vector2 Face
    {
        get
        {
            switch (frame_Y)
            {
                case 0:
                    return Vector2.Down;
                case 1:
                    return Vector2.Up;
                case 2:
                    return Vector2.Left;
                case 3:
                    return Vector2.Right;
            }
            return Vector2.Zero;
        }
    }

    public void GetInput()
    {
        Vector2 velocity = Velocity;

        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (Input.IsAnythingPressed())
        {
            if (Input.IsActionPressed("ui_select"))
                input_action = Action.Action;

            else if (Input.IsActionPressed("item"))
                input_action = Action.Item;

            else if (Input.IsActionPressed("pickup"))
                input_action = Action.Pickup;
            else
                input_action = Action.None;


        }
        else
            input_action = Action.None;

        if (direction != Vector2.Zero && input_action == Action.None)
            velocity = direction * Speed;
        else
        {

            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }
        Velocity = velocity;
    }

    public void Animate()
    {
        if (!(basicSprite != null && actionSprite != null))
            return;

        frame_X = (int)Time.GetTicksMsec() % 500 > 250 ? 1 : 0;

        basicSprite.Visible = (input_action != Action.Action);

        if (direction != Vector2.Zero && input_action != Action.Action)
        {
            frame_X += 2;
            if (direction.X > 0)
                frame_Y = 3;
            if (direction.X < 0)
                frame_Y = 2;
            if (direction.Y > 0)
                frame_Y = 0;
            if (direction.Y < 0)
                frame_Y = 1;
        }
        if (actionSprite != null)
        {
            actionSprite.Visible = (input_action == Action.Action);
            if (input_action == Action.Action)
            {
                activeSprite = actionSprite;
                switch (activeTool)
                {
                    case Tool.Scythe:
                        frame_offset = 0;
                        break;
                    case Tool.Axe:
                        frame_offset = 4;
                        break;
                    case Tool.Watering_Can:
                        frame_offset = 8;
                        break;
                }
            }
            else
            {
                activeSprite = basicSprite;
                frame_offset = 0;
            }
        }
        activeSprite.FrameCoords = new Vector2I(frame_X, frame_Y + frame_offset);

    }
    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        Animate();
        MoveAndSlide();
    }
}
