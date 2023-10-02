//Author: Kilian Kolb
using SFML.Graphics;
using SFML.System;

public class FishManager : GameObject
{
    List<Fish> FishList = new List<Fish>();
    List<Fish> DroppedFish = new List<Fish>();
    List<float> DroppedFishA = new List<float>();
    Random random = new Random();
    bool FishBool;
    float depth = 0;
    float maxBigCount = 3;
    float SpawnRateParse;
    bool FishCought = false;
    Vector2f area;
    float weight;
    float windowoffset = 100;
    float biggestFishDepth = 70;
    public override void Initialize()
    {
        FishList = new List<Fish>();
        maxBigCount = 3;
        SpawnRateParse = ValueParser.ParseGameState(ValueParser.linePositionGame.Bait);
        weight = ValueParser.ParseGameState(ValueParser.linePositionGame.Weight);
    }

    public override void Update(float deltaTime)
    {
        if(!FishCought){
            int n = random.Next(1, 10000);
            if(n >10000-(depth*4.5)-300/(SpawnRateParse*SpawnRateParse*3/weight)){
                FishBool = true;
            }else{
                FishBool = false;
            }
            foreach(Fish fish in FishList){
                //Console.Write("a");
                fish.Update(deltaTime);
            }
            for(int i = 0; i < DroppedFish.Count; i++){
                int Direction = random.Next(0, 1);
                if(Direction == 0) Direction--;

                DroppedFishA[i] -= 100*deltaTime;
                DroppedFish[i].SetPosition(DroppedFish[i].GetPosition() - new Vector2f(-70* deltaTime * Direction, 0));
                DroppedFish[i].SetRotation(new Vector2f(1 * Direction,0));
                DroppedFish[i].SetTransparity((byte)DroppedFishA[i]);
                if(DroppedFishA[i] <= 1){
                    DroppedFishA.RemoveAt(i);
                    DroppedFish.RemoveAt(i);
                }
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
                spawnX = center.X - (area.X + windowoffset)/2 + random.Next(1,3) % 2 * (area.X + windowoffset);
                spawnY = center.Y + random.Next(-(int)area.Y/2, (int)area.Y/2) - area.Y * weight/5;
            }else{
                spawnY = center.Y - (area.Y)/2 + (area.Y + windowoffset);
                spawnX = center.X + random.Next(-(int)area.X/2, (int)area.X/2);
            }
            if(spawnY < 30){
                spawnY = 30;
            }
            Vector2f pos = new Vector2f (spawnX, spawnY);
            Fish fish = null!;
            int basevalue = 0;
        switch (FishNum){
            case 1:
                fish = new BulletFish();
                basevalue = 3;
                break;
            case 2:
                fish = new NormalFish();
                basevalue = 10;
                break;
            case 3:
                fish = new SquidFish();
                basevalue = 4;
                break;
            case 4:
                fish = new DriftFish();
                basevalue = 4;
                break;
            case 5:
            if(maxBigCount >0 && depth > biggestFishDepth){
                maxBigCount--;
                fish = new BiggestFish();
            }
            break;

            case 6:
                //special, as this creates multiple new fish, rather than one
                spawnY = (center.Y - (area.Y/2) + area.Y);
                spawnX = center.X + random.Next(-(int)area.X/2, (int)area.X/2);
                pos = new Vector2f(spawnX, spawnY);
                FishGroupCreator creator = new();
                    creator.Initialize();
                    creator.SetPosition(new Vector2f(spawnX,spawnY));
                    FishList.AddRange(creator.GetFishList());
                break;
            default:
                break;
        }
            if(fish != null){
                fish.Initialize();
                fish.SetPosition(pos);
                fish.SetValue(basevalue, pos.Y/13);
                if(fish is BiggestFish f){ f.SetPosition(pos);}
                FishList.Add(fish);
            }
        }
    }
    public override void Draw(RenderWindow window)
    {
        foreach(Fish fish in FishList){
            fish.Draw(window);
        }
        foreach(Fish fish in DroppedFish){
            fish.Draw(window);
        }
    }


    public bool CollisionCheck(FloatRect player)
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

                Fish save = fish;
                //FishCought = true;
                return save;
                
            }
        }
        return null!;
    }

    public void MoveToGoal(Vector2f player)
    {
        foreach(Fish fish in FishList){
            fish.MoveToGoal(player);
        }
        depth = player.Y /100;
    }

    public List<Fish> GetFish(){
        return FishList;
    }
    public void RemoveFish(Fish fish){
        if(FishList.Contains(fish)) FishList.Remove(fish); DroppedFish.Add(fish); DroppedFishA.Add(255);
        if(fish is BiggestFish){
            maxBigCount++;
        }
    }
}