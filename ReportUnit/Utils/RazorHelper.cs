using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;

namespace ReportUnit.Utils
{
    public static class RazorHelper
    {
        public static string GetTemplate(string templateName)
        {
            return ResourceHelper.GetStringResource($"ReportUnit.Templates.{templateName}.cshtml");
        }

        public static string CompileTemplate<TModel>(TModel model, string templateName)
        {
            var template = GetTemplate(templateName);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), typeof(TModel), model);
        }

        public static void SaveTemplate<TModel>(TModel model, string filePath, string templateName)
        {
            var html = CompileTemplate(model, templateName);
            File.WriteAllText(filePath, html);
        }
    }
}
