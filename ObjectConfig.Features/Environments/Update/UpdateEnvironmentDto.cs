using ObjectConfig.Features.Applictaions.Update;
using System.Collections.Generic;

namespace ObjectConfig.Features.Environments.Update
{
    public class UpdateEnvironmentDto
    {
        public DefinitionDto? ApplicationDefinition { get; set; }

        public List<EnvironmentUserRolesDto>? Users { get; set; }
    }
}
