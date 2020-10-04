﻿using System;
using Grynwald.ChangeLog.ConventionalCommits;
using Grynwald.ChangeLog.Model;
using Xunit.Abstractions;

namespace Grynwald.ChangeLog.Test.Model
{
    /// <summary>
    /// Wrapper class to make <see cref="ChangeLogEntryFooter"/> serializable by xunit
    /// </summary>
    public sealed class XunitSerializableChangeLogEntryFooter : IXunitSerializable
    {
        internal ChangeLogEntryFooter Value { get; private set; }


        internal XunitSerializableChangeLogEntryFooter(ChangeLogEntryFooter value) => Value = value;


        [Obsolete("For use by Xunit only", true)]
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public XunitSerializableChangeLogEntryFooter()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.


        public void Deserialize(IXunitSerializationInfo info)
        {
            var name = info.GetValue<string>(nameof(ChangeLogEntryFooter.Name));
            var value = info.GetValue<string>(nameof(ChangeLogEntryFooter.Value));
            var webUri = info.GetValue<string>(nameof(ChangeLogEntryFooter.WebUri));

            Value = new ChangeLogEntryFooter(
                new CommitMessageFooterName(name),
                value
            );

            if (webUri != null)
            {
                Value.WebUri = new Uri(webUri);
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Value.Name), Value.Name.Value);
            info.AddValue(nameof(Value.Value), Value.Value);
            info.AddValue(nameof(Value.WebUri), Value.WebUri?.ToString());
        }

        internal static XunitSerializableChangeLogEntryFooter Wrap(ChangeLogEntryFooter value) => new XunitSerializableChangeLogEntryFooter(value);
    }
}
