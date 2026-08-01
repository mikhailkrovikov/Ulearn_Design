namespace Inheritance.Geometry.Visitor;

public class BoxifyVisitor : IVisitor
{
    private readonly BoundingBoxVisitor boundingBoxVisitor = new();
    public Body Visit(CompoundBody body)
    {
        return new CompoundBody(body.Parts
            .Select(b => b.Accept(this))
            .ToList());
    }

    public Body Visit(Cylinder body)
    {
        return body.Accept(boundingBoxVisitor);
    }

    public Body Visit(RectangularCuboid body)
    {
        return body;
    }

    public Body Visit(Ball body)
    {
        return body.Accept(boundingBoxVisitor);
    }
}