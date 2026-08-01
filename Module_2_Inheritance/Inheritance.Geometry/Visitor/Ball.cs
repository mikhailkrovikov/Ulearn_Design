namespace Inheritance.Geometry.Visitor;

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
