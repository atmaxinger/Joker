using System;

namespace Joker.Services.Jokes;

public record Joke(string Text, string Category, DateTime Loaded);