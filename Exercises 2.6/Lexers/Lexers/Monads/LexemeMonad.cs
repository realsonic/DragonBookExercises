using Lexers.Locations;

namespace Lexers.Monads;
public abstract record LexemeMonad(string Lexeme, Location Location);
