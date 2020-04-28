﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.ChangeLog.Configuration;
using Grynwald.ChangeLog.Model;
using Grynwald.MarkdownGenerator;
using NuGet.Versioning;

namespace Grynwald.ChangeLog.Templates.GitLabRelease
{
    /// <summary>
    /// Template optimized to produce a Markdown file for use as description text of a GitLab Release
    /// </summary>
    internal class GitLabReleaseTemplate : MarkdownBaseTemplate
    {
        /// <inheritdoc />
        protected override MdSerializationOptions SerializationOptions => MdSerializationOptions.Default;


        public GitLabReleaseTemplate(ChangeLogConfiguration configuration) : base(configuration)
        { }


        /// <inheritdoc />
        protected override MdDocument GetChangeLogDocument(ApplicationChangeLog changeLog)
        {
            if (changeLog.ChangeLogs.Count() > 1)
                throw new TemplateExecutionException("The GitLab Release template cannot render change logs that contain multiple versions");

            if (!changeLog.ChangeLogs.Any())
                return new MdDocument(GetEmptyBlock());

            // Return changes for only a single change, omit surrounding headers
            return new MdDocument(
                GetVersionContentBlock(changeLog.Single())
            );
        }

        /// <inheritdoc />
        protected override MdBlock GetSummaryListHeaderBlock(string listTitle)
        {
            // in GitLab releases, the top heading is <h4> because higher
            // levels are used by the surrounding GitLab Web UI
            return new MdHeading(4, listTitle);
        }

        /// <inheritdoc />
        protected override MdBlock GetBreakingChangesListHeaderBlock(VersionInfo versionInfo)
        {
            // in GitLab releases, the top heading is <h4> because higher
            // levels are used by the surrounding GitLab Web UI
            return new MdHeading(4, "Breaking Changes");
        }

        /// <inheritdoc />
        protected override MdBlock GetDetailSectionHeaderBlock(NuGetVersion version)
        {
            // in GitLab releases, the top heading is <h4> because higher
            // levels are used by the surrounding GitLab Web UI
            return new MdHeading(4, "Details");
        }

        /// <inheritdoc />
        protected override MdBlock GetEntryDetailHeaderBlock(ChangeLogEntry entry)
        {
            // in GitLab releases, the top heading is <h4> because higher
            // levels are used by the surrounding GitLab Web UI
            // => the header for individual entries is the level of the "details" header +1 => 5
            return new MdHeading(5, GetSummaryText(entry));
        }

        /// <inheritdoc />
        protected override string GetHtmlHeadingId(ChangeLogEntry entry)
        {
            // use default header ids generated by GitLab instead of setting a header explicitly
            return new MdHeading(1, GetSummaryText(entry)).Anchor ?? "";
        }
    }
}
