# ChangeLog

[![NuGet](https://img.shields.io/nuget/v/Grynwald.ChangeLog.svg)](https://www.nuget.org/packages/Grynwald.ChangeLog)
[![MyGet](https://img.shields.io/myget/ap0llo-changelog/vpre/Grynwald.ChangeLog.svg?label=myget)](https://www.myget.org/feed/ap0llo-changelog/package/nuget/Grynwald.ChangeLog)

[![Build Status](https://dev.azure.com/ap0llo/OSS/_apis/build/status/changelog?branchName=master)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=17&branchName=master)
[![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/ap0llo/OSS/17)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=17&branchName=master)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg)](https://conventionalcommits.org)
[![Renovate](https://img.shields.io/badge/Renovate-enabled-brightgreen)](https://renovatebot.com/)

## Overview

ChangeLog is a tool to generate a change log based from a project's git history
using [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).

## Documentation

- [Getting started](./docs/getting-started.md)
- [Configuration](./docs/configuration.md)
- [Integrations](./docs/integrations.md)
- [Templates](./docs/templates/README.md)
- [Commandline Reference](./docs/commandline-reference/index.md)
- [Automatic References](./docs/auto-references.md)
- [Commit Message Overrides](./docs/message-overrides.md)

## Building from source

ChangeLog is a .NET Core application.
Building it from source requires the .NET 6 SDK (version 6.0.101 as specified in [global.json](./global.json)) and uses [Cake](https://cakebuild.net/) for the build.

To execute the default task, run

```ps1
.\build.ps1
```

This will build the project, run all tests and pack the NuGet package.

## Issues

If you run into any issues or if you are missing a feature, feel free
to open an [issue](https://github.com/ap0llo/changelog/issues).

I'm also using issues as a backlog of things that come into my mind or
things I plan to implement, so don't be surprised if many issues were
created by me without anyone else being involved in the discussion.

## Acknowledgments

This project was made possible through a number of libraries and tools (aside from .NET Core).
Thanks to all the people contributing to these projects:

- [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning/)
- [FluentValidation](https://fluentvalidation.net/)
- [LibGit2Sharp](https://github.com/libgit2/libgit2sharp)
- [CommandLineParser](https://github.com/gsscoder/commandline)
- [Microsoft.Extensions.Configuration](https://github.com/dotnet/extensions)
- [Microsoft.Extensions.Logging](https://github.com/dotnet/extensions)
- [NuGet.Versioning](https://github.com/NuGet/NuGet.Client)
- [OctoKit](https://github.com/octokit/octokit.net)
- [GitLabApiClient](https://github.com/nmklotas/GitLabApiClient)
- [Autofac](https://autofac.org/)
- [ApprovalTests](https://github.com/approvals/ApprovalTests.Net)
- [Moq](https://github.com/moq/moq4)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [Mono.Cecil](https://github.com/jbevain/cecil/)
- [xUnit](http://xunit.github.io/)
- [Coverlet](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator)
- [NetArchTest](https://github.com/BenMorris/NetArchTest)
- [Microsoft.CodeAnalysis.CSharp](https://github.com/dotnet/roslyn)
- [Markdig](https://github.com/lunet-io/markdig)
- [SourceLink](https://github.com/dotnet/sourcelink)
- [Scriban](https://github.com/lunet-io/scriban)
- [Spectre.Console](https://spectresystems.github.io/spectre.console/)
- [Xunit.Combinatorial](https://github.com/AArnott/Xunit.Combinatorial)
- [CliWrap](https://github.com/Tyrrrz/CliWrap)
- [Zio](https://github.com/xoofx/zio)
- [Cake](https://cakebuild.net/)
- [Cake.BuildSystems.Module](https://github.com/cake-contrib/Cake.BuildSystems.Module)

## Versioning and Branching

The version of the library is automatically derived from git and the information
in `version.json` using [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning):

- The master branch  always contains the latest version. Packages produced from
  master are always marked as pre-release versions (using the `-pre` suffix).
- Stable versions are built from release branches. Build from release branches
  will have no `-pre` suffix
- Builds from any other branch will have both the `-pre` prerelease tag and the git
  commit hash included in the version string

To create a new release branch use the [`nbgv` tool](https://www.nuget.org/packages/nbgv/)
(at least version `3.0.24`):

```ps1
dotnet tool install --global nbgv 
nbgv prepare-release
```
