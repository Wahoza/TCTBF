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
        ShopItem TimeUpgrade = new ShopItem();
        TimeUpgrade.Initialize("UpgradeMove", 7, 2);
        ShopSlots.Add(TimeUpgrade);

        ShopItem BaitUpgrade = new ShopItem();
        BaitUpgrade.Initialize("UpgradeBait", 10, 4);
        ShopSlots.Add(BaitUpgrade);

        ShopItem VisionUpgrade = new ShopItem();
        VisionUpgrade.Initialize("UpgradeVision", 5, 3);
        ShopSlots.Add(VisionUpgrade);

        moneyHud.Initialize();

        float PositionX = -700;
        float PositionY = -200;
        moneyHud.SetPosition(new Vector2f(PositionX, PositionY -200));

        foreach(ShopItem item in ShopSlots){
            item.SetPositon(new SFML.System.Vector2f(PositionX, PositionY));
            PositionX += 200;
        }
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        foreach(ShopItem item in ShopSlots){
            item.Update(deltaTime, MousePoint);
        }
        moneyHud.Update(deltaTime, InputManager.Instance);
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        foreach(ShopItem item in ShopSlots){
            item.Draw(window, states, shaderon);
        }
        moneyHud.Draw(window, states, shaderon);
    }
    public void UpdatePointer (FloatRect pointer){
        MousePoint = pointer;
    }
}