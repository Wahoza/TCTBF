using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class ShopHud : GameObject
{
    List<ShopItem> ShopSlots = new List<ShopItem>();
    FloatRect MousePoint;
    MoneyHud moneyHud = new();
    public override void Initialize()
    {
        ShopItem VisionUpgrade = new ShopItem();
        VisionUpgrade.Initialize("UpgradeVision", 5, ValueParser.linePositionGame.Vision);
        ShopSlots.Add(VisionUpgrade);
        
        ShopItem BaitUpgrade = new ShopItem();
        BaitUpgrade.Initialize("UpgradeBait", 10, ValueParser.linePositionGame.Bait);
        ShopSlots.Add(BaitUpgrade);

        ShopItem TimeUpgrade = new ShopItem();
        TimeUpgrade.Initialize("UpgradeMove", 7, ValueParser.linePositionGame.MoveTime);
        ShopSlots.Add(TimeUpgrade);

        //makes game lag
        ShopItem WeightUpgrade = new ShopItem();
        WeightUpgrade.Initialize("UpgradeWeight", 6, ValueParser.linePositionGame.Weight);
        ShopSlots.Add(WeightUpgrade);

        moneyHud.Initialize();

        float PositionX = -850;
        float PositionY = -200;
        moneyHud.SetPosition(new Vector2f(PositionX, PositionY -150));

        foreach(ShopItem item in ShopSlots){
            item.SetPositon(new SFML.System.Vector2f(PositionX, PositionY));
            PositionX += 185;
        }
    }

    public override void Update(float deltaTime)
    {
        foreach(ShopItem item in ShopSlots){
            item.Update(deltaTime, MousePoint);
        }
        moneyHud.Update(deltaTime);
    }
    public override void Draw(RenderWindow window)
    {
        foreach(ShopItem item in ShopSlots){
            item.Draw(window);
        }
        moneyHud.Draw(window);
    }
    public void UpdatePointer (FloatRect pointer){
        MousePoint = pointer;
    }
}