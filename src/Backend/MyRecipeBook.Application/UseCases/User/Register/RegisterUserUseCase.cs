using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.AutoMapper;
// using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    // private readonly PasswordEncripter _passwordEncripter;
    private readonly IMapper _mapper;


    public RegisterUserUseCase(
        IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        // PasswordEncripter passwordEncripter,
        IMapper mapper)
    {
        _writeOnlyRepository =  writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        // _passwordEncripter = passwordEncripter;
        _mapper =  mapper;
    }

public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
       await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        // user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);
        
        await  _unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        
        var result = validator.Validate(request);
        
        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        // if (emailExist)
        //     result.Errors.Add(new ValidationFailure(string.Empty,ResourceMessagesException));

        if (result.IsValid == false)
        {
            var errorMessages =  result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}