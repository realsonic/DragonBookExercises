using FluentAssertions;

using Lexers;
using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace LexerTests;

public class CommentLexer261Tests
{
    [Fact(DisplayName = "������ ���������� �������� �� ���������� �������")]
    public void Whitespace_string_returns_no_tokens()
    {
        // Arrange
        CommentLexer261 sut = new(" \t  \n ");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "��� ���� ��������� ������ ����� ������ - 3")]
    public void Two_new_lines_gives_Line_3()
    {
        // Arrange
        CommentLexer261 sut = new(" \n \n ");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
        sut.CurrentPosition.Line.Should().Be(3);
    }

    [Fact(DisplayName = "����� ������������")]
    public void Numbers_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("1 23 456");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new NumberToken[]
        {
            new(1, "1", ((1, 1), (1, 1))),
            new(23, "23", ((1, 3), (1, 4))),
            new(456, "456", ((1, 6), (1, 8)))
        });
    }

    [Fact(DisplayName = "������ �������� ������������")]
    public void Boolean_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("true false");

        // Act
        var tokens = sut.Scan().ToList();

        // Assert
        tokens[0].Should().Be(new FalseToken(((1, 1), (1, 4))));
        tokens[1].Should().Be(new TrueToken(((1, 5), (1, 9))));
    }

    [Fact(DisplayName = "�������������� ������������")]
    public void Id_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("id1 id2");

        // Act
        var tokens = sut.Scan().ToList();

        // Assert
        tokens[0].Should().Be(new IdToken("id1", ((1, 1), (1, 3))));
        tokens[1].Should().Be(new IdToken("id2", ((1, 4), (1, 6))));
    }

    [Fact(DisplayName = "������ ������� ������������ ��� ��������� ������")]
    public void Others_parsed()
    {
        // Arrange
        CommentLexer261 sut = new(" ~ !@\n$");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new Token[]
        {
            new("~", ((1, 2), (1, 2))), new("!", ((1, 4),(1, 4))), new("@", ((1, 5),(1, 5))), new("$", ((2, 1),(2, 1)))
        });
    }

    [Fact(DisplayName = "������������ ����������� ������������")]
    public void Single_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // ����������� 1
            // ����������� 2
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "����� � ����� ����� ������������� ������������� ������������")]
    public void Number_and_slash_extracted_between_singleline_comments()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // ����������� 1
            1234
            // ����������� 2
            / /
            // ����������� 3
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new Token[]
        {
            new NumberToken(1234, Lexeme: "1234", Location: ((2,1),(2,4))),
            new("/", ((4,1),(4,1))),
            new("/", ((4,3),(4,3)))
        });
    }

    [Fact(DisplayName = "������������� ����������� ������������")]
    public void Multi_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            /* 
                �������
                �������������
                �����������
            */
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }
}