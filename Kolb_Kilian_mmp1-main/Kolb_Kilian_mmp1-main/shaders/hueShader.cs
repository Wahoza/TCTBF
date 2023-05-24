/* using SFML.Graphics;
using SFML.System;
using SFML.Window;
public class hueShader : Shaders
{
    Vector2f center = new Vector2f(0, 0);
    Vector2f size = new Vector2f(0, 0);
    VertexArray vertices = new VertexArray(PrimitiveType.Quads, 4);

    public override void Draw(RenderWindow window)
    {
        View view = window.GetView();
        size = view.Size;
        center = view.Center;
        float maxDist = size.Magnitude();

        vertices.Clear();
        vertices.Append(new Vertex(center + new Vector2f(-size.X / 2, -size.Y / 2), new Color(0, 0, 0, 0)));
        vertices.Append(new Vertex(center + new Vector2f(size.X / 2, -size.Y / 2), new Color(0, 0, 0, 0)));
        vertices.Append(new Vertex(center + new Vector2f(size.X / 2, size.Y / 2), new Color(0, 0, 0, 0)));
        vertices.Append(new Vertex(center + new Vector2f(-size.X / 2, size.Y / 2), new Color(0, 0, 0, 0)));

        for (uint i = 0; i < 4; i++)
        {
            Vector2f pos = vertices[i].Position;

            float dist = Utils.Distance(player, pos);
            byte hue = (byte)(255f * dist/maxDist);
            Color color = new Color (0, 0, 0, hue);
            vertices[i] = new Vertex(pos, color);
        }

        window.Draw(vertices);
    }

    public override void Initialize(Vector2f size)
    {

    }

    public override void Update(float deltaTime, Vector2f pos)
    {
        
    }
}
*/
