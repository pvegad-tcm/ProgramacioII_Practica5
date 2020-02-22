using System;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;

namespace TcGame
{
  public class Ball : StaticActor
  {
    public float Speed = 800.0f;
    private Random Alea = new Random ();
    private SoundBuffer sb;
    private Sound s;

    public bool Overpowered = false;

    private enum State
    {
      FollowPad,
      Playing
    }

    private State state = new State ();

    public Vector2f Forward { get; set; }

    public Ball ()
    {
      Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Balls/ball_green"));
      Center ();
      Forward = new Vector2f (0.0f, -1.0f);
      Forward.Rotate(Alea.Next (-50, 50));
      state = State.FollowPad;
      Overpowered = false;
    }

    public void SetOverpowered (bool op)
    {
      if (op) {
        Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Balls/ball_silver"));
        Center ();
        Forward = new Vector2f (0.0f, -1.0f);
        Forward.Rotate(Alea.Next (-50, 50));
        state = State.FollowPad;
        Overpowered = op;
      } else {
        Sprite = new Sprite (Resources.Texture ("Arkanoid/Textures/Balls/ball_green"));
        Center ();
        Forward = new Vector2f (0.0f, -1.0f);
        Forward.Rotate(Alea.Next (-50, 50));
        state = State.FollowPad;
        Overpowered = op;
      }
    }

    public override void Update (float dt)
    {
      switch (state) {
      case State.FollowPad:
        Position = Engine.Get.Scene.GetFirst<Pad> ().Position + new Vector2f (0.0f, Engine.Get.Scene.GetFirst<Pad> ().GetLocalBounds ().Height / 2.0f);
        break;
      case State.Playing:
        Position += Forward * Speed * dt;
        break;
      }

      CheckCollisions ();
    }

    private void CheckCollisions ()
    {
      var wall = Engine.Get.Scene.GetFirst<BrickWall> ();
      if (wall != null) {
        wall.CheckBallCollision (this);
      }

      CheckPadCollision ();
      CheckWallCollision ();
    }

    private void CheckPadCollision ()
    {
      if (Speed > 0.0f) {
        FloatRect myBounds = GetGlobalBounds ();

        var pad = Engine.Get.Scene.GetFirst<Pad> ();
        if (pad != null) {
          if (myBounds.Intersects (pad.GetGlobalBounds ())) {
            float x = myBounds.Left + myBounds.Width / 2.0f;
            float padX = pad.Position.X;

            float d = (x - padX) / pad.GetLocalBounds ().Width;
            Forward = (new Vector2f (0.0f, -1.0f)).Rotate (90.0f * d);
            Position = new Vector2f (Position.X, pad.GetGlobalBounds ().Top - pad.GetLocalBounds ().Height / 2.0f);
            PlaySound ();
          }
        }
      }
    }

    private void CheckWallCollision ()
    {
      FloatRect myBounds = GetGlobalBounds ();

      Vector2u screenBounds = Engine.Get.Window.Size;

      if (myBounds.Left <= 0.0f) {
        Forward = new Vector2f (-Forward.X, Forward.Y);
        Position = new Vector2f (myBounds.Width * 0.5f, Position.Y);
        PlaySound ();
      } else if ((myBounds.Left + myBounds.Width * 0.5f) >= screenBounds.X) {
        Forward = new Vector2f (-Forward.X, Forward.Y);
        Position = new Vector2f (screenBounds.X - myBounds.Width * 0.5f, Position.Y);
        PlaySound ();
      } else if (myBounds.Top <= 0.0f) {
        Forward = new Vector2f (Forward.X, -Forward.Y);
        Position = new Vector2f (Position.X, myBounds.Height * 0.5f);
      } else if (myBounds.Top >= screenBounds.Y) {
        if (Engine.Get.Scene.GetAll<Ball> ().Count <= 1) {
          MyGame.Get.HUD.LifeDown ();
        }
        Destroy ();
      }
    }

    public void ChangeState (bool Play)
    {
      if (Play) { state = State.Playing; } else { state = State.FollowPad; }
    }

    public void PlaySound ()
    {
      if (state == State.Playing) {
        switch (Alea.Next (0, 3)) {
        case 0:
          sb = new SoundBuffer ("Data/Arkanoid/Sounds/ball01.wav");
          break;
        case 1:
          sb = new SoundBuffer ("Data/Arkanoid/Sounds/ball02.wav");
          break;
        case 2:
          sb = new SoundBuffer ("Data/Arkanoid/Sounds/ball03.wav");
          break;
        }

        s = new Sound (sb);
        s.Play ();
      }
    }
  }
}
