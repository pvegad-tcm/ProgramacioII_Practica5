using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace TcGame
{
  public class GiftBomb : Gift
  {
    public GiftBomb ()
    {
      Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Gifts/bomb"));
      base.Center ();
    }

    public override void Update (float dt)
    {
      base.Update (dt);
    }

    public override void GiveGift ()
    {
      base.GiveGift ();
      MyGame.Get.HUD.LifeDown ();
      MyGame.Get.Killed ();
    }
  }
}
