﻿using Lexers.Locations;

namespace Lexers.Monads.Language;

public record RootMonad(Position Position) : UncompletedLexemeMonad(string.Empty, new Location(Position, Position))
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (character is ' ' or '\t' or '\r' or '\n')
            return new RootMonad(position);
        
        if (char.IsDigit(character))
            return new UncompletedNumberMonad(character.ToString(), Location.StartAt(position));

        return new UnknownLexemeMonad(character.ToString(), Location.StartAt(position));
    }

    public override LexemeMonad Finalize() => new RootMonad(this);
}
