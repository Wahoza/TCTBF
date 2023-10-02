//Author: Kilian Kolb
//this is a long fish; it just swims twoards the player
using SFML.Graphics;
using SFML.System;

public class BiggestFish : Fish
{
    private Sprite printer = new();
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 5;
    private float hitboxscale = 0.7f;
    Vector2f textureSize;
    List<Vector2f> FishItems = new List<Vector2f>(100);
    float speed = 5;
    float FISH_TILING_X = 7;
    float animationTime = 0;
    float animationSpeed = 10;
    int length = 500;
    float RotationNum = 0;

    public override void Initialize()
    {

        Texture texture =  AssetManager.Instance.Textures["BiggestFish"];
        printer = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
            printer.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(printer.Texture.Size.X/FISH_TILING_X, printer.Texture.Size.Y);

        Random random = new Random();
        scale =  random.Next(70, 100)/10;
        speed = 40;
        printer.Scale *= scale;

        Vector2f center = textureSize * 0.5f;
        printer.Origin = center;

        for(int element = length; element >= 0; element--){
            FishItems.Add(new Vector2f (0, 0));
        }
        CollisionBox = new FloatRect(0, 0, textureSize.X * scale * hitboxscale, textureSize.Y * scale * hitboxscale);
    }

    public override void Update(float deltaTime)
    {
        for(int element = FishItems.Count()-1; element > 0; element--){
            int spacebetween = 4;
            FishItems[element] = FishItems[element - 1] + (FishItems[element] - FishItems[element-1]).Normalize() * spacebetween; 
        }
        FishItems[0] += movevec * deltaTime * speed;

        //Console.WriteLine(FishItems[0]);
        CollisionBox.Left = FishItems[0].X-textureSize.X*scale * hitboxscale/2;
        CollisionBox.Top = FishItems[0].Y-textureSize.Y*scale * hitboxscale/2;
        animationTime += deltaTime * animationSpeed;
    }
    public override void Draw(RenderWindow window)
    {
            int animationFrame = 4+(int)animationTime % 3;
            int spriteOffsetX = animationFrame * printer.TextureRect.Width -1;
            printer.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                printer.TextureRect.Width,
                printer.TextureRect.Height
        );
        for(int element = FishItems.Count()-1; element > 0; element--){
            printer.Position = FishItems[element];
            if(element == FishItems.Count()-1){
                printer.Position -=(FishItems[element-1]-FishItems[element]).Normalize() * scale * textureSize.X/FISH_TILING_X * 4;
            }
            printer.Rotation = Utils.Angle(FishItems[element-1]-FishItems[element]) *  360 /2 /(float)Math.PI;
            window.Draw(printer);

            spriteOffsetX = (1 + element%3)  * printer.TextureRect.Width;
            printer.TextureRect=
                new IntRect(
                    spriteOffsetX,
                    0,
                    printer.TextureRect.Width,
                    printer.TextureRect.Height
            );
        }
        printer.TextureRect=
            new IntRect(
                0,
                0,
                printer.TextureRect.Width,
                printer.TextureRect.Height
            );
        printer.Position = FishItems[0];
        if(RotationNum!= 0){
            printer.Rotation = RotationNum;
        }
        
        window.Draw(printer);
    }

    public override void MoveToGoal(Vector2f player)
    {
        movevec = (player - FishItems[0]).Normalize();
    }
    new public Vector2f GetPosition()
    {
        return FishItems[0];
    }
    public override bool CollisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player)){
            return true;
        }
        return false;
    }
    public new void SetPosition(Vector2f position){
        FishItems[0] = position;
        if(FishItems[1] == FishItems[2]){
            for(int i = 1; i<FishItems.Count; i++){
                FishItems[i] = FishItems[i-1] + new Vector2f(0, 2);
            }
        }
    }
}