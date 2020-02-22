using System.Collections.Generic;
using SFML.Graphics;
using System;

namespace TcGame
{
  public class Scene : Drawable
  {
    private List<Actor> actors = new List<Actor>();
    private List<Actor> actorsToDestroy = new List<Actor>();
    private List<Actor> actorsToAdd = new List<Actor>();

    public T Create<T>(Actor Parent = null) where T : Actor
    {
      return Create(typeof(T), Parent) as T;
    }

    public object Create(Type type, Actor Parent = null)
    {
      Actor actor = null;

      if (type.IsSubclassOf(typeof(Actor)))
      {
        actor = Activator.CreateInstance(type) as Actor;
        actor.Parent = Parent;
        actor.FixParent();

        actorsToAdd.Add(actor);
      }

      return actor;
    }

    public void Destroy(Actor actor)
    {
      actorsToAdd.Remove(actor);
      actorsToDestroy.Add(actor);

      if (actor.OnDestroy != null)
      {
        actor.OnDestroy(actor);
      }
    }

    public void Update(float dt)
    {
      // Remove destroyed actors
      actorsToDestroy.ForEach(x =>
        {
          x.Parent = null;
          x.FixParent();
        });
      actors.RemoveAll(actorsToDestroy.Contains);

      // Add new actors
      actors.AddRange(actorsToAdd);
      actorsToAdd.Clear();

      // Fix parents relationship
      actors.ForEach(x => x.FixParent());

      // Update actors
      List<Actor> roots = actors.FindAll(x => x.Parent == null);
      roots.ForEach(x => x.UpdateRecursive(dt));
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
      List<Actor> roots = actors.FindAll(x => x.Parent == null);
      roots.ForEach(x => x.DrawRecursive(target, states));
    }

    public List<T> GetAll<T>() where T : Actor
    {
      return actors.FindAll(x => !actorsToDestroy.Contains(x)).FindAll(x => x is T).ConvertAll(x => x as T);
    }

    public T GetFirst<T>() where T : Actor
    {
      return actors.Find(x => x is T) as T;
    }

    public T GetRandom<T>() where T : Actor
    {
      Random r = new Random();

      List<T> tActors = GetAll<T>();
      return (tActors.Count > 0) ? tActors[r.Next(tActors.Count)] : null;
    }
  }
}
