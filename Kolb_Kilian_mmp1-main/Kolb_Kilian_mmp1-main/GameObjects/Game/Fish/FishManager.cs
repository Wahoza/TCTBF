using SFML.Graphics;
using SFML.System;

public class FishManager : Fish
{
    List<Fish> FishList;
    float timeSinceSpawn = 0;
    Random random = new Random();
    bool FishBool;
    float depth = 0;
    float maxBigCount = 3;
    float SpawnRateParse;
    bool FishCought = false;
    Vector2f area;
    public override void Initialize()
    {
        FishList = new List<Fish>();
        maxBigCount = 3;
        SpawnRateParse = ValueParser.ParseGameState(4);
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(!FishCought){
            int n = random.Next(1, 10000);
            if(n >10000-depth*5-300/(SpawnRateParse*SpawnRateParse*4)){
                FishBool = true;
            }else{
                FishBool = false;
            }
            foreach(Fish fish in FishList){
                fish.Update(deltaTime, inputman);
            }
        }
    }

    public void SpawnFish(bool started, Vector2f _area, Vector2f center){
        area = _area;
        if(started && FishBool){
            int FishNum = random.Next(1, 7);
            float spawnX = 0;
            float spawnY = 0;
            if(random.Next(1,3)%2 == 0){
                spawnX = center.X - (area.X +100)/2 + random.Next(1,3) % 2 * (area.X +100);
                spawnY = center.Y + random.Next(-(int)area.Y/2, (int)area.Y/2); 
            }else{
                spawnY = center.Y - (area.Y)/2 + (area.Y +100);
                spawnX = center.X + random.Next(-(int)area.X/2, (int)area.X/2);
            }
            if(spawnY < 30){
                spawnY = 30;
            }
            Vector2f pos = new Vector2f (spawnX, spawnY);
            
        switch (FishNum){
            case 1:
                BulletFish BulletFish = new BulletFish();
                BulletFish.Initialize();
                BulletFish.SetPositon(pos);
                FishList.Add(BulletFish);
                break;
            case 2:
                NormalFish NormalFish = new NormalFish();
                NormalFish.Initialize();
                NormalFish.SetPositon(pos);
                FishList.Add(NormalFish);
                break;
            case 3:
                SquidFish SquidFish = new SquidFish();
                SquidFish.Initialize();
                SquidFish.SetPositon(pos);
                FishList.Add(SquidFish);
                break;
            case 4:
                DriftFish DriftFish = new DriftFish();
                DriftFish.Initialize();
                DriftFish.SetPositon(pos);
                FishList.Add(DriftFish);
                break;
            case 5:
            if(maxBigCount >0 && depth > 100){
                maxBigCount--;
                BiggestFish BiggestFish = new BiggestFish();
                BiggestFish.Initialize();
                BiggestFish.SetPositon(pos);
                FishList.Add(BiggestFish);
            }
            break;

            case 6:
                spawnY = center.Y - (area.Y)/2 + (area.Y +500);
                spawnX = center.X + random.Next(-(int)area.X/2, (int)area.X/2);
                FishGroup FishGroup = new FishGroup();
                FishGroup.Initialize();
                FishGroup.SetPositon(new Vector2f(spawnX, spawnY));
                FishList.Add(FishGroup);
                break;
            default:
                break;
        }
        }
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        foreach(Fish fish in FishList){
            fish.Draw(window, states, shaderon);
        }
    }


    public override bool CollisionCheck(FloatRect player)
    {
        foreach(Fish fish in FishList){
            if(fish.CollisionCheck(player)){
                return fish.CollisionCheck(player);
            }
        }
        return false;
    }
    public Fish CollisionTest(FloatRect player){
        foreach(Fish fish in FishList){
            if(fish.CollisionCheck(player)){
                if(fish is FishGroup group){

                    FishCought = true;
                    return group.CollisionTest(player);
                    
                }else{
                    
                    Fish save = fish;
                    FishCought = true;
                    return save;
                }
            }
        }
        return null;  
    }

    public override void MoveToGoal(Vector2f player)
    {
        foreach(Fish fish in FishList){
            fish.MoveToGoal(player);
        }
        depth = player.Y /100;
    }
    public override void SetPositon(Vector2f position)
    {

    }
    //this function exists, but FishManager doesnt have a position;
    public override Vector2f GetPosition()
    {
        return new Vector2f(0, 0);
    }
    public List<Fish> GetFish(){
        return FishList;
    }
        public override void SetRotation(Vector2f Rotationvec)
    {
        foreach (Fish fish in FishList){
            fish.SetRotation(Rotationvec);
        }
    }
    public override float GetScale()
    {
        return Utils.Distance(new Vector2f(area.X, 0), new Vector2f(0, area.Y));
    }
    public override float GetValue()
    {
        float value = 0;
        foreach(Fish fish in FishList){
            value += fish.GetValue();
        }
        return value;
    }
}