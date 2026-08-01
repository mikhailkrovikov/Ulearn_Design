namespace Inheritance.Geometry.Visitor;

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
