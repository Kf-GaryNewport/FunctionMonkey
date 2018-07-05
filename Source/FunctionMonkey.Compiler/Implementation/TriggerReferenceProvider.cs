﻿using System;
using System.Collections.Generic;
using System.Reflection;
using FunctionMonkey.Model;
using Microsoft.Azure.WebJobs;

namespace FunctionMonkey.Compiler.Implementation
{
    internal interface ITriggerReferenceProvider
    {
        Assembly GetTriggerReference(AbstractFunctionDefinition functionDefinition);
    }

    internal class TriggerReferenceProvider : ITriggerReferenceProvider
    {
        private static readonly Dictionary<Type, Assembly> TriggerReferences = new Dictionary<Type, Assembly>
        {
            {typeof(HttpFunctionDefinition), typeof(HttpTriggerAttribute).Assembly},
            {typeof(ServiceBusQueueFunctionDefinition), typeof(ServiceBusTriggerAttribute).Assembly },
            {typeof(ServiceBusSubscriptionFunctionDefinition), typeof(ServiceBusTriggerAttribute).Assembly },
            {typeof(StorageQueueFunctionDefinition), typeof(QueueTriggerAttribute).Assembly },
            {typeof(BlobStreamFunctionDefinition), typeof(BlobTriggerAttribute).Assembly },
            {typeof(BlobFunctionDefinition), typeof(BlobTriggerAttribute).Assembly },
            {typeof(TimerFunctionDefinition), typeof(TimerTriggerAttribute).Assembly }
        };

        public Assembly GetTriggerReference(AbstractFunctionDefinition functionDefinition)
        {
            if (TriggerReferences.TryGetValue(functionDefinition.GetType(), out Assembly assembly))
            {
                return assembly;
            }
            throw new ConfigurationException($"No trigger reference mapping configured for a function of type {functionDefinition.GetType().Name}");
        }
    }
}
