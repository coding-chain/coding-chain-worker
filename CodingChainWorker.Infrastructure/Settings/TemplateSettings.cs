using System.Collections.Generic;
using Domain.TestExecution;

namespace CodingChainApi.Infrastructure.Settings
{
    public class TemplateSetting
    {
        public LanguageEnum Language { get; set; }
        public string Name { get; set; }

        public bool IsCompressed { get; set; } 
    }

    public interface ITemplateSettings
    {
        public IList<TemplateSetting> Templates { get; set; }
    }

    public class TemplateSettings : ITemplateSettings
    {
        public IList<TemplateSetting> Templates { get; set; }
    }
}