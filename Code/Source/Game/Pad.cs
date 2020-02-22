using System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace TcGame
{
  public class Pad : StaticActor
  {
    public bool CanMove { get; set; }
    private float Speed = 1100f;

    private List<Ball> balls = new List<Ball> ();

    public Pad()
    {
      CanMove = false;
      Sprite = new Sprite(Resources.Texture("Arkanoid/Textures/Bats/bat_blue"));
      Center();
    }

    public Ball AddBall(bool op)
    {
      //TODO: Exercise 2
      Ball b = Engine.Get.Scene.Create<Ball> ();
      b.SetOverpowered (op);
      balls.Add (b);
      return null;
    }

    public void LaunchBalls()
    {
      //TODO: Exercise 2
      foreach (Ball x in balls) {
        x.ChangeState (true);
      }
    }

    public void Reset()
    {
      var window = Engine.Get.Window;
      Position = new Vector2f(window.Size.X / 2.0f, window.Size.Y - 100.0f);
    }

    public override void Update(float dt)
    {
      if (CanMove)
      {
        if (Keyboard.IsKeyPressed (Keyboard.Key.A) && Position.X > 0.0f + this.GetLocalBounds ().Width / 2.0f) {
          Position -= new Vector2f (1.0f, 0.0f) * Speed * dt;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.D) && Position.X < Engine.Get.Window.Size.X - this.GetLocalBounds ().Width / 2.0f) {
          Position += new Vector2f (1.0f, 0.0f) * Speed * dt;
        }
      }

      if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) 
      {
        LaunchBalls ();
      }
    }
  }
}

