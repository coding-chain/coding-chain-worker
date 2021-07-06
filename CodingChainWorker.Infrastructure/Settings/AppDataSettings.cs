namespace CodingChainApi.Infrastructure.Settings
{
    public interface IAssetsSettings
    {
        string TemplatesPath { get; set; }
        public string ParticipationTemplatesPath { get; set; }
    }

    public class AssetsSettings : IAssetsSettings
    {
        public string TemplatesPath { get; set; }
        public string ParticipationTemplatesPath { get; set; }
    }
}