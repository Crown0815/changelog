﻿using CommandLine;
using Xunit;

namespace Grynwald.ChangeLog.Test
{
    internal static class CommandLineAssert
    {
        public static T Parsed<T>(ParserResult<T> parserResult)
        {
            Assert.Equal(ParserResultType.Parsed, parserResult.Tag);
            Assert.Equal(typeof(CommandLineParameters), parserResult.TypeInfo.Current);
            var parsed = (Parsed<T>)parserResult;
            return parsed!.Value;
        }
    }
}
