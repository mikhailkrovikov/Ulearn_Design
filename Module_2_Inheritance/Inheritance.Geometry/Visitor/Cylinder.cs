namespace Inheritance.Geometry.Visitor;

public class Cylinder : Body
{
	public double SizeZ { get; }

	public double Radius { get; }

	public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
	{
		SizeZ = sizeZ;
		Radius = radius;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
