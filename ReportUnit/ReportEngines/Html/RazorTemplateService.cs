using System.IO;
using System.Linq;
using System.Reflection;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines.Html
{
    public class RazorTemplateService
    {
        private readonly string _outputDirectory;
        private readonly IRazorEngineService _razor;

        public RazorTemplateService(string templatesNamespace, string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            var templateConfig = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                EncodedStringFactory = new RawStringFactory(),
                CachingProvider = new DefaultCachingProvider(x => { })
            };

            _razor = RazorEngineService.Create(templateConfig);
            var templateNames = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceNames();

            foreach (var templateName in templateNames.Where(name => name.EndsWith(".cshtml")))
            {
                var fileName = Path
                    .GetFileNameWithoutExtension(templateName.Remove(0, templatesNamespace.Length + 1))
                    .Replace('.', '/');
                _razor.AddTemplate(fileName, GetStringResource(templateName));
                _razor.Compile(fileName);
            }
        }

        public void Save<TModel>(TModel model)
            where TModel : IRenderable
        {
            var html = _razor.Run(model.TemplateName, typeof(TModel), model);
            File.WriteAllText(Path.Combine(_outputDirectory, model.FileName + ".html"), html);
        }

        private string GetStringResource(string name)
        {
            using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
