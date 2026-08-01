namespace Inheritance.Geometry.Virtual;

public abstract class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}

	public abstract bool ContainsPoint(Vector3 point);

	public abstract RectangularCuboid GetBoundingBox();
}
