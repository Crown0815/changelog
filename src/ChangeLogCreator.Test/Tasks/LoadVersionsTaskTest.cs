﻿using ChangeLogCreator.Git;
using ChangeLogCreator.Model;
using ChangeLogCreator.Tasks;
using Moq;
using NuGet.Versioning;
using Xunit;

namespace ChangeLogCreator.Test.Tasks
{
    public class LoadVersionsTaskTest
    {

        [Fact]
        public void Run_adds_versions_from_tags()
        {
            // ARRANGE
            var tags = new GitTag[]
            {
                new GitTag("1.2.3-alpha", new GitId("01")),
                new GitTag("4.5.6", new GitId("02"))
            };

            var repoMock = new Mock<IGitRepository>(MockBehavior.Strict);
            repoMock.Setup(x => x.GetTags()).Returns(tags);

            var sut = new LoadVersionsTask(repoMock.Object);

            // ACT
            var changeLog = new ChangeLog();
            sut.Run(changeLog);

            // ASSERT
            Assert.All(
                tags,
                tag =>
                {
                    var version = SemanticVersion.Parse(tag.Name);
                    Assert.Contains(new VersionInfo(version, tag.Commit), changeLog.Versions);
                });
        }

        [Theory]
        [InlineData("not-a-version")]
        [InlineData("1.2.3.4")]
        public void Run_ignores_tags_that_are_not_a_valid_version(string tagName)
        {
            // ARRANGE
            var tags = new GitTag[]
            {
                new GitTag(tagName, new GitId("01")),
            };

            var repoMock = new Mock<IGitRepository>(MockBehavior.Strict);
            repoMock.Setup(x => x.GetTags()).Returns(tags);

            var sut = new LoadVersionsTask(repoMock.Object);

            // ACT
            var changeLog = new ChangeLog();
            sut.Run(changeLog);

            // ASSERT
            Assert.Empty(changeLog.Versions);
        }

        [Theory]
        [InlineData("1.2.3-alpha", "1.2.3-alpha")]
        [InlineData("1.2.3", "1.2.3")]
        // Tags may be prefixed with "v"
        [InlineData("v1.2.3-alpha", "1.2.3-alpha")]
        [InlineData("v4.5.6", "4.5.6")]
        public void Run_correctly_gets_version_from_tag_name(string tagName, string version)
        {
            // ARRANGE
            var tags = new GitTag[]
            {
                new GitTag(tagName, new GitId("0123")),
            };

            var repoMock = new Mock<IGitRepository>(MockBehavior.Strict);
            repoMock.Setup(x => x.GetTags()).Returns(tags);

            var expectedVersion = SemanticVersion.Parse(version);

            var sut = new LoadVersionsTask(repoMock.Object);

            // ACT
            var changeLog = new ChangeLog();
            sut.Run(changeLog);

            // ASSERT
            var versionInfo = Assert.Single(changeLog.Versions);
            Assert.Equal(expectedVersion, versionInfo.Version);
        }
    }
}
