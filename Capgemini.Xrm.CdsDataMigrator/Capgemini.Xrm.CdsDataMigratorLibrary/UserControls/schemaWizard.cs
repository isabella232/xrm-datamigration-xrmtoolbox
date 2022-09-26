﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Controllers;
using Capgemini.Xrm.CdsDataMigratorLibrary.Core;
using Capgemini.Xrm.CdsDataMigratorLibrary.Exceptions;
using Capgemini.Xrm.CdsDataMigratorLibrary.Forms;
using Capgemini.Xrm.CdsDataMigratorLibrary.Models;
using Capgemini.Xrm.CdsDataMigratorLibrary.Services;
using Capgemini.Xrm.DataMigration.Config;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Capgemini.Xrm.CdsDataMigratorLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using System.Threading.Tasks;
using Capgemini.Xrm.CdsDataMigratorLibrary.Extensions;
using Capgemini.Xrm.CdsDataMigratorLibrary.Helpers;

namespace Capgemini.Xrm.DataMigration.XrmToolBoxPlugin
{
    /// <summary>
    /// Implementation of PluginControl.
    /// </summary>
    public partial class SchemaWizard : UserControl
    {
        private readonly CrmSchemaConfiguration crmSchemaConfiguration = new CrmSchemaConfiguration();
        private readonly AttributeTypeMapping attributeMapping = new AttributeTypeMapping();
        private readonly HashSet<string> checkedEntity = new HashSet<string>();
        private readonly HashSet<string> selectedEntity = new HashSet<string>();
        private readonly HashSet<string> checkedRelationship = new HashSet<string>();
        private readonly Dictionary<string, List<Item<EntityReference, EntityReference>>> mapping = new Dictionary<string, List<Item<EntityReference, EntityReference>>>();
        private readonly Dictionary<string, HashSet<string>> entityAttributes = new Dictionary<string, HashSet<string>>();
        private readonly Dictionary<string, HashSet<string>> entityRelationships = new Dictionary<string, HashSet<string>>();
        private readonly Dictionary<string, string> filterQuery = new Dictionary<string, string>();
        private readonly Dictionary<string, Dictionary<string, List<string>>> lookupMaping = new Dictionary<string, Dictionary<string, List<string>>>();
        private readonly Dictionary<string, Dictionary<Guid, Guid>> mapper = new Dictionary<string, Dictionary<Guid, Guid>>();
        private readonly List<EntityMetadata> cachedMetadata = new List<EntityMetadata>();

        private bool workingstate;
        private Panel informationPanel;
        private Guid organisationId = Guid.Empty;
        private string entityLogicalName;

        public SchemaWizard()
        {
            InitializeComponent();
            SetMenuVisibility(WizardMode.All);
        }

        public event EventHandler<RequestConnectionEventArgs> OnConnectionRequested;

        public IOrganizationService OrganizationService { get; set; }

        public IMetadataService MetadataService { get; set; }

        public INotificationService NotificationService { get; set; }

        public IExceptionService ExceptionService { get; set; }

        public Settings Settings { get; set; }

        public void OnConnectionUpdated(Guid ConnectedOrgId, string ConnectedOrgFriendlyName)
        {
            organisationId = ConnectedOrgId;
            toolStripLabelConnection.Text = $"Connected to: {ConnectedOrgFriendlyName}";
            toolStrip2.Enabled = true;
            RefreshEntities(cachedMetadata, workingstate, true);
        }

        public async Task HandleListViewEntitiesSelectedIndexChanged(Dictionary<string, HashSet<string>> inputEntityRelationships, string inputEntityLogicalName, HashSet<string> inputSelectedEntity, ListView.SelectedListViewItemCollection selectedItems, ServiceParameters serviceParameters)
        {
            ListViewItem listViewSelectedItem = selectedItems.Count > 0 ? selectedItems[0] : null;

            await PopulateAttributes(inputEntityLogicalName, listViewSelectedItem, serviceParameters);

            await PopulateRelationship(inputEntityLogicalName, inputEntityRelationships, listViewSelectedItem, serviceParameters);
            AddSelectedEntities(selectedItems.Count, inputEntityLogicalName, inputSelectedEntity);
        }

        public void AddSelectedEntities(int selectedItemsCount, string inputEntityLogicalName, HashSet<string> inputSelectedEntity)
        {
            if (selectedItemsCount > 0 &&
                !(
                    string.IsNullOrEmpty(inputEntityLogicalName) &&
                    inputSelectedEntity.Contains(inputEntityLogicalName)
                  )
                )
            {
                inputSelectedEntity.Add(inputEntityLogicalName);
            }
        }

        public async Task PopulateRelationship(string entityLogicalName, Dictionary<string, HashSet<string>> inputEntityRelationships, ListViewItem listViewSelectedItem, ServiceParameters migratorParameters)
        {
            if (!workingstate)
            {
                lvRelationship.Items.Clear();
                InitFilter(listViewSelectedItem);
                if (listViewSelectedItem != null)
                {
                    Exception error = null;
                    List<ListViewItem> result = null;

                    await Task.Run(() =>
                    {
                        try
                        {
                            result = migratorParameters.PopulateRelationshipAction(entityLogicalName, inputEntityRelationships);
                        }
                        catch (Exception ex)
                        {
                            error = ex;
                        }
                    });

                    var e = new RunWorkerCompletedEventArgs(result, error, false);
                    lvRelationship.OnPopulateCompletedAction(e, NotificationService, this, cbShowSystemAttributes.Checked);
                    ManageWorkingState(false);
                }
            }
        }

        public void LoadSchemaFile(string schemaFilePath, bool working, INotificationService notificationService, Dictionary<string, HashSet<string>> inputEntityAttributes, Dictionary<string, HashSet<string>> inputEntityRelationships)
        {
            if (!string.IsNullOrWhiteSpace(schemaFilePath))
            {
                try
                {
                    var crmSchema = CrmSchemaConfiguration.ReadFromFile(schemaFilePath);
                    crmSchema.Entities?.StoreEntityData(inputEntityAttributes, inputEntityRelationships);
                    ClearAllListViews();
                    PopulateEntities(working);
                }
                catch (Exception ex)
                {
                    notificationService.DisplayFeedback($"Schema File load error, ensure to load correct Schema file, Error: {ex.Message}");
                }
            }
        }

        public void RefreshEntities(List<EntityMetadata> inputCachedMetadata, bool inputWorkingstate, bool isNewConnection = false)
        {
            if (inputCachedMetadata.Count == 0 || isNewConnection)
            {
                ClearMemory();
                PopulateEntities(inputWorkingstate);
            }
        }

        public void ClearMemory()
        {
            ClearInternalMemory();
        }

        public void ManageWorkingState(bool working)
        {
            workingstate = working;
            gbEntities.Enabled = !working;
            gbAttributes.Enabled = !working;
            gbRelationship.Enabled = !working;
            Cursor = working ? Cursors.WaitCursor : Cursors.Default;
        }

        public void InitFilter(ListViewItem entityitem)
        {
            string filter = null;

            if (entityitem != null && entityitem.Tag != null)
            {
                var entity = (EntityMetadata)entityitem.Tag;
                filter = Settings[organisationId.ToString()][entity.LogicalName].Filter;
            }

            tsbtFilters.ForeColor = string.IsNullOrEmpty(filter) ? Color.Black : Color.Blue;
        }

        private void TabStripButtonRetrieveEntitiesClick(object sender, EventArgs e)
        {
            ClearMemory();
            PopulateEntities(workingstate);
        }

        private void ClearInternalMemory()
        {
            checkedEntity.Clear();
            entityAttributes.Clear();
            entityRelationships.Clear();
            mapper.Clear();
            lookupMaping.Clear();
            filterQuery.Clear();
            selectedEntity.Clear();
            checkedRelationship.Clear();
            mapping.Clear();
        }

        private async void ListViewEntitiesSelectedIndexChanged(object sender, EventArgs e)
        {
            var migratorParameters = new ServiceParameters(OrganizationService, MetadataService, NotificationService, ExceptionService);

            var entityitem = lvEntities.SelectedItems.Count > 0 ? lvEntities.SelectedItems[0] : null;

            entityLogicalName = entityitem.GetEntityLogicalName();
            await HandleListViewEntitiesSelectedIndexChanged(entityRelationships, entityLogicalName, selectedEntity, lvEntities.SelectedItems, migratorParameters);
        }

        public async Task PopulateAttributes(string entityLogicalName, ListViewItem listViewSelectedItem, ServiceParameters serviceParameters)
        {
            if (!workingstate)
            {
                lvAttributes.Items.Clear();
                chkAllAttributes.Checked = true;

                InitFilter(listViewSelectedItem);
                if (listViewSelectedItem != null)
                {
                    Exception error = null;
                    List<ListViewItem> result = null;

                    await Task.Run(() =>
                    {
                        try
                        {
                            var unmarkedattributes = Settings[organisationId.ToString()][this.entityLogicalName].UnmarkedAttributes;
                            var attributes = serviceParameters.GetAttributeList(entityLogicalName, cbShowSystemAttributes.Checked);
                            result = attributes.ProcessAllAttributeMetadata(unmarkedattributes, entityLogicalName, entityAttributes);
                        }
                        catch (Exception ex)
                        {
                            error = ex;
                        }
                    });

                    var e = new RunWorkerCompletedEventArgs(result, error, false);
                    lvAttributes.OnPopulateCompletedAction(e, NotificationService, this, cbShowSystemAttributes.Checked);
                    ManageWorkingState(false);
                }
            }
        }

        public void HandleMappingControlItemClick(INotificationService notificationService, string inputEntityLogicalName, bool listViewItemIsSelected, Dictionary<string, List<Item<EntityReference, EntityReference>>> inputMapping, Dictionary<string, Dictionary<Guid, Guid>> inputMapper, Form parentForm)
        {
            if (listViewItemIsSelected)
            {
                if (!string.IsNullOrEmpty(inputEntityLogicalName))
                {
                    if (inputMapping.ContainsKey(inputEntityLogicalName))
                    {
                        MappingIfContainsKey(inputEntityLogicalName, inputMapping, inputMapper, parentForm);
                    }
                    else
                    {
                        MappingIfKeyDoesNotExist(inputEntityLogicalName, inputMapping, inputMapper, parentForm);
                    }
                }
            }
            else
            {
                notificationService.DisplayFeedback("Entity is not selected");
            }
        }

        public void MappingIfKeyDoesNotExist(string inputEntityLogicalName, Dictionary<string, List<Item<EntityReference, EntityReference>>> inputMapping, Dictionary<string, Dictionary<Guid, Guid>> inputMapper, Form parentForm)
        {
            var mappingReference = new List<Item<EntityReference, EntityReference>>();
            using (var mappingDialog = new MappingList(mappingReference)
            {
                StartPosition = FormStartPosition.CenterParent
            })
            {
                if (parentForm != null)
                {
                    mappingDialog.ShowDialog(parentForm);
                }

                var mapList = mappingDialog.GetMappingList(inputEntityLogicalName);
                var guidMapList = mappingDialog.GetGuidMappingList();

                if (mapList.Count > 0)
                {
                    inputMapping.Add(inputEntityLogicalName, mapList);
                    inputMapper.Add(inputEntityLogicalName, guidMapList);
                }
            }
        }

        public void MappingIfContainsKey(string inputEntityLogicalName, Dictionary<string, List<Item<EntityReference, EntityReference>>> inputMapping, Dictionary<string, Dictionary<Guid, Guid>> inputMapper, Form parentForm)
        {
            using (var mappingDialog = new MappingList(inputMapping[inputEntityLogicalName])
            {
                StartPosition = FormStartPosition.CenterParent
            })
            {
                if (parentForm != null)
                {
                    mappingDialog.ShowDialog(parentForm);
                }

                var mapList = mappingDialog.GetMappingList(inputEntityLogicalName);
                var guidMapList = mappingDialog.GetGuidMappingList();

                if (mapList.Count == 0)
                {
                    inputMapping.Remove(inputEntityLogicalName);
                    inputMapper.Remove(inputEntityLogicalName);
                }
                else
                {
                    inputMapping[inputEntityLogicalName] = mapList;
                    inputMapper[inputEntityLogicalName] = guidMapList;
                }
            }
        }

        public void ProcessFilterQuery(INotificationService notificationService, Form parentForm, string inputEntityLogicalName, bool listViewItemIsSelected, Dictionary<string, string> inputFilterQuery, FilterEditor filterDialog)
        {
            if (listViewItemIsSelected)
            {
                if (parentForm != null)
                {
                    filterDialog.ShowDialog(parentForm);
                }

                if (inputFilterQuery.ContainsKey(inputEntityLogicalName))
                {
                    if (string.IsNullOrWhiteSpace(filterDialog.QueryString))
                    {
                        inputFilterQuery.Remove(inputEntityLogicalName);
                    }
                    else
                    {
                        inputFilterQuery[inputEntityLogicalName] = filterDialog.QueryString;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(filterDialog.QueryString))
                    {
                        inputFilterQuery[inputEntityLogicalName] = filterDialog.QueryString;
                    }
                }
            }
            else
            {
                notificationService.DisplayFeedback("Entity list is empty");
            }
        }

        public void StoreAttributeIfRequiresKey(string logicalName, ItemCheckEventArgs e, Dictionary<string, HashSet<string>> inputEntityAttributes, string inputEntityLogicalName)
        {
            var attributeSet = new HashSet<string>();
            if (e.CurrentValue.ToString() != "Checked")
            {
                attributeSet.Add(logicalName);
            }

            inputEntityAttributes.Add(inputEntityLogicalName, attributeSet);
        }

        public void StoreAttriubteIfKeyExists(string logicalName, ItemCheckEventArgs e, Dictionary<string, HashSet<string>> inputEntityAttributes, string inputEntityLogicalName)
        {
            var attributeSet = inputEntityAttributes[inputEntityLogicalName];

            if (e.CurrentValue.ToString() == "Checked")
            {
                if (attributeSet.Contains(logicalName))
                {
                    attributeSet.Remove(logicalName);
                }
            }
            else
            {
                attributeSet.Add(logicalName);
            }
        }

        private void PopulateEntities(bool working)
        {
            if (!working)
            {
                ClearAllListViews();
                ManageWorkingState(true);

                informationPanel = InformationPanel.GetInformationPanel(this, "Loading entities...", 340, 150);

                using (var bwFill = new BackgroundWorker())
                {
                    bwFill.DoWork += (sender, e) =>
                    {
                        var serviceParameters = new ServiceParameters(OrganizationService, MetadataService, NotificationService, ExceptionService);
                        e.Result = serviceParameters.RetrieveSourceEntitiesListToBeDeleted(cbShowSystemAttributes.Checked, cachedMetadata, entityAttributes);
                    };
                    bwFill.RunWorkerCompleted += (sender, e) =>
                    {
                        informationPanel.Dispose();
                        var list = e.Result as List<ListViewItem>;
                        list.PopulateEntitiesListView(e.Error, this, lvEntities, NotificationService);
                        ManageWorkingState(false);
                    };
                    bwFill.RunWorkerAsync();
                }
            }
        }

        private void TabControlSelected(object sender, TabControlEventArgs e)
        {
            toolStrip2.Enabled = true;
            RefreshEntities(cachedMetadata, workingstate);
        }

        private void TabStripButtonMappingsClick(object sender, EventArgs e)
        {
            HandleMappingControlItemClick(NotificationService, entityLogicalName, lvEntities.SelectedItems.Count > 0, mapping, mapper, ParentForm);
        }

        private void TabStripFiltersClick(object sender, EventArgs e)
        {
            var currentFilter = filterQuery.ContainsKey(entityLogicalName) ? filterQuery[entityLogicalName] : null;
            using (var filterDialog = new FilterEditor(currentFilter, FormStartPosition.CenterParent))
            {
                ProcessFilterQuery(NotificationService, ParentForm, entityLogicalName, lvEntities.SelectedItems.Count > 0, filterQuery, filterDialog);
            }
        }

        private void CheckListAllAttributesCheckedChanged(object sender, EventArgs e)
        {
            lvAttributes.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = chkAllAttributes.Checked);
        }

        private void ListViewAttributesColumnClick(object sender, ColumnClickEventArgs e)
        {
            var columnNumber = e.Column;
            if (columnNumber != 3)
            {
                lvAttributes.SetListViewSorting(e.Column, organisationId.ToString(), Settings);
            }
        }

        private void ListViewEntitiesColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvEntities.SetListViewSorting(e.Column, organisationId.ToString(), Settings);
        }

        private void CheckListAllEntitiesCheckedChanged(object sender, EventArgs e)
        {
            lvEntities.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = chkAllEntities.Checked);
        }

        private void ListViewAttributesItemCheck(object sender, ItemCheckEventArgs e)
        {
            var indexNumber = e.Index;
            var logicalName = lvAttributes.Items[indexNumber].SubItems[1].Text;

            if (entityAttributes.ContainsKey(entityLogicalName))
            {
                StoreAttriubteIfKeyExists(logicalName, e, entityAttributes, entityLogicalName);
            }
            else
            {
                StoreAttributeIfRequiresKey(logicalName, e, entityAttributes, entityLogicalName);
            }
        }

        private void ListViewEntitiesItemCheck(object sender, ItemCheckEventArgs e)
        {
            var indexNumber = e.Index;
            var logicalName = lvEntities.Items[indexNumber].SubItems[1].Text;
            if (e.CurrentValue.ToString() == "Checked")
            {
                if (checkedEntity.Contains(logicalName))
                {
                    checkedEntity.Remove(logicalName);
                }
            }
            else
            {
                checkedEntity.Add(logicalName);
            }
        }

        private void TbSaveSchemaClick(object sender, EventArgs e)
        {
            var serviceParameters = new ServiceParameters(OrganizationService, MetadataService, NotificationService, ExceptionService);
            var controller = new SchemaExtension();

            controller.SaveSchema(serviceParameters, checkedEntity, entityRelationships, entityAttributes, attributeMapping, crmSchemaConfiguration, tbSchemaPath.Text);
        }

        private void ButtonSchemaFolderPathClick(object sender, EventArgs e)
        {
            using (var fileDialog = new SaveFileDialog
            {
                Filter = "XML Files|*.xml",
                OverwritePrompt = false
            })
            {
                var dialogResult = fileDialog.ShowDialog();
                var controller = new SchemaExtension();
                var collectionParameters = new CollectionParameters(entityAttributes, entityRelationships, null, null, null, null);

                controller.SchemaFolderPathAction(NotificationService, tbSchemaPath, workingstate, collectionParameters  /*entityAttributes, entityRelationships*/, dialogResult, fileDialog, LoadSchemaFile);
            }
        }

        public void ClearAllListViews()
        {
            lvEntities.Items.Clear();
            lvAttributes.Items.Clear();
            lvRelationship.Items.Clear();
        }

        private void CheckBoxAllRelationshipsCheckedChanged(object sender, EventArgs e)
        {
            lvRelationship.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = chkAllRelationships.Checked);
        }

        private void ListViewRelationshipItemCheck(object sender, ItemCheckEventArgs e)
        {
            var indexNumber = e.Index;
            var logicalName = lvRelationship.Items[indexNumber].SubItems[1].Text;

            if (entityRelationships.ContainsKey(entityLogicalName))
            {
                CollectionHelpers.StoreRelationshipIfKeyExists(logicalName, e, entityLogicalName, entityRelationships);
            }
            else
            {
                CollectionHelpers.StoreRelationshipIfRequiresKey(logicalName, e, entityLogicalName, entityRelationships);
            }
        }

        private void ButtonImportConfigPathClick(object sender, EventArgs e)
        {
            using (var fileDialog = new SaveFileDialog
            {
                Filter = "JSON Files|*.json",
                OverwritePrompt = false
            })
            {
                var result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    tbImportConfig.Text = fileDialog.FileName.ToString(CultureInfo.InvariantCulture);

                    if (File.Exists(tbImportConfig.Text))
                    {
                        var controller = new ConfigurationHelpers();
                        controller.LoadImportConfigFile(NotificationService, tbImportConfig, mapper, mapping);
                    }
                }
            }
        }

        private void ExportConfigPathButtonClick(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog
            {
                Filter = "JSON Files|*.json",
                OverwritePrompt = false
            })
            {
                var result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    tbExportConfig.Text = fileDialog.FileName.ToString(CultureInfo.InvariantCulture);

                    if (File.Exists(tbExportConfig.Text))
                    {
                        var controller = new ConfigurationHelpers();
                        controller.LoadExportConfigFile(NotificationService, tbExportConfig, filterQuery, lookupMaping);
                    }
                }
            }
        }

        private void ToolBarSaveMappingsClick(object sender, EventArgs e)
        {
            var controller = new ConfigurationHelpers();
            controller.GenerateImportConfigFile(NotificationService, tbImportConfig, mapper);
        }

        private void ToolBarSaveFiltersClick(object sender, EventArgs e)
        {
            var controller = new ConfigurationHelpers();
            controller.GenerateExportConfigFile(tbExportConfig, tbSchemaPath, filterQuery, lookupMaping, NotificationService);
        }

        private void ToolBarLoadMappingsFileClick(object sender, EventArgs e)
        {
            var controller = new ConfigurationHelpers();
            controller.LoadImportConfigFile(NotificationService, tbImportConfig, mapper, mapping);
        }

        private void ToolBarLoadSchemaFileClick(object sender, EventArgs e)
        {
            LoadSchemaFile(tbSchemaPath.Text, workingstate, NotificationService, entityAttributes, entityRelationships);
        }

        private void ToolBarLoadFiltersFileClick(object sender, EventArgs e)
        {
            var controller = new ConfigurationHelpers();
            controller.LoadExportConfigFile(NotificationService, tbExportConfig, filterQuery, lookupMaping);
        }

        private void LoadAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            var collectionParameters = new CollectionParameters(entityAttributes, entityRelationships,
              filterQuery, lookupMaping, mapper, mapping);

            LoadAllFiles(NotificationService, tbSchemaPath, tbExportConfig, tbImportConfig, workingstate, collectionParameters);
        }

        public void LoadAllFiles(INotificationService feedbackManager, TextBox schemaPath, TextBox exportConfig, TextBox importConfig, bool inputWorkingstate, CollectionParameters collectionParameters)
        {
            LoadSchemaFile(schemaPath.Text, inputWorkingstate, feedbackManager, collectionParameters.EntityAttributes, collectionParameters.EntityRelationships);

            var controller = new ConfigurationHelpers();
            controller.LoadExportConfigFile(feedbackManager, exportConfig, collectionParameters.FilterQuery, collectionParameters.LookupMaping);
            controller.LoadImportConfigFile(feedbackManager, importConfig, collectionParameters.Mapper, collectionParameters.Mapping);
        }

        private void SaveAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            var controller = new ConfigurationHelpers();
            controller.GenerateImportConfigFile(NotificationService, tbImportConfig, mapper);
            controller.GenerateExportConfigFile(tbExportConfig, tbSchemaPath, filterQuery, lookupMaping, NotificationService);

            var serviceParameters = new ServiceParameters(OrganizationService, MetadataService, NotificationService, ExceptionService);
            var metadataExtensionBase = new SchemaExtension();
            metadataExtensionBase.CollectCrmEntityFields(checkedEntity, crmSchemaConfiguration, entityRelationships, entityAttributes, attributeMapping, serviceParameters);

            var schemaController = new SchemaExtension();
            schemaController.GenerateXmlFile(tbSchemaPath.Text, crmSchemaConfiguration);
            crmSchemaConfiguration.Entities.Clear();
        }

        private void ToolStripButton1Click(object sender, EventArgs e)
        {
            var serviceParameters = new ServiceParameters(OrganizationService, MetadataService, NotificationService, ExceptionService);

            serviceParameters.OpenMappingForm(ParentForm, cachedMetadata, lookupMaping, entityLogicalName);

            tsbtMappings.ForeColor = Settings[organisationId.ToString()].Mappings.Count == 0 ? Color.Black : Color.Blue;
            Settings[organisationId.ToString()].Mappings.Clear();
        }

        protected void RadioButton1CheckedChanged(object sender, EventArgs e)
        {
            SetMenuVisibility(WizardMode.Schema);
        }

        protected void RadioButton2CheckedChanged(object sender, EventArgs e)
        {
            SetMenuVisibility(WizardMode.Export);
        }

        protected void RadioButton3CheckedChanged(object sender, EventArgs e)
        {
            SetMenuVisibility(WizardMode.Import);
        }

        protected void RadioButton4CheckedChanged(object sender, EventArgs e)
        {
            SetMenuVisibility(WizardMode.All);
        }

        private void SetMenuVisibility(WizardMode mode)
        {
            SetImportMenu(mode, tsbtMappings, loadMappingsToolStripMenuItem, saveMappingsToolStripMenuItem, tbImportConfig, btImportConfigPath);
            SetExportMenu(mode, lookupMappings, tsbtFilters, loadFiltersToolStripMenuItem, saveFiltersToolStripMenuItem, tbExportConfig, btExportConfigPath);
            SetSchemaMenu(mode, loadSchemaToolStripMenuItem, saveSchemaToolStripMenuItem, tbSchemaPath, btSchemaFolderPath);
            SetAllMenu(mode, loadAllToolStripMenuItem, saveAllToolStripMenuItem);
        }

        public void SetImportMenu(WizardMode mode, System.Windows.Forms.ToolStripButton mappingsToolStripButton, System.Windows.Forms.ToolStripMenuItem inputLoadMappingsToolStripMenuItem, System.Windows.Forms.ToolStripMenuItem inputSaveMappingsToolStripMenuItem, System.Windows.Forms.TextBox importConfigTextBox, System.Windows.Forms.Button importConfigPathButton)
        {
            mappingsToolStripButton.Enabled = mode == WizardMode.All || mode == WizardMode.Import;
            inputLoadMappingsToolStripMenuItem.Enabled = mode == WizardMode.All || mode == WizardMode.Import;
            inputSaveMappingsToolStripMenuItem.Enabled = mode == WizardMode.All || mode == WizardMode.Import;
            importConfigTextBox.Enabled = mode == WizardMode.All || mode == WizardMode.Import;
            importConfigPathButton.Enabled = mode == WizardMode.All || mode == WizardMode.Import;
        }

        public void SetExportMenu(WizardMode mode, ToolStripButton inputLookupMappings, ToolStripButton filtersToolStripButton, ToolStripMenuItem inputLoadFiltersToolStripMenuItem, ToolStripMenuItem inputSaveFiltersToolStripMenuItem, TextBox exportConfigTextBox, Button exportConfigPathButton)
        {
            inputLookupMappings.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
            filtersToolStripButton.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
            inputLoadFiltersToolStripMenuItem.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
            inputSaveFiltersToolStripMenuItem.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
            exportConfigTextBox.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
            exportConfigPathButton.Enabled = mode == WizardMode.Export || mode == WizardMode.All;
        }

        public void SetSchemaMenu(WizardMode mode, ToolStripMenuItem inputLoadSchemaToolStripMenuItem, ToolStripMenuItem inputSaveSchemaToolStripMenuItem, TextBox schemaPathTextBox, Button schemaFolderPathButton)
        {
            inputLoadSchemaToolStripMenuItem.Enabled = mode == WizardMode.Schema || mode == WizardMode.All;
            inputSaveSchemaToolStripMenuItem.Enabled = mode == WizardMode.Schema || mode == WizardMode.All;
            schemaPathTextBox.Enabled = mode == WizardMode.Schema || mode == WizardMode.All;
            schemaFolderPathButton.Enabled = mode == WizardMode.Schema || mode == WizardMode.All;
        }

        public void SetAllMenu(WizardMode mode, ToolStripMenuItem inputLoadAllToolStripMenuItem, ToolStripMenuItem inputSaveAllToolStripMenuItem)
        {
            inputLoadAllToolStripMenuItem.Enabled = mode == WizardMode.All;
            inputSaveAllToolStripMenuItem.Enabled = mode == WizardMode.All;
        }

        private void ToolStripButtonConnectClick(object sender, EventArgs e)
        {
            if (OnConnectionRequested != null)
            {
                var args = new RequestConnectionEventArgs { ActionName = "SchemaConnection", Control = (CdsMigratorPluginControl)Parent };
                OnConnectionRequested(this, args);
            }
        }
    }
}