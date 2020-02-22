using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace TcGame
{
  public class GiftBall : Gift
  {
    private Random Alea = new Random ();

    public GiftBall ()
    {
      Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Gifts/star"));
      base.Center ();
    }

    public override void Update (float dt)
    {
      base.Update (dt);
    }

    public override void GiveGift ()
    {
      base.GiveGift ();
     bool op;
      if (Alea.Next (0, 2) == 0) {
        op = true;
      } else {
        op = false;
      }
      Engine.Get.Scene.GetFirst<Pad> ().AddBall (op);
    }
  }
}
