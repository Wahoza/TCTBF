using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class ShopBar : GameObject{
    Sprite sprite = new();
    int upgradeTiling = 6;
    ValueParser.linePositionGame ParseLocation;
    float scale = 3;
    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["UpgradeBar"];
        sprite = new Sprite(texture);
            sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / upgradeTiling),
            (int)(texture.Size.Y)
        );
        Vector2f textureSize = new Vector2f(sprite.Texture.Size.X/upgradeTiling, sprite.Texture.Size.Y);
        sprite.Origin = textureSize / 2;
        sprite.Scale *= scale;


    }

    public override void Update(float deltaTime)
    {
        int spriteOffsetX = (ValueParser.ParseGameState(ParseLocation) -1) * sprite.TextureRect.Width;
        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            0,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );
        sprite.Origin = new Vector2f(sprite.Texture.Size.X/6, sprite.Texture.Size.Y)/2;
            
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }
    public void SetPosition(Vector2f pos){
        sprite.Position = pos;
    }
    public void SetParseLocation(ValueParser.linePositionGame _ParseLocation){
        ParseLocation = _ParseLocation;
    }
}