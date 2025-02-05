using Lexers.Locations;

namespace Lexers.Tokens;

public record Token(string Lexeme, Location Location);