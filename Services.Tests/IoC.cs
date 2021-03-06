﻿#region Using directives

using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using Autofac;
using Autofac.Core;
using Contracts.Data;
using Core;
using Core.IoCModules;
using Data;
using Data.Entities;
using Providers;
using Providers.Data;
using Providers.External;

#endregion


namespace Services.Tests
{
    internal static class IoC
    {
        static IoC()
        {
            BuildContainer();
        }

        internal static IContainer Container { get; set; }

        private static string DbConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["AWContext"].ConnectionString; }
        }

        internal static void BuildContainer()
        {
            var builder = new ContainerBuilder();

            // database
            builder.Register(c => Database.DefaultConnectionFactory.CreateConnection(DbConnectionString)).Named<DbConnection>("DbConnection");
            builder.Register(c =>
            {
                var connection = c.ResolveNamed<DbConnection>("DbConnection");
                return new AWContext(connection, false);
            }).As<DbContext>();

            // services
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly).Where(t => t.Name.EndsWith("Service"));

            // providers
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ProductsProvider>().As<IProductsProvider>();
            builder.RegisterType<ExternalProvider>().As<IExternalProvider>();
            builder.RegisterGeneric(typeof(EFProvider<>)).As(typeof(IProvider<>));
            
            // logging
            builder.RegisterModule<IoCLoggingModule>();

            // operationcontext
            builder.RegisterInstance(new OperationContext(new UserDetails(1.ToString(), "Riff Raff", "riff.raff")));

            Container = builder.Build();
        }
       
    }
}