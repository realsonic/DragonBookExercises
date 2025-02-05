using Lexers.Locations;

namespace Lexers.Tokens;

public record NumberToken(string Lexeme, Location Location, int Value) : Token(Lexeme, Location);