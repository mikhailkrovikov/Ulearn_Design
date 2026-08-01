namespace Inheritance.Geometry.Visitor;

public interface IVisitor
{
	Body Visit(CompoundBody body);

    Body Visit(Cylinder body);

    Body Visit(RectangularCuboid body);

    Body Visit(Ball body);
}
