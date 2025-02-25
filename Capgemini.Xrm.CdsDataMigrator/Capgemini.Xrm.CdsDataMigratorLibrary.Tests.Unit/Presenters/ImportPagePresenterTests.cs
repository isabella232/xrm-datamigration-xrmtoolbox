﻿using Capgemini.Xrm.CdsDataMigrator.Tests.Unit;
using Capgemini.Xrm.CdsDataMigratorLibrary.Enums;
using Capgemini.Xrm.CdsDataMigratorLibrary.Helpers;
using Capgemini.Xrm.CdsDataMigratorLibrary.Presenters;
using Capgemini.Xrm.CdsDataMigratorLibrary.Tests.Unit.Extensions;
using Capgemini.Xrm.DataMigration.Config;
using Capgemini.Xrm.DataMigration.CrmStore.Config;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Capgemini.Xrm.CdsDataMigratorLibrary.Tests.Unit.Presenters
{
    [TestClass]
    public class ImportPresenterTests : TestBase
    {
        private Mock<IImportPageView> mockImportView;
        private Mock<IWorkerHost> mockWorkerHost;
        private Mock<IViewHelpers> mockViewHelpers;
        private ImportPagePresenter systemUnderTest;

        [TestInitialize]
        public void TestSetup()
        {
            SetupServiceMocks();
            mockImportView = new Mock<IImportPageView>();
            mockWorkerHost = new Mock<IWorkerHost>();
            mockViewHelpers = new Mock<IViewHelpers>();

            systemUnderTest = new ImportPagePresenter(mockImportView.Object, mockWorkerHost.Object, DataMigrationServiceMock.Object, ServiceMock.Object, MetadataServiceMock.Object, mockViewHelpers.Object, EntityRepositoryServiceMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldSetDefaultConfigProperties()
        {
            // Arrange
            var importConfig = new CrmImportConfig();

            // Assert
            VerifyViewPropertiesSet(importConfig);
        }

        [TestMethod]
        public void LoadConfig_ShouldSetConfigPropertiesWhenValidFilePathSelected()
        {
            // Arrange
            var viewMappings = GetMappingsAsViewTypeToMatchConfigFile();
            var importConfigFilePath = @"TestData\ImportConfig.json";
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            List<DataGridViewRow> mappingsAsViewType = ProvideMappingsAsViewType();
            var entityMetaDataList = new List<EntityMetadata>()
                {
                    new EntityMetadata { LogicalName = "account" }
                };
            mockViewHelpers
                .Setup(x => x.AreAllCellsPopulated(new DataGridViewRow()))
                .Returns(true);
            mockViewHelpers
                .Setup(x => x.GetMappingsFromViewWithEmptyRowsRemoved(new List<DataGridViewRow>()))
                .Returns(mappingsAsViewType);
            mockImportView
                .Setup(x => x.Mappings)
                .Returns(mappingsAsViewType);
            MetadataServiceMock.Setup(x => x.RetrieveEntities(It.IsAny<IOrganizationService>()))
                .Returns(entityMetaDataList)
                .Verifiable();
            mockImportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            VerifyViewPropertiesSet(importConfig);
        }

        [TestMethod]
        public void LoadConfig_ShouldDoNothingWhenInvalidFilePathSelected()
        {
            // Arrange
            mockImportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns("$a-random-non-existent-file$");

            // Act
            mockImportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            VerifyViewPropertiesNotSet();
        }

        [TestMethod]
        public void LoadConfig_ShouldDoNothingWhenEmptyFilePathSelected()
        {
            // Arrange
            mockImportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns("");

            // Act
            mockImportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            VerifyViewPropertiesNotSet();
        }

        [TestMethod]
        public void LoadConfig_ShouldNotifyExceptionWhenAnExceptionIsThrown()
        {
            // Arrange
            var thrownException = new Exception("Test exception");
            mockImportView
                .Setup(x => x.AskForFilePathToOpen())
                .Throws(thrownException);

            // Act
            mockImportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            mockViewHelpers.Verify(x => x.ShowMessage(
                "Test exception", "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error), Times.Once);
        }

        [TestMethod]
        public void SaveConfig_ShouldUpdateOrCreateConfigFileWhenValidFilePathSelected()
        {
            // Arrange
            var importConfigFilePath = @"TestData\NewImportConfig.json";
            var viewMappings = ProvideMappingsAsViewType();
            var configMappings = ProvideMappingsAsConfigType();
            mockViewHelpers.Setup(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Verifiable();

            mockImportView.SetupGet(x => x.SaveBatchSize).Returns(1000);
            mockImportView.SetupGet(x => x.IgnoreStatuses).Returns(true);
            mockImportView.SetupGet(x => x.IgnoreSystemFields).Returns(true);
            mockImportView.SetupGet(x => x.JsonFolderPath).Returns(@"C:\\Some\Path\To\A\Folder");
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            importConfig.SaveBatchSize.Should().Be(1000);
            importConfig.IgnoreStatuses.Should().Be(true);
            importConfig.IgnoreSystemFields.Should().Be(true);
            importConfig.JsonFolderPath.Should().Be(@"C:\\Some\Path\To\A\Folder");
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        public void SaveConfig_ShouldNotIncludeRowWithEmptyCellIntheMappings()
        {
            // Arrange
            var importConfigFilePath = @"TestData\NewImportConfig.json";
            var viewMappings = ProvideMappingsAsViewType();
            var newRow = GetRowWithBlankCell();
            viewMappings.Add(newRow);
            var configMappings = ProvideMappingsAsConfigType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockViewHelpers.SetupSequence(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Returns(false);
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        public void SaveConfig_ShouldNotIncludeRowWithDefaultIdsIntheMappings()
        {
            // Arrange
            var importConfigFilePath = @"TestData\NewImportConfig.json";
            var viewMappings = ProvideMappingsAsViewType();
            var newRow = GetRowWithDefaultIds();
            viewMappings.Add(newRow);
            var configMappings = ProvideMappingsAsConfigType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockViewHelpers.Setup(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Verifiable();
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        public void SaveConfig_ShouldCorrectlyAddNewMappingWhenExistingMappingAlreadyExistsForEntity()
        {
            // Arrange
            var importConfigFilePath = @"TestData\NewImportConfig.json";
            var viewMappings = ProvideMappingsAsViewType();
            var newRow = GetRowWithAccountEntityAndValidGuids();
            viewMappings.Add(newRow);
            var configMappings = ProvideTwoMappingsForSameEntityAsConfigType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockViewHelpers.Setup(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Verifiable();
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        [Ignore("What is an invalid file?")]
        // To do: add UI logic that prevents invalid file from being saved
        public void SaveConfig_ShouldDoNothingWhenInvalidFilePathSelected()
        {
            // Arrange
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns("a-random-non-existent-file");

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            //Assert
            mockImportView.VerifyAll();
        }

        [TestMethod]
        public void SaveConfig_ShouldDoNothingWhenEmptyFilePathSelected()
        {
            // Arrange
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Returns("");

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
        }

        [TestMethod]
        public void SaveConfig_ShouldReuseLoadedFilePath()
        {
            // Arrange
            var importConfigFilePath = @"TestData\NewImportConfig.json";
            var importConfig = CrmImportConfig.GetConfiguration(importConfigFilePath);
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView
                .Setup(x => x.AskForFilePathToOpen())
                .Returns(importConfigFilePath);

            // Act
            mockImportView.Raise(x => x.LoadConfigClicked += null, EventArgs.Empty);
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.Verify(x => x.AskForFilePathToSave(importConfigFilePath));
        }

        [TestMethod]
        public void SaveConfig_ShouldNotifyExceptionWhenAnExceptionIsThrown()
        {
            // Arrange
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            var thrownException = new Exception("Test exception");
            mockImportView
                .Setup(x => x.AskForFilePathToSave(null))
                .Throws(thrownException);

            // Act
            mockImportView.Raise(x => x.SaveConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            mockViewHelpers.Verify(x => x.ShowMessage(
                "Test exception", "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error), Times.Once);
        }

        [TestMethod]
        public void RunConfig_ShouldReadValuesFromView()
        {
            // Arrange
            var mockIOrganisationService = new Mock<IOrganizationService>();
            var viewMappings = ProvideMappingsAsViewType();
            var configMappings = ProvideMappingsAsConfigType();
            mockViewHelpers.Setup(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Verifiable();

            mockImportView.SetupGet(x => x.SaveBatchSize).Returns(1000);
            mockImportView.SetupGet(x => x.IgnoreStatuses).Returns(true);
            mockImportView.SetupGet(x => x.IgnoreSystemFields).Returns(true);
            mockImportView.SetupGet(x => x.JsonFolderPath).Returns(@"C:\\Some\Path\To\A\Folder");
            mockImportView.SetupGet(x => x.DataFormat).Returns(Enums.DataFormat.Json);
            mockImportView.SetupGet(x => x.Service).Returns(mockIOrganisationService.Object);
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView.SetupGet(x => x.MaxThreads).Returns(1);

            // Act
            mockImportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);

            var workInfo = mockWorkerHost.Invocations[0].Arguments[0].As<WorkAsyncInfo>();
            workInfo.Work(null, null);

            // Assert
            mockImportView.VerifyAll();
            workInfo.Message.Should().Be("Importing data...");
            DataMigrationServiceMock.Verify(x => x.ImportData(mockIOrganisationService.Object, Enums.DataFormat.Json, It.IsAny<CrmSchemaConfiguration>(), It.IsAny<CrmImportConfig>(), 1, EntityRepositoryServiceMock.Object));

            var importConfig = DataMigrationServiceMock.Invocations[0].Arguments[3].As<CrmImportConfig>();

            importConfig.SaveBatchSize.Should().Be(1000);
            importConfig.IgnoreStatuses.Should().Be(true);
            importConfig.IgnoreSystemFields.Should().Be(true);
            importConfig.JsonFolderPath.Should().Be(@"C:\\Some\Path\To\A\Folder");
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        public void RunConfig_ShouldImport()
        {
            // Arrange
            var mockIOrganisationService = new Mock<IOrganizationService>();
            var viewMappings = ProvideMappingsAsViewType();
            var configMappings = ProvideMappingsAsConfigType();
            mockViewHelpers.Setup(x => x.AreAllCellsPopulated(It.IsAny<DataGridViewRow>()))
                .Returns(true)
                .Verifiable();

            mockImportView.SetupGet(x => x.SaveBatchSize).Returns(1000);
            mockImportView.SetupGet(x => x.IgnoreStatuses).Returns(true);
            mockImportView.SetupGet(x => x.IgnoreSystemFields).Returns(true);
            mockImportView.SetupGet(x => x.JsonFolderPath).Returns(@"C:\\Some\Path\To\A\Folder");
            mockImportView.SetupGet(x => x.DataFormat).Returns(Enums.DataFormat.Json);
            mockImportView.SetupGet(x => x.Service).Returns(mockIOrganisationService.Object);
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView.SetupGet(x => x.MaxThreads).Returns(1);

            // Act
            mockImportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);

            var workInfo = mockWorkerHost.Invocations[0].Arguments[0].As<WorkAsyncInfo>();
            workInfo.Work(null, null);

            // Assert
            mockImportView.VerifyAll();
            workInfo.Message.Should().Be("Importing data...");
            DataMigrationServiceMock.Verify(x => x.ImportData(mockIOrganisationService.Object, Enums.DataFormat.Json, It.IsAny<CrmSchemaConfiguration>(), It.IsAny<CrmImportConfig>(), 1, EntityRepositoryServiceMock.Object));

            var importConfig = DataMigrationServiceMock.Invocations[0].Arguments[3].As<CrmImportConfig>();

            importConfig.SaveBatchSize.Should().Be(1000);
            importConfig.IgnoreStatuses.Should().Be(true);
            importConfig.IgnoreSystemFields.Should().Be(true);
            importConfig.JsonFolderPath.Should().Be(@"C:\\Some\Path\To\A\Folder");
            importConfig.MigrationConfig.Should().BeEquivalentTo(configMappings);
        }

        [TestMethod]
        public void RunConfig_ShouldNotifyExceptionWhenAnExceptionIsThrownOutsideWorkerHost()
        {
            // Arrange
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            var thrownException = new Exception("Test exception");
            mockWorkerHost
                .Setup(x => x.WorkAsync(It.IsAny<WorkAsyncInfo>()))
                .Throws(thrownException);

            // Act
            mockImportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);

            // Assert
            mockImportView.VerifyAll();
            mockViewHelpers.Verify(x => x.ShowMessage(
                "Test exception", "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error), Times.Once);
        }

        [TestMethod]
        public void RunConfig_ShouldNotifyExceptionWhenAnExceptionIsThrownInsideWorkerHost()
        {
            // Arrange
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);
            mockImportView.SetupGet(x => x.MaxThreads).Returns(2);
            var thrownException = new Exception("Test exception");
            DataMigrationServiceMock
                .Setup(x => x.ImportData(It.IsAny<IOrganizationService>(), It.IsAny<DataFormat>(), It.IsAny<CrmSchemaConfiguration>(), It.IsAny<CrmImportConfig>(), 2, EntityRepositoryServiceMock.Object))
                .Throws(thrownException);

            // Act
            mockImportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);
            mockWorkerHost.ExecuteWork(0);

            // Assert
            mockImportView.VerifyAll();
            mockViewHelpers.Verify(x => x.ShowMessage(
                "Test exception", "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error), Times.Once);
        }

        [TestMethod]
        public void RunConfig_ShouldNotifySuccessWhenNotExceptionIsThrownInsideWorkerHost()
        {
            // Arrange
            var viewMappings = ProvideMappingsAsViewType();
            mockImportView.SetupGet(x => x.Mappings).Returns(viewMappings);

            // Act
            mockImportView.Raise(x => x.RunConfigClicked += null, EventArgs.Empty);
            mockWorkerHost.ExecuteWork(0);

            // Assert
            mockImportView.VerifyAll();
            mockViewHelpers.Verify(x => x.ShowMessage(
                "Data import is complete.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information), Times.Once);
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsNull()
        {
            // Arrange
            mockImportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(() => null);

            // Act
            var result = systemUnderTest.GetSchemaConfiguration();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsEmpty()
        {
            // Arrange
            mockImportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(" ");

            // Act
            var result = systemUnderTest.GetSchemaConfiguration();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnNullWhenCrmMigrationToolSchemaPathIsInvalid()
        {
            // Arrange
            mockImportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns("a-random-non-existent-file");

            // Act
            var result = systemUnderTest.GetSchemaConfiguration();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetSchemaConfiguration_ShouldReturnSchemaWhenCrmMigrationToolSchemaPathIsValid()
        {
            // Arrange
            var filePath = @"TestData\BusinessUnitSchema.xml";
            mockImportView
                .SetupGet(x => x.CrmMigrationToolSchemaPath)
                .Returns(filePath);

            // Act
            var result = systemUnderTest.GetSchemaConfiguration();

            // Assert
            result.Should().BeEquivalentTo(CrmSchemaConfiguration.ReadFromFile(filePath));
        }


        private void VerifyViewPropertiesSet(CrmImportConfig ImportConfig)
        {
            mockImportView.VerifySet(x => x.SaveBatchSize = ImportConfig.SaveBatchSize, "Batch Size does not match config");
            mockImportView.VerifySet(x => x.IgnoreStatuses = ImportConfig.IgnoreStatuses, "IgnoreStatuses does not match config");
            mockImportView.VerifySet(x => x.IgnoreSystemFields = ImportConfig.IgnoreSystemFields, "IgnoreSystemFields does not match config");
            mockImportView.VerifySet(x => x.JsonFolderPath = ImportConfig.JsonFolderPath, "JsonFolderPath does not match config");
        }

        private void VerifyViewPropertiesNotSet()
        {
            // One time is expected in the constuctor, not after the file is loaded.
            mockImportView.VerifySet(x => x.SaveBatchSize = It.IsAny<int>(), Times.Once, "Page size was set unexpectedly");
            mockImportView.VerifySet(x => x.IgnoreStatuses = It.IsAny<bool>(), Times.Once, "IgnoreStatusese was set unexpectedly");
            mockImportView.VerifySet(x => x.IgnoreSystemFields = It.IsAny<bool>(), Times.Once, "IgnoreSystemFields was set unexpectedly");
            mockImportView.VerifySet(x => x.JsonFolderPath = It.IsAny<string>(), Times.Once, "JsonFolderPathh was set unexpectedly");
        }

        private static List<DataGridViewRow> ProvideMappingsAsViewType()
        {
            List<DataGridViewRow> mappings = new List<DataGridViewRow>();
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000001" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000002" });
            mappings.Add(dataGridViewRow);

            return mappings;
        }

        private static MappingConfiguration ProvideMappingsAsConfigType()
        {
            var importConfig = new CrmImportConfig();
            Dictionary<string, Dictionary<Guid, Guid>> mappings = new Dictionary<string, Dictionary<Guid, Guid>>();
            var guidsDictionary = new Dictionary<Guid, Guid>();
            var entity = "Account";
            var sourceId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var targetId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            guidsDictionary.Add(sourceId, targetId);
            mappings.Add(entity, guidsDictionary);
            importConfig.MigrationConfig = new MappingConfiguration();
            importConfig.MigrationConfig.Mappings.AddRange(mappings);
            return importConfig.MigrationConfig;
        }

        private static MappingConfiguration ProvideTwoMappingsForSameEntityAsConfigType()
        {
            var importConfig = new CrmImportConfig();
            Dictionary<string, Dictionary<Guid, Guid>> mappings = new Dictionary<string, Dictionary<Guid, Guid>>();
            var guidsDictionary = new Dictionary<Guid, Guid>();
            var entity = "Account";
            var sourceId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var targetId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            guidsDictionary.Add(sourceId, targetId);
            mappings.Add(entity, guidsDictionary);
            mappings[entity].Add(Guid.Parse("00000000-0000-0000-0000-000000000003"), Guid.Parse("00000000-0000-0000-0000-000000000004"));
            importConfig.MigrationConfig = new MappingConfiguration();
            importConfig.MigrationConfig.Mappings.AddRange(mappings);
            return importConfig.MigrationConfig;
        }

        private static DataGridViewRow GetRowWithBlankCell()
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000002" });
            return dataGridViewRow;
        }

        private static DataGridViewRow GetRowWithDefaultIds()
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000000" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000000" });
            return dataGridViewRow;
        }

        private static DataGridViewRow GetRowWithAccountEntityAndValidGuids()
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000003" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000004" });
            return dataGridViewRow;
        }

        private static List<DataGridViewRow> GetMappingsAsViewTypeToMatchConfigFile()
        {
            List<DataGridViewRow> mappings = new List<DataGridViewRow>();
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000003" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000004" });
            DataGridViewRow dataGridViewRow2 = new DataGridViewRow();
            dataGridViewRow2.Cells.Add(new DataGridViewTextBoxCell { Value = "App Action" });
            dataGridViewRow2.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000004" });
            dataGridViewRow2.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000005" });
            DataGridViewRow dataGridViewRow3 = new DataGridViewRow();
            dataGridViewRow3.Cells.Add(new DataGridViewTextBoxCell { Value = "AAD User" });
            dataGridViewRow3.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000006" });
            dataGridViewRow3.Cells.Add(new DataGridViewTextBoxCell { Value = "00000000-0000-0000-0000-000000000007" });
            mappings.Add(dataGridViewRow);
            mappings.Add(dataGridViewRow2);
            mappings.Add(dataGridViewRow3);

            return mappings;
        }
    }
}