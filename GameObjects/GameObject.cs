using SFML.Graphics;

public abstract class GameObject : Transformable
{
    public abstract void Initialize(AssetManager assetman);

    public abstract void Update(float deltaTime, InputManager inputman);

    public abstract void Draw(RenderWindow window);
}