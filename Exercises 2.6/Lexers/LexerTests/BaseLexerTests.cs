using FluentAssertions;

using Lexers;
using Lexers.Tokens;

namespace LexerTests;

public class BaseLexerTests
{
    [Fact(DisplayName = "—трока пробелов ничего не возвращает")]
    public void Whitespace_string_returns_nothing()
    {
        // Arrange
        BaseLexer baseLexer = new(" \t  \n ");

        // Act
        List<Token> tokens = baseLexer.Scan().ToList();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "ѕри двух переносах строки номер строки должен быть 3")]
    public void Two_new_lines_gives_Line_3()
    {
        // Arrange
        BaseLexer baseLexer = new(" \n \n  ");

        // Act
        List<Token> tokens = baseLexer.Scan().ToList();

        // Assert
        tokens.Should().BeEmpty();
        baseLexer.Line.Should().Be(3);
    }
}