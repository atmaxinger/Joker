using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Joker.Services.Jokes;

public class JokeHistory
{
    private readonly Stack<Joke> _previousJokes = new();
    private readonly Stack<Joke> _nextJokes = new();
    public Joke? CurrentJoke { get; private set; }
    
    public int JokesCount => _previousJokes.Count + _nextJokes.Count + (CurrentJoke == null ? 0 : 1);
    
    public bool HasPrevious => _previousJokes.Count > 0;

    public bool TryGetPreviousJoke([NotNullWhen(true)] out Joke? joke)
    {
        if (!HasPrevious)
        {
            joke = null;
            return false;
        }
        
        joke = _previousJokes.Pop();
        if (CurrentJoke != null)
        {
            _nextJokes.Push(CurrentJoke);
        }
        CurrentJoke = joke;
        return true;
    }
    
    public bool HasNext => _nextJokes.Count > 0;
    
    public bool TryGetNextJoke([NotNullWhen(true)] out Joke? joke)
    {
        if (!HasNext)
        {
            joke = null;
            return false;
        }
        
        joke = _nextJokes.Pop();
        if (CurrentJoke != null)
        {
            _previousJokes.Push(CurrentJoke);
        }
        
        CurrentJoke = joke;
        return true;
    }

    public void AddJoke(Joke joke)
    {
        if (CurrentJoke != null)
        {
            _previousJokes.Push(CurrentJoke);
        }
        
        CurrentJoke = joke;
    }

    public void Clear()
    {
        CurrentJoke = null;
        _previousJokes.Clear();
        _nextJokes.Clear();
    }
}