using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

public class FishMouse : GameObject
{
    Vector2f area;
    Sprite sprite = new();
    int MOUSE_TILING_X = 2;
    float scale = 2;
    FloatRect CollisionBox;
    float hitboxscale = 0.1f;
    Vector2f textureSize;
    public override void Initialize()
    {
        Texture texture = AssetManager.Instance.Textures["Mouse"];
        sprite = new Sprite(texture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / MOUSE_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/MOUSE_TILING_X, sprite.Texture.Size.Y);
        sprite.Scale *= scale;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);

    }

    public override void Update(float deltaTime)
    {
        sprite.Position = (Vector2f)Mouse.GetPosition()-area/2;
        
        int spriteOffsetX = 0;
        if(InputManager.Instance.GetMousePressed(Mouse.Button.Left)){
            spriteOffsetX = sprite.TextureRect.Width;
        }
        
        sprite.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );

        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, CollisionBox.Width, CollisionBox.Height);
        
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
        //DrawUtils.DrawRect(Color.Red, CollisionBox, window);
    }
    public void SetFishMouseArea(Vector2f _area){
        area = _area;
    }
    public FloatRect GetMouseRect(){
        CollisionBox = new FloatRect(Mouse.GetPosition().X-area.X/2, Mouse.GetPosition().Y-area.Y/2, CollisionBox.Width, CollisionBox.Height);
        return CollisionBox;
    }
}