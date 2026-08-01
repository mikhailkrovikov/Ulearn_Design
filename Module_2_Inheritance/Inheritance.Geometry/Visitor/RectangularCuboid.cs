namespace Inheritance.Geometry.Visitor;

public class RectangularCuboid : Body
{
	public double SizeX { get; }
	public double SizeY { get; }
	public double SizeZ { get; }

	public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
	{
		SizeX = sizeX;
		SizeY = sizeY;
		SizeZ = sizeZ;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
