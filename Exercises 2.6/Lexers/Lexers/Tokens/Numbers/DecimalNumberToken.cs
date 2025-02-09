using Lexers.Locations;

namespace Lexers.Tokens.Numbers;

public record DecimalNumberToken(decimal Value, string Lexeme, Location Location) : Token(Lexeme, Location);