using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public static class CommandExtentions
    {
        public static void ThrowNotFoundExceptionWhenValueIsNull<T, Command>(this Command command, T obj)
            where T : class
            where Command : ApplicationArgumentCommand
        {
            if (obj == null)
            {
                if (command is ConfigArgumentCommand configCommand)
                {
                    throw new NotFoundException($"Config '{configCommand.EnvironmentCode}(env:{configCommand.EnvironmentCode}, app:{configCommand.ApplicationCode})' isn't found");
                }

                if (command is EnvironmentArgumentCommand environmenCommand)
                {
                    throw new NotFoundException($"Environment '{environmenCommand.EnvironmentCode}(app:{command.ApplicationCode})' isn't found");
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
                if (command is ConfigArgumentCommand configCommand)
                {
                    throw new NotFoundException($"Config '{configCommand.EnvironmentCode}(env:{configCommand.EnvironmentCode}, app:{configCommand.ApplicationCode})' is denied access");
                }

                if (command is EnvironmentArgumentCommand environmenCommand)
                {
                    throw new ForbidenException($"Environment '{environmenCommand.EnvironmentCode}(app:{command.ApplicationCode})' is denied access");
                }

                throw new ForbidenException($"Application '{command.ApplicationCode}' is denied access");
            }
        }
    }
}