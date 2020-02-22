using System;
using SFML.Graphics;
using SFML.Window;

namespace TcGame
{
  public class Gift : StaticActor
  {
    private float Speed;
    private Vector2f Forward = new Vector2f (0, 1);
    private Random Alea = new Random ();

    public Gift()
    {
      Speed = Alea.Next (50, 100);
    }

    public override void Update(float dt)
    {
      Position += Forward * Speed * dt;
      // Check if collides with pad
      CheckPadCollision();
    }

    private void CheckPadCollision()
    {
      var pad = Engine.Get.Scene.GetFirst<Pad>();
      if (pad != null)
      {
        if (pad.GetGlobalBounds().Contains(WorldPosition.X, WorldPosition.Y))
        {
          GiveGift();
          Destroy();
        }
      }
    }

    /// <summary>
    /// Executes the gift action when the pad takes it
    /// </summary>
    public virtual void GiveGift()
    {

    }
  }
}

