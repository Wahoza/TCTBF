using SFML.Graphics;
using SFML.System;
using SFML.Window;
public static class DrawUtils{
    public static void DrawRect(Color color, FloatRect rect, RenderWindow window){
        RectangleShape rectangleShape = new RectangleShape(new Vector2f(rect.Width, rect.Height));
        rectangleShape.FillColor = color;
        rectangleShape.Position = new Vector2f(rect.Left, rect.Top);
        window.Draw(rectangleShape);
    }
}
