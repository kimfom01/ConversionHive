using AutoMapper;
using ConversionHive.Dtos.MailConfigDto;
using ConversionHive.Entities;
using ConversionHive.Repository;

namespace ConversionHive.Services.Implementations;

public class MailConfigService : IMailConfigService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProcessor _jwtProcessor;
    private readonly IMapper _mapper;
    private readonly IEncoder _encoder;
    private readonly IDecoder _decoder;

    public MailConfigService(
        IUnitOfWork unitOfWork,
        IJwtProcessor jwtProcessor,
        IMapper mapper,
        IEncoder encoder,
        IDecoder decoder)
    {
        _unitOfWork = unitOfWork;
        _jwtProcessor = jwtProcessor;
        _mapper = mapper;
        _encoder = encoder;
        _decoder = decoder;
    }

    public async Task<ReadMailConfigDto> PostMailConfig(string authorization, CreateMailConfigDto createMailConfigDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var mailConfig = _mapper.Map<MailConfig>(createMailConfigDto);

        mailConfig.UserId = userId;
        mailConfig.Password = _encoder.EncodeValue(mailConfig.Password, mailConfig.SenderEmail);

        var added = await _unitOfWork.MailConfigs.AddItem(mailConfig);
        await _unitOfWork.SaveChangesAsync();

        var returnVal = _mapper.Map<ReadMailConfigDto>(added);

        return returnVal;
    }

    public async Task<ReadMailConfigDto> GetMailConfig(string authorization, int mailConfigId)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var mailConfig = await _unitOfWork.MailConfigs.GetItem(conf => 
            conf.Id == mailConfigId && conf.UserId == userId);

        var readMailConfigDto = _mapper.Map<ReadMailConfigDto>(mailConfig);

        readMailConfigDto.Password = _decoder.DecodeValue(readMailConfigDto.Password, readMailConfigDto.SenderEmail);

        return readMailConfigDto;
    }

    public async Task UpdateMailConfig(string authorization, UpdateMailConfigDto updateMailConfigDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);
    }
}