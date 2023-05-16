using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class CircularWaveEffect
{
    private Shader shader;
    private VertexArray vertices;

    public CircularWaveEffect(Vector2u windowSize)
    {
        vertices = new VertexArray(PrimitiveType.Quads, 4);
        vertices[0] = new Vertex(new Vector2f(0, 0));
        vertices[1] = new Vertex(new Vector2f(windowSize.X, 0));
        vertices[2] = new Vertex(new Vector2f(windowSize.X, windowSize.Y));
        vertices[3] = new Vertex(new Vector2f(0, windowSize.Y));

        shader = new Shader(null, null, "wave.frag");
        shader.SetUniform("center", new Vector2f(windowSize.X / 2f, windowSize.Y / 2f));
        shader.SetUniform("radius", (float)(windowSize.X + windowSize.Y) / 4f);
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Shader = shader;
        target.Draw(vertices, states);
    }

    public void Update(float deltaTime)
    {
        shader.SetUniform("time", deltaTime);
    }
    
}