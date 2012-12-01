﻿/*=============================================================================
*
*	(C) Copyright 2011, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
*
*   http://www.TheCodeKing.co.uk
*  
*	All rights reserved.
*	The code and information is provided "as-is" without waranty of any kind,
*	either expressed or implied.
*
*=============================================================================
*/
using System;
using TheCodeKing.Utils.IoC;
using TheCodeKing.Utils.Serialization;
using XDMessaging.Specialized;

namespace XDMessaging.IoC
{
    public sealed class SimpleIocContainerBootstrapper
    {
        #region Constants and Fields

        private static Lazy<IocContainer> instance =
            new Lazy<IocContainer>(() => new SimpleIocContainer(c => new IocActivator(c), (c) => new SimpleIocScanner(c)).Initialize(Configure), true);

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public static IocContainer GetInstance()
        {
            return instance.Value;
        }

        internal static IocContainer GetInstance(bool clear)
        {
            if (clear)
            {
                instance = new Lazy<IocContainer>(() => new SimpleIocContainer(c => new IocActivator(c),
                                                                               c => new SimpleIocScanner(c)).
                                                            Initialize(Configure), true);
            }
            return GetInstance();
        }

        #endregion

        #region Methods

        public static void Configure(IocContainer container)
        {
            const string binarySerializer = "Binary";
            const string jsonSerializer = "Json";
            container.Register<ISerializer, JsonSerializer>(jsonSerializer);
            container.Register<ISerializer, BinaryBase64Serializer>(binarySerializer);
            container.Register<ISerializer>(
                () => new SpecializedSerializer(container.Resolve<ISerializer>(binarySerializer),
                                                container.Resolve<ISerializer>(jsonSerializer)));
            container.Scan.ScanAllAssemblies();
        }

        #endregion
    }
}