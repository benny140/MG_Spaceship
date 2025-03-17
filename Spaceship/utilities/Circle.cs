using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public struct Circle
{
    public Vector2 Center { get; }
    public float Radius { get; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public static bool Intersects(Circle circle, Rectangle rectangle)
    {
        // Find the closest point on the rectangle to the circle's center
        float closestX = MathHelper.Clamp(circle.Center.X, rectangle.Left, rectangle.Right);
        float closestY = MathHelper.Clamp(circle.Center.Y, rectangle.Top, rectangle.Bottom);

        // Calculate the distance between the circle's center and the closest point
        float distanceX = circle.Center.X - closestX;
        float distanceY = circle.Center.Y - closestY;
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

        // Check if the distance is less than or equal to the circle's radius
        return distanceSquared <= (circle.Radius * circle.Radius);
    }
}
