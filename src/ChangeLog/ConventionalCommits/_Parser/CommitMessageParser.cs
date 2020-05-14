﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grynwald.ChangeLog.ConventionalCommits
{
    /// <summary>
    /// Parser for commit messages following the <see href="https://www.conventionalcommits.org">Conventional Commits </see> format.
    /// </summary>
    /// <seealso href="https://www.conventionalcommits.org">Conventional Commits</see>
    /// <remarks>
    /// As different parsing rules apply to the different parts of the commit message (header/subject, body and footers),
    /// The parser and tokenizer is split into multiple parts.
    /// <para>
    /// This parser uses <see cref="LineTokenizer"/> that splits the input into either lines with content or "blank" lines.
    /// These lines are then parsed by the "second layer" of parsers (<see cref="HeaderParser"/> and <see cref="FooterParser"/>) that
    /// parse a single line and have their own tokenizers.
    /// </para>
    /// <para>
    /// The message body is handled directly in <see cref="CommitMessageParser"/> because the body has no inner structure and can be parsed by simply
    /// distinguishing between non-blank and blank lines.
    /// </para>
    /// </remarks>
    /// <seealso cref="HeaderParser"/>
    /// <seealso cref="FooterParser"/>
    public class CommitMessageParser : Parser<LineToken, LineTokenKind>
    {
        private readonly string m_Input;


        private CommitMessageParser(string commitMessage)
        {
            m_Input = commitMessage ?? throw new ArgumentNullException(nameof(commitMessage));

        }


        protected override IReadOnlyList<LineToken> GetTokens() => LineTokenizer.GetTokens(m_Input).ToArray();

        private CommitMessage Parse()
        {
            // Reset parser
            Reset();

            // Parse Header
            var header = HeaderParser.Parse(MatchToken(LineTokenKind.Line));

            // Parse Body
            IReadOnlyList<string> body = Array.Empty<string>();
            if (!TestToken(LineTokenKind.Eof))
            {
                body = ParseBody();
            }

            // Parse Footer(s)
            IReadOnlyList<CommitMessageFooter> footers = Array.Empty<CommitMessageFooter>();
            if (!TestToken(LineTokenKind.Eof))
            {
                footers = ParseFooters();
            }

            // Consume all trailing blank lines (ignored)
            while (TestAndMatchToken(LineTokenKind.Blank, out _))
            { }

            // Ensure all tokens were parsed
            MatchToken(LineTokenKind.Eof);

            return new CommitMessage(header, body, footers);
        }

        private IReadOnlyList<string> ParseBody()
        {
            var body = new List<string>();

            // parse paragraphs until we reach the first footer
            while (TestForParagraph())
            {
                // Paragraphs are separated from earlier paragraphs and the subject line by a blank line
                // There must be at least one blank line, multiple consecutive blank lines are treated the same as a single blank line.
                MatchTokens(LineTokenKind.Blank, 1);
                body.Add(ParseParagraph());
            }

            return body;
        }

        private string ParseParagraph()
        {
            var currentParagraph = new StringBuilder();

            while (TestAndMatchToken(LineTokenKind.Line, out var line))
            {
                currentParagraph.AppendLine(line.Value);
            }

            return currentParagraph.ToString();
        }

        private bool TestForParagraph()
        {
            // both paragraphs and footer are separated from previous paragraphs and the header using a blank line.
            // If there is at least 1 blank line followed by a non-blank line,
            // the line could be either the first line of the next paragraph of the first footer.
            // To determine if we reached the end of the body, test if the next line is
            // a valid start of a footer

            return
                TestTokens(LineTokenKind.Blank, out var blanklineCount) &&  // check for at least one blank line                                                                                
                TestToken(LineTokenKind.Line, blanklineCount) &&            // check if blank lines are followed by a non-blank line(look ahead the number of blank lines matched)                                                                    
                !FooterParser.IsFooter(Peek(blanklineCount));               // check if the line is the start of a footer
        }

        private IReadOnlyList<CommitMessageFooter> ParseFooters()
        {
            // Footers are separated by the message body using a blank line.
            // There must be at least one blank line, multiple consecutive blank lines are treated the same as a single blank line.
            MatchTokens(LineTokenKind.Blank, 1);

            var footers = new List<CommitMessageFooter>();
            while (TestAndMatchToken(LineTokenKind.Line, out var currentLine))
            {
                footers.Add(FooterParser.Parse(currentLine));

                // ignore blank lines between footers
                MatchTokens(LineTokenKind.Blank, 0);
            }
            return footers;
        }


        /// <summary>
        /// Parses the specified commit message.
        /// </summary>
        /// <param name="commitMessage">The commit message to be parsed.</param>
        /// <exception cref="ParserException">Thrown when the message could not be parsed</exception>
        public static CommitMessage Parse(string commitMessage) => new CommitMessageParser(commitMessage).Parse();
    }
}
