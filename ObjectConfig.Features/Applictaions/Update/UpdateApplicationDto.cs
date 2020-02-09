using System.Collections.Generic;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class UpdateApplicationDto
    {
        public DefinitionDto? ApplicationDefinition { get; set; }

        public List<ApplicationUserRolesDto>? Users { get; set; }
    }
}
