namespace Inheritance.Geometry.Virtual;

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}

    public override bool ContainsPoint(Vector3 point)
    {
        return Parts.Any(body => body.ContainsPoint(point));
    }

    public override RectangularCuboid GetBoundingBox()
    {
        var boundings = Parts.Select(b => b.GetBoundingBox());

        var minX = boundings.Min(b => b.Position.X - b.SizeX / 2);
        var minY = boundings.Min(b => b.Position.Y - b.SizeY / 2);
        var minZ = boundings.Min(b => b.Position.Z - b.SizeZ / 2);
        var maxX = boundings.Max(b => b.Position.X + b.SizeX / 2);
        var maxY = boundings.Max(b => b.Position.Y + b.SizeY / 2);
        var maxZ = boundings.Max(b => b.Position.Z + b.SizeZ / 2);

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
}