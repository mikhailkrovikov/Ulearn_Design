namespace Inheritance.Geometry.Visitor;

public class BoundingBoxVisitor : IVisitor
{
    public Body Visit(CompoundBody body)
    {
        var boundings = body.Parts.Select(b => b.Accept(this));

        var minX = boundings.Min(b => b.Position.X - ((RectangularCuboid)b).SizeX / 2);
        var minY = boundings.Min(b => b.Position.Y - ((RectangularCuboid)b).SizeY / 2);
        var minZ = boundings.Min(b => b.Position.Z - ((RectangularCuboid)b).SizeZ / 2);
        var maxX = boundings.Max(b => b.Position.X + ((RectangularCuboid)b).SizeX / 2);
        var maxY = boundings.Max(b => b.Position.Y + ((RectangularCuboid)b).SizeY / 2);
        var maxZ = boundings.Max(b => b.Position.Z + ((RectangularCuboid)b).SizeZ / 2);

        var position = new Vector3(
            (minX + maxX) / 2,
            (minY + maxY) / 2,
            (minZ + maxZ) / 2);

        return new RectangularCuboid(
            position,
            maxX - minX,
            maxY - minY,
            maxZ - minZ);
    }

    public Body Visit(Cylinder body)
    {
        var diametr = body.Radius * 2;
        return new RectangularCuboid(body.Position, diametr, diametr, body.SizeZ);
    }

    public Body Visit(RectangularCuboid body)
    {
        return body;
    }

    public Body Visit(Ball body)
    {
        var diametr = body.Radius * 2;
        return new RectangularCuboid(body.Position, diametr, diametr, diametr);
    }
}
