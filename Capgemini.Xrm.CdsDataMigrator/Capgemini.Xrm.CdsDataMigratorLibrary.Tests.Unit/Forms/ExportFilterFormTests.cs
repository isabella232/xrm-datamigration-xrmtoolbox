﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Forms;
using Capgemini.Xrm.CdsDataMigratorLibrary.Models;
using Capgemini.Xrm.CdsDataMigratorLibrary.Presenters;
using Capgemini.Xrm.DataMigration.Config;
using Capgemini.Xrm.DataMigration.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Capgemini.Xrm.CdsDataMigratorLibrary.Tests.Unit.Forms
{
    [TestClass]
    public class ExportFilterFormTests
    {
        [TestMethod]
        public void EntityList_GetSet()
        {
            // Arrange
            var value = new List<ListBoxItem<CrmEntity>>();
            using (var systemUnderTest = new ExportFilterForm())
            {
                // Act
                systemUnderTest.As<IExportFilterFormView>().EntityList = value;

                // Assert
                systemUnderTest.As<IExportFilterFormView>().EntityList.Should().BeEquivalentTo(value);
            }
        }

        [TestMethod]
        public void SelectedEntity_GetSet()
        {
            // Arrange
            var value = new CrmEntity();
            using (var systemUnderTest = new ExportFilterForm())
            {
                systemUnderTest.As<IExportFilterFormView>().EntityList = new List<ListBoxItem<CrmEntity>>
                {
                    new ListBoxItem<CrmEntity> { DisplayName = "Entity", Item = value }
                };

                // Act
                systemUnderTest.As<IExportFilterFormView>().SelectedEntity = value;

                // Assert
                systemUnderTest.As<IExportFilterFormView>().SelectedEntity.Should().Be(value);
            }
        }

        [TestMethod]
        public void SelectedEntity_ShouldNotifyPresenterWhenUpdated()
        {
            // Arrange
            var value = new CrmEntity();
            using (var systemUnderTest = new ExportFilterForm())
            {
                var isCalled = false;
                systemUnderTest.OnEntitySelected += (object sender, EventArgs e) => isCalled = true;

                systemUnderTest.As<IExportFilterFormView>().EntityList = new List<ListBoxItem<CrmEntity>>
                {
                    new ListBoxItem<CrmEntity> { DisplayName = "Entity", Item = value }
                };

                // Act
                systemUnderTest.As<IExportFilterFormView>().SelectedEntity = value;

                // Assert
                isCalled.Should().BeTrue();
            }
        }

        [TestMethod]
        public void EntityFilters_GetSet()
        {
            // Arrange
            var value = new Dictionary<string, string>();
            using (var systemUnderTest = new ExportFilterForm())
            {
                // Act
                systemUnderTest.EntityFilters = value;

                // Assert
                systemUnderTest.As<IExportFilterFormView>().EntityFilters.Should().BeEquivalentTo(value);
            }
        }

        [TestMethod]
        public void SchemaConfiguration_GetSet()
        {
            // Arrange
            var value = new CrmSchemaConfiguration();
            using (var systemUnderTest = new ExportFilterForm())
            {
                // Act
                systemUnderTest.SchemaConfiguration = value;

                // Assert
                systemUnderTest.As<IExportFilterFormView>().SchemaConfiguration.Should().Be(value);
            }
        }

        [TestMethod]
        public void FilterText_GetSet()
        {
            // Arrange
            var value = "some text";
            using (var systemUnderTest = new ExportFilterForm())
            {
                // Act
                systemUnderTest.As<IExportFilterFormView>().FilterText = value;

                // Assert
                systemUnderTest.As<IExportFilterFormView>().FilterText.Should().Be(value);
            }
        }

        [TestMethod]
        public void FilterText_ShouldNotifyPresenterWhenSet()
        {
            // Arrange
            var value = "some text";
            using (var systemUnderTest = new ExportFilterForm())
            {
                var isCalled = false;
                systemUnderTest.OnFilterTextChanged += (object sender, EventArgs e) => isCalled = true;

                // Act
                systemUnderTest.As<IExportFilterFormView>().FilterText = value;

                // Assert
                isCalled.Should().BeTrue();
            }
        }

        [TestMethod]
        public void OnVisibleChanged_ShouldNotifyPresenterWhenTrue()
        {
            // Arrange
            using (var systemUnderTest = new ExportFilterForm())
            {
                var isCalled = false;
                systemUnderTest.OnVisible += (object sender, EventArgs e) => isCalled = true;

                // Act
                systemUnderTest.Visible = true;

                // Assert
                isCalled.Should().BeTrue();
            }
        }

        [TestMethod]
        public void OnVisibleChanged_ShouldNotNotifyPresenterWhenFalse()
        {
            // Arrange
            using (var systemUnderTest = new ExportFilterForm())
            {
                var isCalled = false;
                systemUnderTest.OnVisible += (object sender, EventArgs e) => isCalled = true;
                // Act
                systemUnderTest.Visible = false;

                // Assert
                isCalled.Should().BeFalse();
            }
        }
    }
}
