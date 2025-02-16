<!--
  <auto-generated>
    The contents of this file were generated by a tool.
    Any changes to this file will be overwritten.
    To change the content of this file, edit 'message-overrides.md.scriban'
  </auto-generated>
-->
# Commit Message Overrides

**Supported versions:** 1.0+

By default, changelog parses commit messages to generate the change log.
Since the commit message in git cannot (easily) be changed after committing, the change log entry cannot be changed afterwards either.

To allow editing the message used for generating the change log, changelog supports "Commit Message Overrides" which allow specifying a message that will be user by changelog instead of the commit message.

There are two ways to provide an alternate message for a commit:

- [Git Notes](#overriding-commit-messages-through-git-notes), added in version 1.0 (**default**)
- [Message Override Files](#overriding-commit-messages-through-override-files), added in version 1.1

By default, the Git Notes are used.
This can be customized through the [Message Override Provider setting](./configuration/settings/message-overrides.md#message-override-provider).

Both approaches have different advantages and downsides:

- [Git notes](https://git-scm.com/docs/git-notes) are a little difficult to handle as git will not fetch them by default but can be accessed independently of the currently checked out commit.
- Placing override files in the repository is more straight-forward but it may clutter the repository and you need to make sure to have the right branch checked out when generating the change log.

Note that when a override message is found, it **always** takes precedence over the commit message, regardless of whether the commit message and/or the override message can be parsed as conventional commit.

## Use Cases:

- **Modifying a change log entry:**

  Assuming a commit's message is a valid conventional commit, the resulting change log entry can be modified, by adding a override message that is a valid conventional commit message as well.

- **Adding a change log entry:**

  Commit messages which do not follow the conventional commits format will be ignored when generating the change log.
  To include the commit in the change log, add a override message which follows the conventional commits format.

- **Removing a change log entry**

  To remove a commit from the change log, add a override message that does *not* follow the conventional commits format.
  When generating the changelog, the commit will be ignored.

## Disabling Message Overrides

The commit message overrides are enabled by default. 
Overrides can be disabled by setting the [Enable Message Overrides](./configuration/settings/message-overrides.md#enable-message-overrides) setting to `false`.

## Overriding Commit Messages through Git Notes

When commit message overrides are configured to use git notes (see [Message Override Provider setting](./configuration/settings/message-overrides.md#message-override-provider)), override messages are read from the git notes namespace `changelog/message-overrides` .
This can be customized through the [Git Notes Namespace setting](./configuration/settings/message-overrides.md#git-notes-namespace).

### Working with Git Notes

For more information on git notes, please refer to the [Git Documentation](https://git-scm.com/docs/git-notes).

To add an override message, run

```ps1
git notes --ref "changelog/message-overrides" add "<COMMIT>"
```

where `<COMMIT>` is the SHA1 of the commit to add a note to.


Similarly, notes can be edited by running

```ps1
git notes --ref "changelog/message-overrides" edit "<COMMIT>"
```

or removed using 

```ps1
git notes --ref "changelog/message-overrides" remove "<COMMIT>"
```


To show message overrides in the output of `git log`, run 

```ps1
# Include "changelog message overrides" in the output of git log
git log --show-notes=changelog/message-overrides

# Show *all* notes in the output of git log
git log --show-notes=*
```


### Pushing

**⚠️ Note that git does not include notes by default in push or pull operations**

To push git notes, run 

```ps1
# Fetch changelog message overrides
git push origin "refs/notes/changelog/message-overrides"

# Push all notes
git push origin "refs/notes/*"
```

Before generating the change log, ensure that git notes have been fetched into the local repository by running

```ps1
# Fetch notes for the message override namespace
git fetch origin "refs/notes/changelog/message-overrides:refs/notes/changelog/message-overrides"

# Fetch all notes
git fetch origin "refs/notes/*:refs/notes/*"
```

## Overriding Commit Messages through Override Files

As an alternative to using git-notes, version 1.1 added the option to load override messages from the file system.

To use the file system, set the [Message Override Provider setting](./configuration/settings/message-overrides.md#message-override-provider) to `FileSystem`.

When enabled, changelog will search the directory `.config/changelog/message-overrides` (in the repository) for override files.
In this directory, place one or more text files which are named after the id of the git commit you wish to override the message for (without a file extension).

You can use the abbreviated commit id instead of the full 40-character id, but changelog will throw an error if multiple files in the directory resolve to the same commit
(e.g. when files for both the abbreviated and the full commit id are present).

For example, your repository with override files could look something like this:

```txt
<root>
 └─.config
    └─changelog
       └─message-overrides
          ├─ff186833f7a546173e77b43951bd94d57f4ccd82
          ├─3963110882b7e85dd4de9cfb4b140a9ad661b754
          └─a11f9b7
```

You can customize the directory to use for message overrides through the [Source Directory Path setting](./configuration/settings/message-overrides.md#source-directory-path).

💡Tip: To initialize a override file with the commit's original message, you can run

```ps1
git show -s --format=%B <COMMIT> > .\.config\changelog\message-overrides\<COMMIT>
```

where `<COMMIT>` is the SHA1 of the commit to set the message for.
<COMMIT>
## See Also

- [Commit Message Override Settings](./configuration/settings/message-overrides.md)
- [Git Notes Documentation](https://git-scm.com/docs/git-notes)