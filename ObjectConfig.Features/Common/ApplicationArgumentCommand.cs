using ObjectConfig.Exceptions;
using System;

namespace ObjectConfig.Features.Common
{
    public abstract class ApplicationArgumentCommand
    {
        protected ApplicationArgumentCommand(string applicationCode)
        {
            if (string.IsNullOrWhiteSpace(applicationCode))
            {
                throw new RequestException($"Parameter '{nameof(applicationCode)}' isn't should empty");
            }

            ApplicationCode = applicationCode;
        }

        public string ApplicationCode { get; }
    }

    public static class CommnadExtentions
    {
        public static void ThrowNotFoundExceptionWhenValueIsNull<T, Command>(this Command command, T obj)
            where T : class
            where Command : ApplicationArgumentCommand
        {
            if (obj == null)
            {
                if (command is EnvironmentArgumentCommand environmenCommand)
                {
                    throw new NotFoundException($"Environment '{environmenCommand.EnvironmentCode}({command.ApplicationCode})' isn't found");
                }

                throw new NotFoundException($"Application '{command.ApplicationCode}' isn't found");
            }
        }

        public static void ThrowForbidenExceptionWhenValueIsNull<T, Command>(this Command command, T obj)
            where T : class
            where Command : ApplicationArgumentCommand
        {
            if (obj == null)
            {
                if (command is EnvironmentArgumentCommand environmenCommand)
                {
                    throw new ForbidenException($"Environment '{environmenCommand.EnvironmentCode}({command.ApplicationCode})' is denied access");
                }

                throw new ForbidenException($"Application '{command.ApplicationCode}' is denied access");
            }
        }

        public static void ThrowForbidenExceptionWhenValueIsNull<T>(this ApplicationArgumentCommand command, Func<bool> expression) where T : class
        {
            if (expression())
            {
                throw new ForbidenException($"Application '{command.ApplicationCode}' is denied access");
            }
        }
    }
}
