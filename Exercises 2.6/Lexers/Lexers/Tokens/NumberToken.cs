using Lexers.Locations;

namespace Lexers.Tokens;

public record NumberToken(int Value, string Lexeme, Location Location) : Token(Lexeme, Location);