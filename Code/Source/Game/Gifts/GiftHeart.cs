using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace TcGame
{
  public class GiftHeart : Gift
  {
    private SoundBuffer sb;
    private Sound s;

    public GiftHeart ()
    {
      Sprite = new Sprite (Resources.Texture("Arkanoid/Textures/Gifts/heart"));
      base.Center ();
    }

    public override void Update (float dt)
    {
      base.Update (dt);

      if (Rotation >= 360) {
        Rotation -= 360;
      }

      Rotation += 150f * dt;
    }

    public override void GiveGift ()
    {
      base.GiveGift ();
      MyGame.Get.HUD.LifeUp ();

      sb = new SoundBuffer (Resources.Sound("Arkanoid/Sounds/life"));
      s = new Sound (sb);
      s.Play ();
    }
  }
}
