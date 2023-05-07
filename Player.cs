using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Player : GameObject
{
    private const int PLAYER_TILING_X = 3;
    private const int PLAYER_TILING_Y = 3;

    private Sprite sprite;

    private float animationTime = 0;
    private float animationSpeed = 10;

    private Vector2f move = new Vector2f(0, 0);

    private int[] frameCountPerAnimation = new int[3];
    private AnimationType currentAnimation = AnimationType.Down;
    public enum AnimationType
{
    Down = 0,
    Left = 1,
    Right = 2
}
    
    private Vector2f MoveVec;
    private float MoveScale = 50;
    private float MoveCooldown = 0;
    bool startmovement = false;
    private float DeltaTime;
    public override void Initialize(AssetManager assetManager)
    {
        frameCountPerAnimation[0] = 1;
        frameCountPerAnimation[1] = 3;
        frameCountPerAnimation[2] = 3;

        Texture playerTexture = assetManager.Textures["hookwbait"];
        sprite = new Sprite(playerTexture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(playerTexture.Size.X / PLAYER_TILING_X),
            (int)(playerTexture.Size.Y / PLAYER_TILING_Y)
        );
        sprite.Scale *= (float) 3;
        sprite.Position = new Vector2f(0, 0);
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        animationTime += deltaTime * animationSpeed;
        
        if(MoveStarted(inputman)){
            Move(deltaTime, inputman);
        }

        int animationFrame = (int)animationTime % frameCountPerAnimation[(int)currentAnimation];

        int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
        int spriteOffsetY = (int)currentAnimation * sprite.TextureRect.Height;

        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            spriteOffsetY,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );

    }
    private bool MoveStarted(InputManager inputman){
        if(!(startmovement) && inputman.GetKeyDown(Keyboard.Key.Space)){
            startmovement = true;
            move = new Vector2f(5, -4) * MoveScale;
        }
        return startmovement;
    }

    //makes "floaty" movement, still needs more "jank" feel
    private void Move(float dt, InputManager inputman){
        
        if(MoveCooldown <= 0){
            if(inputman.GetKeyDown(Keyboard.Key.A)){
                move = new Vector2f(-4, -2) * MoveScale;
                MoveCooldown = (float)0.5;
                currentAnimation = AnimationType.Left;            
                }
            if(inputman.GetKeyDown(Keyboard.Key.D)){
                move = new Vector2f(4, -2) * MoveScale;
                MoveCooldown = (float)0.5;
                currentAnimation = AnimationType.Right; 
            }
            if(inputman.GetKeyDown(Keyboard.Key.S)){
                move = new Vector2f(0, -5) * MoveScale;
                MoveCooldown = (float)0.5;
                currentAnimation = AnimationType.Down; 
            }
        }else{
            MoveCooldown -= dt;
        }

        if(move.X == 0){
            //do nothing, but skip other checks
            currentAnimation = AnimationType.Down;

        }else if((int)move.X < 0){
            move += new Vector2f(1, 0) * dt * MoveScale * 2;
        }else if ((int)move.X>0){
            move += new Vector2f(-1, 0) * dt * MoveScale * 4;
        }else{
            move.X = 0;
        }

        if((int)move.Y <= 0){
            move += new Vector2f(0, 1) * dt * MoveScale * 5;
        }else{
            move += new Vector2f(0, 1) * dt * MoveScale;
        }
        sprite.Position += move * dt;
    }
    private Vector2f GetCoords(){
        FloatRect bounds = sprite.GetLocalBounds();
        float centerX = bounds.Left + bounds.Width / 5;
        float centerY = bounds.Top + bounds.Height / 2;
        return new Vector2f(centerX+sprite.Position.X, centerY+sprite.Position.Y);
    }

    public override void Draw(RenderWindow window)
    {
        View view = window.GetView();
        view.Center = new Vector2f(0, sprite.Position.Y);
        window.SetView(view);

        window.Draw(sprite);
    }
}