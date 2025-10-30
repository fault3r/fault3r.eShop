using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Application.Interfaces.UseCases;
using AccountService.Domain.Entities;
using AccountService.Domain.Interfaces;

namespace AccountService.Application.UseCases.SignUpAccount
{

    public class SignUpAccountService : ISignUpAccountService
    {
        private readonly IRepository<Account> _repository;

        private readonly ILoggerService<SignUpAccountService> _logger;


        public SignUpAccountService(IRepository<Account> repository,
            ILoggerService<SignUpAccountService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // public async Task<> ExecuteAsync()
        // {
            
        // }
    }
}