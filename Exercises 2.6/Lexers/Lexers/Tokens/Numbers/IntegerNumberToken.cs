using Lexers.Locations;

namespace Lexers.Tokens.Numbers;

public record IntegerNumberToken(int Value, string Lexeme, Location Location) : Token(Lexeme, Location);