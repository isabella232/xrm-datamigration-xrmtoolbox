﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Enums;
using Capgemini.Xrm.CdsDataMigratorLibrary.Presenters;
using Capgemini.Xrm.CdsDataMigratorLibrary.Services;
using Capgemini.Xrm.CdsDataMigratorLibrary.Tests.Unit.Extensions;
using Capgemini.Xrm.DataMigration.Config;
using Capgemini.Xrm.DataMigration.CrmStore.Config;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Capgemini.Xrm.CdsDataMigrator.Tests.Unit.Presenters
{
    [TestClass]
    public class ExportPagePresenterTests
    {
        private Mock<IExportPageView> mockExportView;
        private Mock<IWorkerHost> mockWorkerHost;
        private Mock<IDataMigrationService> mockDataMigrationService;
        private Mock<INotifier> mockNotifier;
        private ExportPagePresenter systemUnderTest;

        [TestInitialize]
        public void TestSetup()
        {
            mockExportView = new Mock<IExportPageView>();
            mockWorkerHost = new Mock<IWorkerHost>();
            mockDataMigrationService = new Mock<IDataMigrationService>();
            mockNotifier = new Mock<INotifier>();

            systemUnderTest = new ExportPagePresenter(mockExportView.Object, mockWorkerHost.Object, mockDataMigrationService.Object, mockNotifier.Object);
        }

        [TestMethod]
        public void Constructor_ShouldSetDefaultConfigProperties()
        {
            // Arrange
            var exportConfig = new CrmExporterConfig();

            // Assert
            VerifyViewPropertiesSet(exportConfig);
        }

        [TestMethod]
        public void LoadConfig_ShouldSetConfigPropertiesWhenValidFilePathSelected()
        {
            // Arrange
            var exportConfigFilePath = @"TestData\ExportConfig.json";
            var exportConfig = CrmExporterConfig.GetConfiguration(exportConfigFilePath);
            mockExportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns(exportConfigFilePath);

            // Act
            mockExportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            VerifyViewPropertiesSet(exportConfig);
        }

        [TestMethod]
        public void LoadConfig_ShouldDoNothing_WhenInvalidFilePathSelected()
        {
            // Arrange
            mockExportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns("$a-random-non-existent-file$");

            // Act
            mockExportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            VerifyViewPropertiesNotSet();
        }

        [TestMethod]
        public void LoadConfig_ShouldDoNothingWhenEmptyFilePathSelected()
        {
            // Arrange
            mockExportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns("");

            // Act
            mockExportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            VerifyViewPropertiesNotSet();
        }

        [TestMethod]
        public void LoadConfig_ShouldNotifyExceptionWhenAnExceptionIsThrown()
        {
            // Arrange
            var thrownException = new Exception("Test exception");
            mockExportView
                .Setup(x => x.AskForFilePathToOpen())
                .Throws(thrownException);

            // Act
            mockExportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            mockNotifier.Verify(x => x.ShowError(thrownException));
        }

        [TestMethod]
        public void SaveConfig_ShouldUpdateOrCreateConfigFileWhenValidFilePathSelected()
        {
            // Arrange
            var exportConfigFilePath = @"TestData\NewExportConfig.json";
            mockExportView.SetupGet(x => x.PageSize).Returns(1000);
            mockExportView.SetupGet(x => x.BatchSize).Returns(2000);
            mockExportView.SetupGet(x => x.TopCount).Returns(3000);
            mockExportView.SetupGet(x => x.OnlyActiveRecords).Returns(true);
            mockExportView.SetupGet(x => x.OneEntityPerBatch).Returns(false);
            mockExportView.SetupGet(x => x.CrmMigrationToolSchemaPath).Returns(@"C:\\Some\Path\To\A\Schema.xml");
            mockExportView.SetupGet(x => x.JsonFolderPath).Returns(@"C:\\Some\Path\To\A\Folder");
            mockExportView.SetupGet(x => x.FilePrefix).Returns("Release_X_");
            mockExportView.SetupGet(x => x.CrmMigrationToolSchemaFilters).Returns(new Dictionary<string, string> { { "entity", "filters" } });
            mockExportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns(exportConfigFilePath);

            // Act
            mockExportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            var exportConfig = CrmExporterConfig.GetConfiguration(exportConfigFilePath);
            exportConfig.PageSize.Should().Be(1000);
            exportConfig.BatchSize.Should().Be(2000);
            exportConfig.TopCount.Should().Be(3000);
            exportConfig.OnlyActiveRecords.Should().Be(true);
            exportConfig.OneEntityPerBatch.Should().Be(false);
            exportConfig.CrmMigrationToolSchemaPaths.Count.Should().Be(1);
            exportConfig.CrmMigrationToolSchemaPaths.FirstOrDefault().Should().Be(@"C:\\Some\Path\To\A\Schema.xml");
            exportConfig.JsonFolderPath.Should().Be(@"C:\\Some\Path\To\A\Folder");
            exportConfig.FilePrefix.Should().Be("Release_X_");
            exportConfig.CrmMigrationToolSchemaFilters.Should().BeEquivalentTo(new Dictionary<string, string> { { "entity", "filters" } });
        }

        [TestMethod]
        [Ignore("What is an invalid file?")]
        public void SaveConfig_ShouldDoNothingWhenInvalidFilePathSelected()
        {
            // Arrange
            mockExportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns("a-random-non-existent-file");

            // Act
            mockExportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
        }

        [TestMethod]
        public void SaveConfig_ShouldDoNothingWhenEmptyFilePathSelected()
        {
            // Arrange
            mockExportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns("");

            // Act
            mockExportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
        }

        [TestMethod]
        public void SaveConfig_ShouldReuseLoadedFilePath()
        {
            // Arrange
            var exportConfigFilePath = @"TestData\NewExportConfig.json";
            var exportConfig = CrmExporterConfig.GetConfiguration(exportConfigFilePath);
            mockExportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns(exportConfigFilePath);
            mockExportView
                .Setup(x => x.AskForFilePathToSave(exportConfigFilePath))
                .Returns(exportConfigFilePath);

            // Act
            mockExportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);
            mockExportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
        }

        [TestMethod]
        public void SaveConfig_ShouldNotifyExceptionWhenAnExceptionIsThrown()
        {
            // Arrange
            var thrownException = new Exception("Test exception");
            mockExportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Throws(thrownException);

            // Act
            mockExportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            mockNotifier.Verify(x => x.ShowError(thrownException));
        }

        [TestMethod]
        public void RunConfig_ShouldReadValuesFromView()
        {
            // Arrange
            var mockIOrganisationService = new Mock<IOrganizationService>();
            mockExportView.SetupGet(x => x.PageSize).Returns(1000);
            mockExportView.SetupGet(x => x.BatchSize).Returns(2000);
            mockExportView.SetupGet(x => x.TopCount).Returns(3000);
            mockExportView.SetupGet(x => x.OnlyActiveRecords).Returns(true);
            mockExportView.SetupGet(x => x.OneEntityPerBatch).Returns(false);
            mockExportView.SetupGet(x => x.SeperateFilesPerEntity).Returns(false);
            mockExportView.SetupGet(x => x.DataFormat).Returns(CdsDataMigratorLibrary.Enums.DataFormat.Json);
            mockExportView.SetupGet(x => x.CrmMigrationToolSchemaPath).Returns(@"C:\\Some\Path\To\A\Schema.xml");
            mockExportView.SetupGet(x => x.JsonFolderPath).Returns(@"C:\\Some\Path\To\A\Folder");
            mockExportView.SetupGet(x => x.FilePrefix).Returns("Release_X_");
            mockExportView.SetupGet(x => x.CrmMigrationToolSchemaFilters).Returns(new Dictionary<string, string> { { "entity", "filters" } });
            mockExportView.SetupGet(x => x.Service).Returns(mockIOrganisationService.Object);

            // Act
            mockExportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);
            var workInfo = mockWorkerHost.Invocations[0].Arguments[0].As<WorkAsyncInfo>();
            workInfo.Work(null, null);

            // Assert
            mockExportView.VerifyAll();
            workInfo.Message.Should().Be("Exporting data...");
            mockDataMigrationService.Verify(x => x.ExportData(mockIOrganisationService.Object, CdsDataMigratorLibrary.Enums.DataFormat.Json, It.IsAny<CrmExporterConfig>()));
            var exportConfig = mockDataMigrationService.Invocations[0].Arguments[2].As<CrmExporterConfig>();
            exportConfig.PageSize.Should().Be(1000);
            exportConfig.BatchSize.Should().Be(2000);
            exportConfig.TopCount.Should().Be(3000);
            exportConfig.OnlyActiveRecords.Should().Be(true);
            exportConfig.OneEntityPerBatch.Should().Be(false);
            exportConfig.SeperateFilesPerEntity.Should().Be(false);
            exportConfig.CrmMigrationToolSchemaPaths.Count.Should().Be(1);
            exportConfig.CrmMigrationToolSchemaPaths.FirstOrDefault().Should().Be(@"C:\\Some\Path\To\A\Schema.xml");
            exportConfig.JsonFolderPath.Should().Be(@"C:\\Some\Path\To\A\Folder");
            exportConfig.FilePrefix.Should().Be("Release_X_");
            exportConfig.CrmMigrationToolSchemaFilters.Should().BeEquivalentTo(new Dictionary<string, string> { { "entity", "filters" } });
        }

        [TestMethod]
        public void RunConfig_ShouldNotifyExceptionWhenAnExceptionIsThrownOutsideWorkerHost()
        {
            // Arrange
            var thrownException = new Exception("Test exception");
            mockWorkerHost
                .Setup(x => x.WorkAsync(It.IsAny<WorkAsyncInfo>()))
                .Throws(thrownException);

            // Act
            mockExportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);

            // Assert
            mockExportView.VerifyAll();
            mockNotifier.Verify(x => x.ShowError(thrownException));
        }

        [TestMethod]
        public void RunConfig_ShouldNotifyExceptionWhenAnExceptionIsThrownInsideWorkerHost()
        {
            // Arrange
            var thrownException = new Exception("Test exception");
            mockDataMigrationService
                .Setup(x => x.ExportData(It.IsAny<IOrganizationService>(), It.IsAny<DataFormat>(), It.IsAny<CrmExporterConfig>()))
                .Throws(thrownException);

            // Act
            mockExportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);
            mockWorkerHost.ExecuteWork(0);

            // Assert
            mockExportView.VerifyAll();
            mockNotifier.Verify(x => x.ShowError(thrownException));
        }

        [TestMethod]
        public void RunConfig_ShouldNotifySuccessWhenNotExceptionIsThrownInsideWorkerHost()
        {
            // Act
            mockExportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);
            mockWorkerHost.ExecuteWork(0);

            // Assert
            mockExportView.VerifyAll();
            mockNotifier.Verify(x => x.ShowSuccess("Data export is complete."));
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsNull()
        {
            // Arrange
            CrmSchemaConfiguration result = null;
            mockExportView
                            .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(() => null);
            mockExportView
                .SetupSet(x => x.SchemaConfiguration = It.IsAny<CrmSchemaConfiguration>())
                .Callback<CrmSchemaConfiguration>(x => result = x);

            // Act
            mockExportView.Raise(x => x.SchemaConfigPathChanged += null, EventArgs.Empty);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsEmpty()
        {
            // Arrange
            CrmSchemaConfiguration result = null;
            mockExportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(" ");
            mockExportView
                .SetupSet(x => x.SchemaConfiguration = It.IsAny<CrmSchemaConfiguration>())
                .Callback<CrmSchemaConfiguration>(x => result = x);

            // Act
            mockExportView.Raise(x => x.SchemaConfigPathChanged += null, EventArgs.Empty);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsInvalid()
        {
            // Arrange
            CrmSchemaConfiguration result = null;
            mockExportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns("a-random-non-existent-file");
            mockExportView
                .SetupSet(x => x.SchemaConfiguration = It.IsAny<CrmSchemaConfiguration>())
                .Callback<CrmSchemaConfiguration>(x => result = x);

            // Act
            mockExportView.Raise(x => x.SchemaConfigPathChanged += null, EventArgs.Empty);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnSchemaWhenCrmMigrationToolSchemaPathIsValid()
        {
            // Arrange
            var filePath = @"TestData\BusinessUnitSchema.xml";
            CrmSchemaConfiguration result = null;
            mockExportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(filePath);
            mockExportView
                .SetupSet(x => x.SchemaConfiguration = It.IsAny<CrmSchemaConfiguration>())
                .Callback<CrmSchemaConfiguration>(x => result = x);

            // Act
            mockExportView.Raise(x => x.SchemaConfigPathChanged += null, EventArgs.Empty);

            // Assert
            result.Should().BeEquivalentTo(CrmSchemaConfiguration.ReadFromFile(filePath));
        }

        private void VerifyViewPropertiesSet(CrmExporterConfig exportConfig)
        {
            mockExportView.VerifySet(x => x.PageSize = exportConfig.PageSize, "Page size does not match config");
            mockExportView.VerifySet(x => x.BatchSize = exportConfig.BatchSize, "Batch size does not match config");
            mockExportView.VerifySet(x => x.TopCount = exportConfig.TopCount, "Top count does not match config");
            mockExportView.VerifySet(x => x.CrmMigrationToolSchemaPath = exportConfig.CrmMigrationToolSchemaPaths.FirstOrDefault(), "Schema Path does not match config");
            mockExportView.VerifySet(x => x.OnlyActiveRecords = exportConfig.OnlyActiveRecords, "Only active records does not match config");
            mockExportView.VerifySet(x => x.OneEntityPerBatch = exportConfig.OneEntityPerBatch, "One entity per batch does not match config");
            mockExportView.VerifySet(x => x.SeperateFilesPerEntity = exportConfig.OneEntityPerBatch, "Separate files per entity does not match config");
            mockExportView.VerifySet(x => x.FilePrefix = exportConfig.FilePrefix, "File prefix does not match config");
            mockExportView.VerifySet(x => x.CrmMigrationToolSchemaFilters = exportConfig.CrmMigrationToolSchemaFilters, "CrmMigrationToolSchemaFilters does not match config");
        }

        private void VerifyViewPropertiesNotSet()
        {
            // One time is expected in the constuctor, not after the file is loaded.
            mockExportView.VerifySet(x => x.PageSize = It.IsAny<int>(), Times.Once, "Page size was set unexpectedly");
            mockExportView.VerifySet(x => x.BatchSize = It.IsAny<int>(), Times.Once, "Batch size was set unexpectedly");
            mockExportView.VerifySet(x => x.TopCount = It.IsAny<int>(), Times.Once, "Top count was set unexpectedly");
            mockExportView.VerifySet(x => x.CrmMigrationToolSchemaPath = It.IsAny<string>(), Times.Once, "Schema path was set unexpectedly");
            mockExportView.VerifySet(x => x.OnlyActiveRecords = It.IsAny<bool>(), Times.Once, "Only active records was set unexpectedly");
            mockExportView.VerifySet(x => x.OneEntityPerBatch = It.IsAny<bool>(), Times.Once, "One entity per batch was set unexpectedly");
            mockExportView.VerifySet(x => x.SeperateFilesPerEntity = It.IsAny<bool>(), Times.Once, "Separate files per entity was set unexpectedly");
            mockExportView.VerifySet(x => x.FilePrefix = It.IsAny<string>(), Times.Once, "File prefix was set unexpectedly");
            mockExportView.VerifySet(x => x.CrmMigrationToolSchemaFilters = It.IsAny<Dictionary<string, string>>(), "CrmMigrationToolSchemaFilters was set unexpectedly");
        }
    }
}