﻿using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace Lexers;

public class CommentLexer261
{
    public CommentLexer261(IEnumerable<char> input)
    {
        inputEnumerator = input.GetEnumerator();
        hasCurrentChar = inputEnumerator.MoveNext();
    }

    public IEnumerable<Token> Scan()
    {
        Token? token;
        while ((token = GetNextToken()) != null)
        {
            yield return token;
        }
    }

    public Token? GetNextToken()
    {
        for (; ; NextChar())
        {
            if (CurrentChar is ' ' or '\t' or '\r') continue;
            else if (CurrentChar is '\n') Line++;
            else if (CurrentChar is '/')
            {
                NextChar();
                if (CurrentChar is '/')
                {
                    do
                    {
                        NextChar();
                    } while (CurrentChar is not null and not '\n');
                }
                else
                {
                    return new Token("/");
                }
            }
            else break;
        }

        if (CurrentChar is not null && char.IsDigit(CurrentChar.Value))
        {
            string numberString = string.Empty;
            do
            {
                numberString += CurrentChar;
                NextChar();
            } while (CurrentChar is not null && char.IsDigit(CurrentChar.Value));

            return new NumberToken(numberString, int.Parse(numberString));
        }

        if (CurrentChar is not null && char.IsLetter(CurrentChar.Value))
        {
            string wordString = string.Empty;
            do
            {
                wordString += CurrentChar;
                NextChar();
            } while (CurrentChar is not null && char.IsLetterOrDigit(CurrentChar.Value));

            if (Words.TryGetValue(wordString, out Token? token))
                return token;

            IdToken idToken = new(wordString);
            Words.Add(wordString, idToken);
            return idToken;
        }

        if (CurrentChar is not null)
        {
            Token token = new(CurrentChar.Value.ToString());
            NextChar();
            return token;
        }

        return null;
    }

    public int Line { get; private set; } = 1;

    #region Private members

    private readonly IEnumerator<char> inputEnumerator;

    private char? CurrentChar => hasCurrentChar ? inputEnumerator.Current : null;
    private void NextChar() => hasCurrentChar = inputEnumerator.MoveNext();

    private bool hasCurrentChar;

    private readonly Dictionary<string, Token> Words = new()
    {
        { "true", new TrueToken() },
        { "false", new FalseToken() }
    };

    #endregion
}
