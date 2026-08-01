namespace Inheritance.Geometry.Virtual;

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

    public override bool ContainsPoint(Vector3 point)
    {
        var vector = point - Position;
        var length2 = vector.GetLength2();
        return length2 <= Radius * Radius;
    }

    public override RectangularCuboid GetBoundingBox()
    { 
        var diametr = Radius * 2;
        return new RectangularCuboid(Position, diametr, diametr, diametr);
    }
}
