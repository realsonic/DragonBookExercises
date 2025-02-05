using Lexers.Locations;

namespace Lexers.Monads;

public record UnknownLexemeMonad(string Lexeme, Location Location) : LexemeMonad(Lexeme, Location);