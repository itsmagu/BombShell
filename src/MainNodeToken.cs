﻿using System.Collections.Generic;

namespace BombShell;

public partial class MainNode
{
    private void Tokenize(string commandString){ //TODO Tokenize()
        Log.Text += "\n&";
        List<Token> tokens = [];
        int current = 0;
        while (current < commandString.Length){
            switch (commandString[current]){
            case ' ':
                tokens.Add(
                    new Token(
                        TokenType.Whitespace,
                        current,
                        1,
                        " "
                    )
                );
                Log.Text += "+";
                current++;
                break;
            default:
                int starting = current;
                string content = "";
                while (true){
                    if (current == commandString.Length || commandString[current] == ' ')
                        break;
                    content += commandString[current];
                    Log.Text += "_";
                    current++;
                }
                tokens.Add(
                    new Token(
                        TokenType.Content,
                        starting,
                        content.Length,
                        content
                    )
                );
                break;
            }
        }
        foreach (Token token in tokens){
            Log.Text += $"\n{token}";
        }
    }

    private struct Token(TokenType type, int start, int consumed, string content)
    {
        private TokenType type = type;
        private int start = start;
        private int consumed = consumed;
        private string content = content;
        public override string ToString() =>
            $"{start}-{start + consumed}({consumed}) {content} as {type}";
    }

    private enum TokenType
    {
        None,
        Content,
        Whitespace
    }
}