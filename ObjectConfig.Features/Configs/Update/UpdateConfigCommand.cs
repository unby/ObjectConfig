using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;
using System;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public UpdateConfigCommand(string applicationCode, string environmentCode, string configCode, string versionFrom, string? versionTo)
            : base(applicationCode, environmentCode, configCode, versionFrom)
        {
            if (string.IsNullOrWhiteSpace(versionFrom))
            {
                throw new RequestException($"Parameter '{nameof(versionFrom)}' isn't should empty");
            }

            if (!string.IsNullOrWhiteSpace(versionTo))
            {
                To = new Version(versionFrom);
                VesionTo = Config.ConvertVersionToLong(To);
            }
        }

        public readonly Version? To;
        public readonly long? VesionTo;
    }
}
