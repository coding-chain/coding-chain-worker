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
        FileInfo? GetTemplateDirectoryByParticipation(ParticipationAggregate participation);
        void DeleteParticipationDirectory(ParticipationAggregate participation);
    }

    public class DirectoryService : IDirectoryService
    {
        private readonly IAppDataSettings _appDataSettings;
        private readonly ITemplateSettings _templateSettings;

        public DirectoryService(ITemplateSettings templateSettings, IAppDataSettings appDataSettings)
        {
            _templateSettings = templateSettings;
            _appDataSettings = appDataSettings;
            Directory.CreateDirectory(TemplateExtractParentDirectoryPath);
            Directory.CreateDirectory(TemplateZipParentDirectoryPath);
        }

        private string TemplateExtractParentDirectoryPath =>
            Path.Join(_appDataSettings.BasePath, _appDataSettings.ParticipationTemplatesPath);

        private string TemplateZipParentDirectoryPath =>
            Path.Join(_appDataSettings.BasePath, _appDataSettings.TemplatesPath);

        public FileInfo? GetTemplateDirectoryByParticipation(ParticipationAggregate participation)
        {
            var zipPath = GetZipPathByParticipation(participation);
            var extractDir = GetTemplateExtractPathByParticipation(participation);
            if (Directory.Exists(extractDir)) return new FileInfo(extractDir);

            ClearDeleteFileFlag(extractDir);
            ZipFile.ExtractToDirectory(zipPath, extractDir);
            if (ClearDeleteFileFlag(extractDir))
            {
                DeleteParticipationDirectoryWithoutDeleteFileFlag(participation, out var templatePath);
                return null;
            }

            return new FileInfo(extractDir);
        }

        public void DeleteParticipationDirectory(ParticipationAggregate participation)
        {
            if (DeleteParticipationDirectoryWithoutDeleteFileFlag(participation, out var templateDirectoryPath))
                WriteDeleteFlagFile(templateDirectoryPath);
        }

        private bool ClearDeleteFileFlag(string templateDirectoryPath)
        {
            var deleteFileFlagName = GetDeleteFileFlagName(templateDirectoryPath);
            if (File.Exists(deleteFileFlagName))
            {
                File.Delete(deleteFileFlagName);
                return true;
            }

            return false;
        }

        private string GetDeleteFileFlagName(string templateDirectoryPath)
        {
            return $"{templateDirectoryPath}.delete";
        }

        private void WriteDeleteFlagFile(string templateDirectoryPath)
        {
            File.Create(GetDeleteFileFlagName(templateDirectoryPath)).Close();
        }

        private bool DeleteParticipationDirectoryWithoutDeleteFileFlag(ParticipationAggregate participation,
            out string templateDirectoryPath)
        {
            var dir = GetTemplateExtractPathByParticipation(participation);
            templateDirectoryPath = dir;
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
                return true;
            }

            return false;
        }

        private TemplateSetting GetTemplateSettingsByLanguage(LanguageEnum language)
        {
            var templateSetting = _templateSettings.Templates.FirstOrDefault(t => t.Language == language);
            if (templateSetting is null)
                throw new InfrastructureException($"No template zip setting for language{language}");
            return templateSetting;
        }


        private string GetTemplateExtractPathByParticipation(ParticipationAggregate participation)
        {
            return Path.GetFullPath(Path.Join(TemplateExtractParentDirectoryPath, participation.Id.Value.ToString()));
        }

        private string GetZipPathByParticipation(ParticipationAggregate participation)
        {
            return Path.GetFullPath(Path.Join(TemplateZipParentDirectoryPath,
                GetTemplateSettingsByLanguage(participation.Language).Name));
        }
    }
}