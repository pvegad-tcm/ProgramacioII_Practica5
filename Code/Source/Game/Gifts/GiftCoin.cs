using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace TcGame
{
  public class GiftCoin : Gift
  {
    SoundBuffer sb;
    Sound s;

    public GiftCoin ()
    {
      Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Gifts/coin"));
      base.Center ();
    }

    public override void Update (float dt)
    {
      base.Update (dt);

      if (Rotation >= 360) {
        Rotation -= 360;
      }

      Rotation += 200f * dt;
    }
    public override void GiveGift ()
    {
      base.GiveGift ();
      MyGame.Get.HUD.PointUp (25);

      sb = new SoundBuffer (Resources.Sound("Arkanoid/Sounds/coin"));
      s = new Sound (sb);  
      s.Play ();
    }
  }
}

