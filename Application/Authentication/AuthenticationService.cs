using Application.Common.Data;
using Domain.User.Errors;

namespace Application.Authentication;

using Application.Authentication.Model;
using Application.Common.Interfaces;
using Domain.Shared;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;
using Domain.User.Models.Dtos;
using Domain.User.Entities;
public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterResponse>> Register(CreateUserDto userDto)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmail(userDto.Email);
            if (existingUser is not null)
            {
                return Result<RegisterResponse>.Failure(UserError.UserAlreadyExists);
            }

            User user = User.Create(userDto.Name, userDto.Surname, userDto.Email, userDto.Password);

            await _userRepository.Add(user);
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);

            await _unitOfWork.SaveChangesAsync();
            return Result<RegisterResponse>.Success(new RegisterResponse(token, user.Name, user.Surname, user.Email));
        }
        catch (Exception e)
        {
            return Result<RegisterResponse>.Failure(new Error("error" ,e.Message));
        }
    }

    public RegisterResponse Login(string email, string password)
    {
        throw new NotImplementedException();
    }
}