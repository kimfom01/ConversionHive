using ConversionHive.Dtos.MailConfigDto;

namespace ConversionHive.Services;

public interface IMailConfigService
{
    Task<ReadMailConfigDto> PostMailConfig(string authorization, CreateMailConfigDto createMailConfigDto);
    Task<ReadMailConfigDto> GetMailConfig(string authorization, int mailConfigId);
    Task UpdateMailConfig(string authorization, UpdateMailConfigDto updateMailConfigDto);
}