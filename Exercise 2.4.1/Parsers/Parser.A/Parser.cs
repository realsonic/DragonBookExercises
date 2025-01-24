namespace ParserA;

/// <summary>
/// Анализатор грамматики: <code>S ➡️ +SS | -SS | a</code>
/// </summary>
internal class Parser
{
    public Parser(IEnumerable<char> source)
    {
        sourceEnumerator = source.GetEnumerator();
        lookahead = NextTerminal();
    }

    public void Parse()
    {
        S();
        if(lookahead is not null) throw new Exception("Source is too long");
    }

    private void S()
    {
        switch (lookahead)
        {
            case '+':
                Match('+'); S(); S();
                break;

            case '-':
                Match('-'); S(); S();
                break;

            case 'a':
                Match('a');
                break;

            case null:
                throw new Exception($"Source is too short");

            default:
                throw new Exception($"Syntax error: unexpected '{lookahead}'");
        }
    }

    private void Match(char terminal)
    {
        if (lookahead == terminal) lookahead = NextTerminal();
        else throw new Exception($"Syntax error: expected '{terminal}', but met '{lookahead}'");
    }

    private char? NextTerminal() => sourceEnumerator.MoveNext() ? sourceEnumerator.Current : null;

    private readonly IEnumerator<char> sourceEnumerator;
    private char? lookahead;
}
