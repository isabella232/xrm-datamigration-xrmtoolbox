﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Capgemini.Xrm.CdsDataMigratorLibrary.Services
{
    public interface IMetadataService
    {
        List<EntityMetadata> RetrieveEntities(IOrganizationService orgService);

        EntityMetadata RetrieveEntities(string logicalName, IOrganizationService orgService, IExceptionService dataMigratorExceptionHelper);
    }
}