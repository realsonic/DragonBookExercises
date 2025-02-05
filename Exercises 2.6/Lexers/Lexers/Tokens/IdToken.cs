using Lexers.Locations;

namespace Lexers.Tokens;
public record IdToken(string Id, Location Location) : Token(Id, Location);
