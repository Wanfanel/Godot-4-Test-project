using Godot;
using System;

public sealed partial class DemoSprite2D : Node2D
{

	private int Speed = 400;
	private float AngularSpeed = Mathf.Pi;

	public DemoSprite2D()
	{
		GD.Print("Msec"+ Time.GetTicksMsec()+", Usec:" + Time.GetTicksUsec());
	}
	public override void _Process(double delta)
	{
		Rotation += AngularSpeed * (float)delta;
GD.Print("Msec"+ Time.GetTicksMsec()%1000);

	}

}
