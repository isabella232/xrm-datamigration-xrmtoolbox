﻿using Capgemini.DataMigration.Core;
using Capgemini.Xrm.CdsDataMigratorLibrary.Enums;
using Capgemini.Xrm.CdsDataMigratorLibrary.Services;
using Capgemini.Xrm.DataMigration.Config;
using Capgemini.Xrm.DataMigration.Core;
using Capgemini.Xrm.DataMigration.CrmStore.Config;
using Capgemini.Xrm.DataMigration.Engine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Capgemini.Xrm.CdsDataMigrator.Tests.Unit.Services
{
    [TestClass]
    public class CrmGenericMigratorFactoryTests : TestBase
    {
        private CrmGenericMigratorFactory systemUnderTest;

        [TestInitialize]
        public void TestSetup()
        {
            SetupServiceMocks();
            systemUnderTest = new CrmGenericMigratorFactory();
        }

        [TestMethod]
        public void CrmGenericMigratorIntantiation()
        {
            FluentActions.Invoking(() => new CrmGenericMigratorFactory())
                            .Should()
                            .NotThrow();
        }

        [TestMethod]
        public void RequestJsonMigrator()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepoMock = new Mock<IEntityRepository>();
            entityRepoMock.SetupGet(x => x.GetEntityMetadataCache).Returns(new Mock<IEntityMetadataCache>().Object);
            var exportConfig = new CrmExporterConfig
            {
                JsonFolderPath = Path.Combine(Environment.CurrentDirectory, "temp")
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();

            var migrator = systemUnderTest.GetCrmExportDataMigrator(DataFormat.Json, logger, entityRepoMock.Object, exportConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataExporter>();
        }

        [TestMethod]
        public void RequestCSVMigrator()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepoMock = new Mock<IEntityRepository>();
            entityRepoMock.SetupGet(x => x.GetEntityMetadataCache).Returns(new Mock<IEntityMetadataCache>().Object);
            var exportConfig = new CrmExporterConfig
            {
                JsonFolderPath = Path.Combine(Environment.CurrentDirectory, "temp")
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();
            schema.Entities.AddRange(new DataMigration.Model.CrmEntity[] { new DataMigration.Model.CrmEntity { } });

            var migrator = systemUnderTest.GetCrmExportDataMigrator(DataFormat.Csv, logger, entityRepoMock.Object, exportConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataExporterCsv>();
        }

        [TestMethod]
        public void RequestUnknownMigrator()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepo = new Mock<IEntityRepository>().Object;
            var exportConfig = new CrmExporterConfig();
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();

            FluentActions.Invoking(() => systemUnderTest.GetCrmExportDataMigrator(DataFormat.Unknown, logger, entityRepo, exportConfig, cancellationToken, schema))
                .Should()
                .Throw<NotSupportedException>();
        }

        [TestMethod]
        public void RequestJsonImportMigrator()
        {
            var logger = new Mock<ILogger>().Object;
            EntityRepositoryMock.SetupGet(x => x.GetEntityMetadataCache).Returns(new Mock<IEntityMetadataCache>().Object);
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();

            var migrator = systemUnderTest.GetCrmImportDataMigrator(DataFormat.Json, logger, EntityRepositoryMock.Object, importConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataImporter>();
        }

        [TestMethod]
        public void RequestCSVImportMigrator()
        {
            var logger = new Mock<ILogger>().Object;
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();
            schema.Entities.AddRange(new DataMigration.Model.CrmEntity[] { new DataMigration.Model.CrmEntity { } });

            var migrator = systemUnderTest.GetCrmImportDataMigrator(DataFormat.Csv, logger, EntityRepositoryMock.Object, importConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataImporterCsv>();
        }

        [TestMethod]
        public void RequestImportMigratorWhenDataFormatUnknown()
        {
            var logger = new Mock<ILogger>().Object;
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();
            schema.Entities.AddRange(new DataMigration.Model.CrmEntity[] { new DataMigration.Model.CrmEntity { } });

            FluentActions.Invoking(() => systemUnderTest.GetCrmImportDataMigrator(DataFormat.Unknown, logger, EntityRepositoryMock.Object, importConfig, cancellationToken, schema))
                .Should()
                .Throw<NotSupportedException>();
        }

        [TestMethod]
        public void RequestJsonImportMigratorWhenMaxThreadsGreaterThanOne()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepoMockList = new Mock<List<IEntityRepository>>();
            entityRepoMockList.Object.Add(EntityRepositoryMock.Object);
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();

            var migrator = systemUnderTest.GetCrmImportDataMigrator(DataFormat.Json, logger, entityRepoMockList.Object, importConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataImporter>();
        }

        [TestMethod]
        public void RequestCSVImportMigratorWhenMaxThreadsGreaterThanOne()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepoMockList = new Mock<List<IEntityRepository>>();
            entityRepoMockList.Object.Add(EntityRepositoryMock.Object);
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();
            schema.Entities.AddRange(new DataMigration.Model.CrmEntity[] { new DataMigration.Model.CrmEntity { } });

            var migrator = systemUnderTest.GetCrmImportDataMigrator(DataFormat.Csv, logger, entityRepoMockList.Object, importConfig, cancellationToken, schema);

            migrator.Should().BeOfType<CrmFileDataImporterCsv>();
        }

        [TestMethod]
        public void RequestImportMigratorWhenMaxThreadsGreaterThanOneAndDataFormatUnknown()
        {
            var logger = new Mock<ILogger>().Object;
            var entityRepoMockList = new Mock<List<IEntityRepository>>();
            entityRepoMockList.Object.Add(EntityRepositoryMock.Object);
            var importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = true,
                IgnoreSystemFields = true,
                SaveBatchSize = 1000,
                JsonFolderPath = "TestData"
            };
            var cancellationToken = CancellationToken.None;
            var schema = new CrmSchemaConfiguration();
            schema.Entities.AddRange(new DataMigration.Model.CrmEntity[] { new DataMigration.Model.CrmEntity { } });

            FluentActions.Invoking(() => systemUnderTest.GetCrmImportDataMigrator(DataFormat.Unknown, logger, entityRepoMockList.Object, importConfig, cancellationToken, schema))
                .Should()
                .Throw<NotSupportedException>();
        }
    }
}