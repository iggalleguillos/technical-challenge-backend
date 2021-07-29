using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalChallenge.Application.User.Queries;
using TechnicalChallenge.Domain.Interfaces;
using TechnicalChallenge.Infrastructure.Repositories;
using TechnicalChallenge.Infrastructure.Services;

namespace TechnicalChallenge.API.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtService>()
                .As<IJwtService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EncryptService>()
                .As<IEncryptService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAuthQueries>()
                .As<IUserAuthQueries>()
                .WithParameter(new ResolvedParameter((p, ctx) => p.ParameterType == typeof(IUserRepository), (p, ctx) => ctx.Resolve<IUserRepository>()))
                .WithParameter(new ResolvedParameter((p, ctx) => p.ParameterType == typeof(IJwtService), (p, ctx) => ctx.Resolve<IJwtService>()))
                .WithParameter(new ResolvedParameter((p, ctx) => p.ParameterType == typeof(IEncryptService), (p, ctx) => ctx.Resolve<IEncryptService>()))
                .InstancePerLifetimeScope();
        }
    }
}
