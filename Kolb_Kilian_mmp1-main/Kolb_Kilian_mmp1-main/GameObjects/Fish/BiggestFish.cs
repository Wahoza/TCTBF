//this is a long fish; it just swims twoards the player
using SFML.Graphics;
using SFML.System;

public class BiggestFish : Fish
{
    private Sprite printer;
    private bool locked = false;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 5;
    private float hitboxscale = 0.7f;
    Vector2f textureSize;
    List<Vector2f> FishItems = new List<Vector2f>(100);
    float speed = 4;
    float FISH_TILING_X = 7;
    float FISH_TILING_Y = 1;
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
            (int)(texture.Size.Y / FISH_TILING_Y)
        );
        textureSize = new Vector2f(printer.Texture.Size.X/FISH_TILING_X, printer.Texture.Size.Y/FISH_TILING_Y);

        Random random = new Random();
        scale =  random.Next(40, 100)/10;
        speed = 300/scale;
        printer.Scale *= scale;

        Vector2f center = textureSize * 0.5f;
        printer.Origin = center;

        for(int element = length*(int)scale/4; element >= 0; element--){
            FishItems.Add(new Vector2f (-1000, 0)); 
        }
        CollisionBox = new FloatRect(0, 0, textureSize.X * scale * hitboxscale, textureSize.Y * scale * hitboxscale);
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        for(int element = FishItems.Count()-1; element > 0; element--){
            FishItems[element] = FishItems[element - 1]; 
        }
        FishItems[0] += movevec * deltaTime * speed;

        animationTime += deltaTime * animationSpeed;
            int animationFrame = 4+ (int)animationTime % 3;
            int spriteOffsetX = animationFrame * printer.TextureRect.Width;
            printer.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                printer.TextureRect.Width,
                printer.TextureRect.Height
        );
        if(FishItems[0].Y< 300){
            SetPositon(new Vector2f(FishItems[0].X, 300));
        }

        CollisionBox.Left = FishItems[0].X-textureSize.X*scale * hitboxscale/2;
        CollisionBox.Top = FishItems[0].Y-textureSize.X*scale * hitboxscale/2;
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        if(shaderon == true){
            for(int element = FishItems.Count()-1; element > 0; element--){
                printer.Position = FishItems[element];
                printer.Rotation = Utils.Angle(FishItems[element-1]-FishItems[element]) *  360 /2 /(float)Math.PI;
                window.Draw(printer, states);

                int spriteOffsetX = (1 + element%3)  * printer.TextureRect.Width;
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
            window.Draw(printer, states);


        }else{
            for(int element = FishItems.Count()-1; element > 0; element--){
                printer.Position = FishItems[element];
                if(element == FishItems.Count()-1){
                    printer.Position -=(FishItems[element-1]-FishItems[element]).Normalize() * scale * textureSize.X/FISH_TILING_X * 4;
                }
                printer.Rotation = Utils.Angle(FishItems[element-1]-FishItems[element]) *  360 /2 /(float)Math.PI;
                window.Draw(printer);

                int spriteOffsetX = (1 + element%3)  * printer.TextureRect.Width;
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

    }

    public override void MoveToGoal(Vector2f player)
    {
        movevec = (player - FishItems[0]).Normalize();
    }

    public override void SetPositon(Vector2f position)
    {
        FishItems[0] = position;
    }
    public override Vector2f GetPosition()
    {
        return FishItems[0];
    }
    public override int ColisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player)){
            return 1000000;
        }
        return 0;
    }

    public override void SetRotation(Vector2f Rotationvec)
    {
        RotationNum = (float)(Utils.Angle(Rotationvec) * 360 / (2*Math.PI));
    }
    public override float GetScale()
    {
        return scale;
    }
}