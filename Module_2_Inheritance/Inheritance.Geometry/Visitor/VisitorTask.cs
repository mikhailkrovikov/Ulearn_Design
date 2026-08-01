namespace Inheritance.Geometry.Visitor;

public abstract class Body
{
	public Vector3 Position { get; }

	public abstract Body Accept(IVisitor visitor);

    protected Body(Vector3 position)
	{
		Position = position;
	}
}
