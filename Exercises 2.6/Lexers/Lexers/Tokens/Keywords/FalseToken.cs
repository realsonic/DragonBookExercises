using Lexers.Locations;

namespace Lexers.Tokens.Keywords;
public record FalseToken(Location Location) : Token("false", Location);
