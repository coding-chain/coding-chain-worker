﻿using System;
using Application.Contracts;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.Services.Processes;
using Domain.TestExecution;
using Microsoft.Extensions.DependencyInjection;

namespace CodingChainApi.Infrastructure.Factories
{
    public class ProcessServiceFactory: IProcessServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProcessService GetProcessServiceByLanguage(LanguageEnum language)
        {
            return language switch
            {
                LanguageEnum.Typescript => _serviceProvider.GetRequiredService<TypescriptProcessService>(),
                _ => _serviceProvider.GetRequiredService<CsharpProcessService>()
            };
        }
    }
}