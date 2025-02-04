using System.Collections;

namespace Lexers;

public class PositionedEnumerable(IEnumerable<char> enumerable) : IEnumerable<(char, Position)>
{
    IEnumerator<(char, Position)> IEnumerable<(char, Position)>.GetEnumerator()
    {
        return Enumerate().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Enumerate().GetEnumerator();
    }

    private IEnumerable<(char, Position)> Enumerate()
    {
        Position position = new(1, 0);

        foreach (char character in enumerable)
        {
            position = character is '\n' ? position.NewLine() : position.AddColumn();
            yield return (character, position);
        }
    }
}