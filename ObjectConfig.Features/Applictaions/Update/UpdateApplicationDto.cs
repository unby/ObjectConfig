using System.Collections.Generic;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class UpdateApplicationDto
    {
        public ApplicationDefinitionDto? ApplicationDefinition { get; set; }

        public List<ApplicationUserRolesDto>? Users { get; set; }
    }
}
