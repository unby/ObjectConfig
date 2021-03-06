﻿using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.FindConfig
{
    public class FindConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public FindConfigCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom)
                : base(applicationCode, environmentCode, configCode, versionFrom)
        {
        }
    }
}
