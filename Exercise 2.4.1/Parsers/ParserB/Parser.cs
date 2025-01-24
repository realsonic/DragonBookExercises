namespace ParserB;

/// <summary>
/// Анализатор грамматики: <code>S ➡️ S ( S ) S | ϵ</code>
/// </summary>
/// <remarks>Анализатор написан эврестически.</remarks>
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
        if (lookahead is not null) throw new Exception("Source is too long");
    }

    private void S()
    {
        switch (lookahead)
        {
            case '(':
                Match('('); S(); Match(')'); S();
                break;

            default: // ϵ-продукция
                break;
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