﻿using System;
using System.Net;
using System.Reflection;
using Bit.Core.Contracts;
using Bit.Core.Models;
using Bit.Owin.Contracts;
using Bit.Owin.Exceptions;
using Bit.Owin.Metadata;

namespace Bit.Owin.Implementations
{
    public class DefaultExceptionToHttpErrorMapper : IExceptionToHttpErrorMapper
    {
        public virtual IAppEnvironmentProvider AppEnvironmentProvider
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(AppEnvironmentProvider));

                _activeAppEnvironment = value.GetActiveAppEnvironment();
            }
        }

        private AppEnvironment _activeAppEnvironment;

        protected virtual Exception UnWrapException(Exception exp)
        {
            if (exp == null)
                throw new ArgumentNullException(nameof(exp));

            if (exp is TargetInvocationException)
                return exp.InnerException;

            return exp;
        }

        public virtual string GetMessage(Exception exp)
        {
            exp = UnWrapException(exp);

            string message = BitMetadataBuilder.UnKnownError;

            if (IsKnownError(exp))
                message = exp.Message;
            else if (_activeAppEnvironment.DebugMode == true)
                message = exp.ToString();

            return message;
        }

        public virtual string GetReasonPhrase(Exception exp)
        {
            exp = UnWrapException(exp);

            string reasonPhrase = BitMetadataBuilder.UnKnownError;

            if (IsKnownError(exp))
                reasonPhrase = BitMetadataBuilder.KnownError;

            return reasonPhrase;
        }

        public virtual HttpStatusCode GetStatusCode(Exception exp)
        {
            exp = UnWrapException(exp);

            if (exp is IHttpStatusCodeAwareException httpStatusCodeAwareException)
                return httpStatusCodeAwareException.StatusCode;

            return HttpStatusCode.InternalServerError;
        }

        public virtual bool IsKnownError(Exception exp)
        {
            exp = UnWrapException(exp);

            if (exp is IKnwoException)
                return true;

            return false;
        }
    }
}
