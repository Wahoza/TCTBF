using SFML.System;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
public class waveShader : Shaders
{
    Shader shader;
    public override RenderStates GetRenderStates()
    {
        RenderStates renderStates = new RenderStates();
        renderStates.Shader = shader;
    
        return renderStates;
    }

    public override void Initialize(Vector2f size)
    {
        shader = new Shader("shaders/wave.glsl", null, "shaders/frag.frag");
        shader.SetUniform("wave_phase", 1);
        shader.SetUniform("wave_amplitude", new Vector2f(1, 1));
        RenderTexture renderTexture = new RenderTexture((uint)size.X, (uint)size.Y);

    }

    public override void Update(float deltaTime, Vector2f pos)
    {
        shader.SetUniform("wave_phase", 1);
        shader.SetUniform("wave_amplitude", new Vector2f(1, 1));
    }
}