﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using Grynwald.ChangeLog.Configuration;
using Grynwald.ChangeLog.Model;
using Grynwald.ChangeLog.Model.Text;
using Grynwald.ChangeLog.Templates.ViewModel;
using Scriban;
using Scriban.Runtime;

namespace Grynwald.ChangeLog.Templates
{
    internal abstract class ScribanBaseTemplate : ITemplate
    {
        private class EnumerableFunctions : ScriptObject
        {
            public static bool Any(IEnumerable enumerable) => enumerable.Cast<object>().Any();

            public static new int Count(IEnumerable enumerable) => enumerable.Cast<object>().Count();

            public static object First(IEnumerable enumerable) => enumerable.Cast<object>().First();

            public static object Single(IEnumerable enumerable) => enumerable.Cast<object>().Single();
        }

        private class TextElementFunctions : ScriptObject
        {
            public static bool IsLink(ITextElement element) => element is IWebLinkTextElement;

            public static bool IsChangeLogEntryReference(ITextElement element) => element is ChangeLogEntryReferenceTextElement;
        }

        private class ChangeLogEntryFunctions : ScriptObject
        {
            //TODO: This should be handled in the view model
            public static bool HasScope(ChangeLogEntry entry) => !String.IsNullOrEmpty(entry.Scope);
        }

        private readonly ChangeLogConfiguration m_Configuration;


        public ScribanBaseTemplate(ChangeLogConfiguration configuration)
        {
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public void SaveChangeLog(ApplicationChangeLog changeLog, string outputPath)
        {
            var viewModel = new ApplicationChangeLogViewModel(m_Configuration, changeLog);

            var templateLoader = CreateTemplateLoader();


            var templateContext = new TemplateContext()
            {
                TemplateLoader = templateLoader
            };
            var rootScriptObject = new ScriptObject()
            {
                { "model", viewModel },
                { "enumerable", new EnumerableFunctions() },
                { "textelement", new TextElementFunctions() },
                { "changelogentry", new ChangeLogEntryFunctions() },
            };
            templateContext.PushGlobal(rootScriptObject);

            var entryTemplate = Template.Parse(templateLoader.LoadEntryTemplate());
            var rendered = entryTemplate.Render(templateContext);
            File.WriteAllText(outputPath, rendered);
        }


        protected abstract ScribanTemplateLoader CreateTemplateLoader();
    }
}
