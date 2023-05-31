using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Turtle : GameObject
{
    //turtle related
    private const int TURTLE_TILING_X = 9;
    private const int TURTLE_TILING_Y = 1;

    private Sprite sprite;
    private Texture turtleTexture;
    private float scale = 3;
    private float animationTime = 0;
    private float animationSpeed = 10;
    private int[] frameCountPerAnimation = new int[1];
    private AnimationType currentAnimation = AnimationType.Throw;
    bool IsFishing = false;
    bool initAnim = false;
    public enum AnimationType
{
    Throw = 0
}
    bool startmovement = false;
    private float DeltaTime;

    //line related
    private VertexArray line;
    bool lineVisible = false;
    Vector2f startPoint;
    Vector2f endPoint;
    Vector2f controlPoint;
    const int NumPoints = 100;
    private Sprite? LineEnd;

    private float LineEndScale = 3f;
    private float LineStartScale = 4f;
    float lineanimationTime;
    public override void Initialize()
    {
        //turtle related
        frameCountPerAnimation[0] = 9;

        turtleTexture = AssetManager.Instance.Textures["turtle"];
        sprite = new Sprite(turtleTexture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(turtleTexture.Size.X / TURTLE_TILING_X),
            (int)(turtleTexture.Size.Y / TURTLE_TILING_Y)
        );
        sprite.Scale *= scale;

        //line related
        Texture lineEndTexture = AssetManager.Instance.Textures["lineEnd"];

        LineEnd = new Sprite(lineEndTexture);

        Vector2f center = new Vector2f(LineEnd.Texture.Size.X * 0.5f, LineEnd.Texture.Size.Y * 0.5f);
        LineEnd.Origin = center;
        LineEnd.Scale *= LineEndScale;

        line = new VertexArray(PrimitiveType.LineStrip);
        lineVisible = false;

        startPoint = new Vector2f(turtleTexture.Size.X/TURTLE_TILING_X * scale, turtleTexture.Size.Y / TURTLE_TILING_Y * scale + 0.2f);
        endPoint = new Vector2f(700, 300);
        controlPoint = new Vector2f(400, 100);

        for (int i = 0; i <= NumPoints; i++)
        {
            float t = i / (float)NumPoints;
            Vector2f point = Utils.Lerp(Utils.Lerp(startPoint, controlPoint, t), Utils.Lerp(controlPoint, endPoint, t), t);

            line.Append(new Vertex(point, Color.Black));
        }
        lineanimationTime = 0;

        IsFishing = false;
        initAnim = false;
    }
    
    public void SetPositon(Vector2f pos){
        sprite.Position = pos;

        startPoint = new Vector2f(turtleTexture.Size.X/TURTLE_TILING_X/scale * 0.5f, turtleTexture.Size.Y / TURTLE_TILING_Y/scale*4f) + sprite.Position;
        endPoint = startPoint;
        controlPoint = startPoint;
    }

    public override void Update(float deltaTime, InputManager inputman){
        if(Throw(inputman) || initAnim){
            animationTime += deltaTime * animationSpeed;

            int animationFrame = (int)animationTime % frameCountPerAnimation[(int)currentAnimation];
            int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
            int spriteOffsetY = (int)currentAnimation * sprite.TextureRect.Height;

            sprite.TextureRect = new IntRect(
                spriteOffsetX,
                spriteOffsetY,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );
            if(animationFrame == frameCountPerAnimation[(int)currentAnimation]-1){
                initAnim = false;
            }

            //line animation this was pain
            float updownspeed = 55;
            if(animationFrame < 4 && animationFrame > 1){
                lineanimationTime += deltaTime * animationSpeed;
                startPoint += new Vector2f(90, -10/lineanimationTime/lineanimationTime).Normalize() * deltaTime * scale * 150;
            }
            if(animationFrame == 5){
                startPoint += new Vector2f(0.25f, 1).Normalize() * updownspeed * deltaTime * scale;
            }
            else if (animationFrame == 7){
                startPoint += new Vector2f(-0.25f, -1).Normalize() * updownspeed * deltaTime * scale;
            }
            controlPoint += new Vector2f(3, 1).Normalize() * 200* deltaTime;
        }


        //line update code starts here
        line.Clear();
        controlPoint = new Vector2f((endPoint.X-startPoint.X)/2, (endPoint.Y - startPoint.Y)/2);
        for (int i = 0; i <= NumPoints; i++)
        {
            float t = i / (float)NumPoints;
            Vector2f point = Utils.Lerp(Utils.Lerp(startPoint, controlPoint, t), Utils.Lerp(controlPoint, endPoint, t), t);
            
            line.Append(new Vertex(point, Color.Black));
        }
        LineEnd.Position = endPoint;
    }
    
    public bool Throw(InputManager inputman){
        if(!IsFishing && inputman.GetKeyPressed(Keyboard.Key.Space)){
            IsFishing = true;
            initAnim = true;
            return true;
        }
        return false;
    }

    public override void Draw(RenderWindow window, RenderStates states, bool shaderon){
        if(shaderon){
            window.Draw(LineEnd, states);
            window.Draw(line, states);
        }else{
            window.Draw(LineEnd);
            window.Draw(line);
        }
        window.Draw(sprite);
    }

    internal void SetEndPoint(Vector2f position)
    {
        endPoint = position + new Vector2f(0,-3);
    }

    public Vector2f GetLineStart(){
        return startPoint;
    }
}