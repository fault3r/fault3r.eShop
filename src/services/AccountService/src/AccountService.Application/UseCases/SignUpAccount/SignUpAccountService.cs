
using System;
using AccountService.Application.DTOs;
using AccountService.Application.Extensions;
using AccountService.Application.Interfaces.Services;
using AccountService.Application.Interfaces.UseCases;
using AccountService.Domain.Entities;
using AccountService.Domain.Interfaces;
using FluentValidation;

namespace AccountService.Application.UseCases.SignUpAccount
{
    public class SignUpAccountService : ISignUpAccountService
    {
        private readonly IRepository<Account> _repository;
        
        private readonly IValidator<SignUpAccountRequest> _validator;

        private readonly ILoggerService<SignUpAccountService> _logger;


        public SignUpAccountService(
            IRepository<Account> repository,
            IValidator<SignUpAccountRequest> validator,
            ILoggerService<SignUpAccountService> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<SignUpAccountResult> ExecuteAsync(SignUpAccountRequest account)
        {
            var validate = await _validator.ValidateAsync(account);
            if (!validate.IsValid)
                return new SignUpAccountResult { Message = validate.ToErrorString() };        
            
        }
    }
}