//Author: Kilian Kolb
using SFML.Graphics;
using SFML.System;


//Generator for Many Subfishes, creating a cluster
public class FishGroupCreator
{
    public List<SubFish> Group = new();
    Vector2f position;
    float radius;
    int MaxfishAmount = 7;
    int MinfishAmount = 1;
    Random random = new();

    public void Initialize()
    {
        radius = random.Next(100, 500);
        Group = new();
        for(int i = 0; i <= random.Next(MinfishAmount, MaxfishAmount); i++){
            SubFish fish = new();
            fish.Initialize();
            fish.SetRadius(radius);
            Group.Add(fish);
        }
    }
    public bool CollisionCheck(FloatRect player)
    {
        foreach(SubFish fish in Group){
            if(fish.CollisionCheck(player)){
                return fish.CollisionCheck(player);
            }

        }
        return false;
    }

    public void SetPositon(Vector2f _position)
    {
        position = _position + new Vector2f(0, radius);
        foreach(SubFish fish in Group){
            float y = random.Next(0, 2*(int)radius) - radius;
            float x = random.Next(0, 2*(int)radius) - radius;
            fish.SetPositon(position - new Vector2f(x, y));
            fish.SetOrigin(position);
        }
    }
    public List<SubFish> GetFishList(){
        return Group;
    }
}

