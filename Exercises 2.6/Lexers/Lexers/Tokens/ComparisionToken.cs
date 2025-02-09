using Lexers.Locations;

namespace Lexers.Tokens;
public record ComparisionToken(string Lexeme, Location Location) : Token(Lexeme, Location);