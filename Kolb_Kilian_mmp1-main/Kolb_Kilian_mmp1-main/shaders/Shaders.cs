using SFML.Graphics;
using SFML.System;
using SFML.Window;
public abstract class Shaders
{
    public abstract void Initialize(Vector2f size);
    public abstract RenderStates GetRenderStates();
    public abstract void Update(float deltaTime, Vector2f pos);
}