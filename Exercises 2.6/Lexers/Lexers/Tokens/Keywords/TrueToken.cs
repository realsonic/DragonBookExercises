using Lexers.Locations;

namespace Lexers.Tokens.Keywords;
public record TrueToken(Location Location) : Token("true", Location);
