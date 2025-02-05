using FluentAssertions;

using Lexers;
using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace LexerTests;

public class BaseLexerTests
{
    [Fact(DisplayName = "������ ���������� �������� �� ���������� �������")]
    public void Whitespace_string_returns_no_tokens()
    {
        // Arrange
        BaseLexer sut = new(" \t  \n ");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "��� ���� ��������� ������ ����� ������ - 3")]
    public void Two_new_lines_gives_Line_3()
    {
        // Arrange
        BaseLexer sut = new(" \n \n ");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
        sut.Line.Should().Be(3);
    }

    [Fact(DisplayName = "����� ������������")]
    public void Numbers_parsed()
    {
        // Arrange
        BaseLexer sut = new("1 23 456");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new NumberToken[]
        {
            new("1", null, 1),
            new("23", null, 23),
            new("456", null, 456)
        });
    }

    [Fact(DisplayName = "������ �������� ������������")]
    public void Boolean_parsed()
    {
        // Arrange
        BaseLexer sut = new("true false");

        // Act
        Token? firstToken = sut.GetNextToken();
        Token? secondToken = sut.GetNextToken();

        // Assert
        firstToken.Should().Be(new TrueToken(null));
        secondToken.Should().Be(new FalseToken(null));
    }

    [Fact(DisplayName = "�������������� ������������")]
    public void Id_parsed()
    {
        // Arrange
        BaseLexer sut = new("id1 id2");

        // Act
        Token? firstToken = sut.GetNextToken();
        Token? secondToken = sut.GetNextToken();

        // Assert
        firstToken.Should().Be(new IdToken("id1", null));
        secondToken.Should().Be(new IdToken("id2", null));
    }

    [Fact(DisplayName = "������ ������� ������������ ��� ��������� ������")]
    public void Others_parsed()
    {
        // Arrange
        BaseLexer sut = new(" ~ !@\n$");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new Token[]
        {
            new("~", null), new("!", null), new("@", null), new("$", null)
        });
    }
}