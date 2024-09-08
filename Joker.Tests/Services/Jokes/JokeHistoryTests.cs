using Joker.Services.Jokes;

namespace Joker.Tests.Services.Jokes;

public class JokeHistoryTests
{
    private static readonly Joke Joke1 = new("Joke1", "");
    private static readonly Joke Joke2 = new("Joke2", "");
    private static readonly Joke Joke3 = new("Joke3", "");

    [Fact]
    public void AddJoke_EmptyHistory_ShouldNotHavePreviousOrNext()
    {
        // Arrange
        var jokeHistory = new JokeHistory();

        // Act
        jokeHistory.AddJoke(Joke1);

        // Assert
        Assert.False(jokeHistory.HasPrevious);
        Assert.False(jokeHistory.HasNext);
        Assert.Equal(Joke1, jokeHistory.CurrentJoke);
    }

    [Fact]
    public void AddJoke_ExistingJokes_ShouldHavePreviousButNotExt()
    {
        // Arrange
        var jokeHistory = new JokeHistory();

        jokeHistory.AddJoke(Joke1);

        // Act
        jokeHistory.AddJoke(Joke2);

        // Assert
        Assert.True(jokeHistory.HasPrevious);
        Assert.False(jokeHistory.HasNext);
        Assert.Equal(Joke2, jokeHistory.CurrentJoke);
    }

    [Fact]
    public void GetPreviousJokes_WithHistory_ShouldGetPreviousJokeAndHaveNextOne()
    {
        // Arrange
        var jokeHistory = new JokeHistory();
        jokeHistory.AddJoke(Joke1);
        jokeHistory.AddJoke(Joke2);
        jokeHistory.AddJoke(Joke3);
        
        // Act
        var successful = jokeHistory.TryGetPreviousJoke(out var previousJoke);
        
        // Arrange
        Assert.True(successful);
        Assert.NotNull(previousJoke);
        Assert.Equal(Joke2, previousJoke);
        Assert.Equal(Joke2, jokeHistory.CurrentJoke);
        Assert.True(jokeHistory.HasPrevious);
        Assert.True(jokeHistory.HasNext);
    }
}