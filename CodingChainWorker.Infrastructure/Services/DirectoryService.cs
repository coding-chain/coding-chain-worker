using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CodingChainApi.Infrastructure.Common.Exceptions;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution;

namespace CodingChainApi.Infrastructure.Services
{
    public interface IDirectoryService
    {
        FileInfo GetTemplateDirectoryByParticipation(ParticipationTestingAggregate participation);
        void DeleteParticipationDirectory(ParticipationTestingAggregate participation);
    }

    public class DirectoryService : IDirectoryService
    {
        private readonly ITemplateSettings _templateSettings;
        private readonly IAppDataSettings _appDataSettings;

        public DirectoryService(ITemplateSettings templateSettings, IAppDataSettings appDataSettings)
        {
            _templateSettings = templateSettings;
            _appDataSettings = appDataSettings;
        }

        public FileInfo GetTemplateDirectoryByParticipation(ParticipationTestingAggregate participation)
        {
            var zipPath = GetZipPathByParticipation(participation);
            var extractDir = GetTemplateExtractPathByParticipation(participation);
            if (Directory.Exists(extractDir))
            {
                Directory.Delete(extractDir, true);
            }
            ZipFile.ExtractToDirectory(zipPath, extractDir);
            return new FileInfo(extractDir);
        }

        public void DeleteParticipationDirectory(ParticipationTestingAggregate participation)
        {
            var dir =  GetTemplateExtractPathByParticipation(participation);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
        }

        private TemplateSetting GetTemplateSettingsByLanguage(LanguageEnum language)
        {
            var templateSetting = _templateSettings.Templates.FirstOrDefault(t => t.Language == language);
            if (templateSetting is null)
                throw new InfrastructureException($"No template zip setting for language{language}");
            return templateSetting;
        }

        private string GetTemplateExtractPathByParticipation(ParticipationTestingAggregate participation)
        {
            return Path.GetFullPath(Path.Join(_appDataSettings.BasePath, _appDataSettings.ParticipationTemplatesPath,
                participation.Id.Value.ToString()));
        }

        private string GetZipPathByParticipation(ParticipationTestingAggregate participation)
        {
            return Path.GetFullPath(Path.Join(_appDataSettings.BasePath, _appDataSettings.TemplatesPath,
                GetTemplateSettingsByLanguage(participation.Language).Name));
        }
    }
}