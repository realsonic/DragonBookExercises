﻿namespace ParserC;

/// <summary>
/// Анализатор грамматики: <code>S ➡️ 0 S 1 | 0 1</code>
/// </summary>
/// <remarks>Анализатор написан с использованием переписанной грамматики без повторящихся FIRST(S):
/// <code>
/// S ➡️ 0 S 1 | 0 1
///   ⬇️
/// S ➡️ 0 R
/// R ➡️ S 1 | 0 1
/// </code>
/// </remarks>
internal class ParserWithRewrittenGrammar
{
    public ParserWithRewrittenGrammar(IEnumerable<char> source)
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
        Match('0'); R();
    }

    private void R()
    {
        switch (lookahead)
        {
            case '0': // S может начаться только с 0
                S(); Match('1');
                break;

            case '1':
                Match('1');
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