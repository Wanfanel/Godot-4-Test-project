using Godot;
using System;

namespace Moreus;
public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 100.0f;
    [Export] public Sprite2D basicSprite;
    [Export] public Sprite2D actionSprite;
    [Export] public Tool activeTool = Tool.Hoe;
    private Sprite2D activeSprite;
    private Vector2 direction;
    private Boolean input_action;
    private int frame_X, frame_Y, frame_offset;

    public void GetInput()
    {

        Vector2 velocity = Velocity;

        input_action = Input.IsActionPressed("ui_select");
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero && !input_action)
        {
            velocity = direction * Speed;

        }
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

        basicSprite.Visible = !input_action;

        if (direction != Vector2.Zero && !input_action)
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
            actionSprite.Visible = input_action;
            if (input_action)
            {
                activeSprite = actionSprite;
                switch (activeTool)
                {
                    case Tool.Hoe:
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
