using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace TcGame
{
  public class MyGame : Game
  {
    /// <summary>
    /// Game States
    /// </summary>
    private enum State
    {
      None,
      WaitingToStart,
      Playing,
      WaitingForBall,
      GameOver
    }

    /// <summary>
    /// Current State of the game
    /// </summary>
    private State currentState = State.None;

    public HUD HUD { private set; get; }

    private BrickWall brickWall;
    private Pad pad;
    private Random Alea = new Random ();
    private SoundBuffer sb;
    private Sound s;

    private float timeWaiting = 0;

    private Music song;

    public Background Background { get; private set; }

    /// <summary>
    /// Singleton instance
    /// </summary>
    private static MyGame instance;

    /// <summary>
    /// Returns the Singleton Instance
    /// </summary>
    public static MyGame Get {
      get {
        if (instance == null) {
          instance = new MyGame ();
        }

        return instance;
      }
    }

    /// <summary>
    /// Private Constructor (Singleton pattern purposes)
    /// </summary>
    private MyGame ()
    {

    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    public void Init ()
    {
      var engine = Engine.Get;

      Background = engine.Scene.Create<Background> ();

      ChangeState (State.WaitingToStart);

      Engine.Get.Window.KeyPressed += HandleKeyPressed;

      song = new Music ("Data/Arkanoid/Music/arkanoid2012beta.wav");
      song.Loop = true;
      song.Play ();
    }

    void HandleKeyPressed (object sender, KeyEventArgs e)
    {
      if (e.Code == Keyboard.Key.Return) {
        if (currentState == State.WaitingToStart) {
          ChangeState (State.Playing);
        }
      }
    }

    /// <summary>
    /// DeInitializes the game
    /// </summary>
    public void DeInit ()
    {

    }

    /// <summary>
    /// Updates the Finite State Machine (FSM) of the game
    /// </summary>
    public void Update (float dt)
    {
      switch (currentState) {
      case State.WaitingToStart: {
          WaitingToStart (dt);
          break;
        }

      case State.Playing: {
          if (Engine.Get.Scene.GetAll<Ball> ().Count < 1) {
            ChangeState (State.WaitingForBall);
          }
          Playing (dt);
          break;
        }

      case State.WaitingForBall: {
          WaitingForBall (dt);
          break;
        }

      case State.GameOver: {
          GameOver (dt);
          break;
        }

      default: {
          break;
        }
      }
    }

    /// <summary>
    /// Method with code associated to FSM transitions
    /// </summary>
    private void ChangeState (State newState)
    {
      // Exit state logic
      if (currentState == State.None) {
        HUD = Engine.Get.Scene.Create<HUD> ();
      } else if (currentState == State.WaitingToStart) {
        // HUD
        HUD.HideInfo ();
        HUD.NumLifes = 5;
        HUD.NumPoints = 0;
        HUD.ShowScore = true;

        // Pad
        pad = Engine.Get.Scene.Create<Pad> (Background);
        pad.Reset ();
        pad.CanMove = true;

        // BrickWall
        brickWall = Engine.Get.Scene.Create<BrickWall> (Background);
        brickWall.Position = new Vector2f (100.0f, 100.0f);
        brickWall.ConstructWall (8, 12);

      } else if (currentState == State.Playing) {
        switch (Alea.Next (0, 2)) {
        case 0:
          sb = new SoundBuffer ("Data/Arkanoid/Sounds/bomb01.wav");
          break;
        case 1:
          sb = new SoundBuffer ("Data/Arkanoid/Sounds/bomb02.wav");
          break;
        }
        s = new Sound (sb);
        s.Play ();

      } else if (currentState == State.GameOver) {
        Engine.Get.Scene.Destroy (brickWall);
        DestroyAll<Brick> ();
        brickWall = null;
      }

      //-----------------------------------------------------------

      // Enter state logic
      if (newState == State.WaitingToStart) {
        //TODO: Exercise 1

        HUD.ShowInfo ("Press Start");
      } else if (newState == State.Playing) {
        
        //Ball
        pad.AddBall (false);

      } else if (newState == State.WaitingForBall) {
        DestroyAll<Ball> ();
        DestroyAll<Gift> ();
        timeWaiting = 0;

      } else if (newState == State.GameOver) {
        DestroyAll<Ball> ();
        DestroyAll<Pad> ();
        DestroyAll<Gift> ();
        HUD.ShowScore = false;
        HUD.ShowInfo ("Game Over");
        timeWaiting = 0;
      }

      currentState = newState;
    }

    /// <summary>
    /// WaitingToStart state
    /// </summary>
    private void WaitingToStart (float dt)
    {
      
    }

    /// <summary>
    /// Playing state
    /// </summary>
    private void Playing (float dt)
    {
      if (Get.HUD.NumLifes == 0) {
        ChangeState (State.GameOver);
      }
    }

    /// <summary>
    /// WaitingForBall state
    /// </summary>
    private void WaitingForBall (float dt)
    {
      timeWaiting += dt;
      if (timeWaiting >= 1.0f) {
        ChangeState (State.Playing);
      }
    }

    /// <summary>
    /// GameOver state
    /// </summary>
    private void GameOver (float dt)
    {
      timeWaiting += dt;
      if (timeWaiting >= 5.0f) {
        ChangeState (State.WaitingToStart);
      }
    }

    /// <summary>
    /// Auxiliar method for destroy things
    /// </summary>
    private void DestroyAll<T> () where T : Actor
    {
      var actors = Engine.Get.Scene.GetAll<T> ();
      actors.ForEach (x => x.Destroy ());
    }

    public void Killed ()
    {
      if (Get.HUD.NumLifes == 0) {
        ChangeState (State.GameOver);
      } else {
        ChangeState (State.WaitingForBall);
      }
    }
  }
}
