﻿using Capgemini.DataMigration.Resiliency.Polly;
using Capgemini.Xrm.DataMigration.Config;
using Capgemini.Xrm.DataMigration.Core;
using Capgemini.Xrm.DataMigration.CrmStore.Config;
using Capgemini.Xrm.DataMigration.Engine;
using Capgemini.Xrm.DataMigration.Repositories;
using Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.UserControls
{
    public partial class importWizard : UserControl
    {
        private CrmImportConfig importConfig;

        public importWizard()
        {
            InitializeComponent();

            importConfig = new CrmImportConfig()
            {
                IgnoreStatuses = cbIgnoreStatuses.Checked,
                IgnoreSystemFields = cbIgnoreSystemFields.Checked,
                SaveBatchSize = Convert.ToInt32(nudSavePageSize.Value),
                JsonFolderPath = string.Empty
            };

            wizardButtons1.OnExecute += button2_Click;
            logger = new MessageLogger(tbLogger, SynchronizationContext.Current);
        }

        public event EventHandler<RequestConnectionEventArgs> OnConnectionRequested;

        public MessageLogger logger { get; }

        public string TargetConnectionString { get; set; }

        public CrmServiceClient CrmServiceClient { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var fd = folderBrowserDialog1.ShowDialog();

            if (fd == DialogResult.OK)
            {
                tbSourceDataLocation.Text = folderBrowserDialog1.SelectedPath;
                importConfig.JsonFolderPath = folderBrowserDialog1.SelectedPath;
                stepWizardControl1.Pages[1].AllowNext = true;
            }
        }

        private void buttonTargetConnectionString_Click(object sender, EventArgs e)
        {
            OnConnectionRequested(this, new RequestConnectionEventArgs { ActionName = "" });
            labelTargetConnectionString.Text = CrmServiceClient.ConnectedOrgFriendlyName;
            stepWizardControl1.Pages[3].AllowNext = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = !radioButton2.Checked;
            groupBox1.Visible = false;
            stepWizardControl1.Pages[0].AllowNext = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            stepWizardControl1.Pages[0].AllowNext = radioButton1.Checked && tbImportSchema.Text != string.Empty;
            radioButton2.Checked = !radioButton1.Checked;
            groupBox1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tokenSource = new CancellationTokenSource();
            tbLogger.Clear();
            Task.Run(() =>
            {
                var orgService = CrmServiceClient.OrganizationWebProxyClient != null ? (IOrganizationService)CrmServiceClient.OrganizationWebProxyClient : (IOrganizationService)CrmServiceClient.OrganizationServiceProxy;

                if (nudMaxThreads.Value > 1 && !string.IsNullOrWhiteSpace(TargetConnectionString))
                {
                    logger.Info("Starting MultiThreaded Processing, using " + nudMaxThreads.Value + " threads");
                    List<IEntityRepository> repos = new List<IEntityRepository>();
                    int threadCount = Convert.ToInt32(nudMaxThreads.Value);

                    while (threadCount > 0)
                    {
                        threadCount--;
                        repos.Add(new EntityRepository(orgService, new ServiceRetryExecutor()));
                    }

                    CrmFileDataImporter fileExporter = new CrmFileDataImporter(logger, repos, importConfig, tokenSource.Token);
                    fileExporter.MigrateData();
                }
                else
                {
                    logger.Info("Starting Single Threaded processing, you must configure connection string for multithreaded processing adn set up max threads to more than 1");
                    EntityRepository entityRepo = new EntityRepository(orgService, new ServiceRetryExecutor());

                    if (radioButton2.Checked)
                    {
                        CrmFileDataImporter fileExporter = new CrmFileDataImporter(logger, entityRepo, importConfig, tokenSource.Token);
                        fileExporter.MigrateData();
                    }
                    else
                    {
                        CrmSchemaConfiguration schema = CrmSchemaConfiguration.ReadFromFile(tbImportSchema.Text);
                        CrmFileDataImporterCsv fileExporter = new CrmFileDataImporterCsv(logger, entityRepo, importConfig, schema, tokenSource.Token);
                        fileExporter.MigrateData();
                    }
                }
            });
        }

        private void btLoadImportConfigFile_Click(object sender, EventArgs e)
        {
            var fd = openFileDialog1.ShowDialog();

            if (fd == DialogResult.OK)
            {
                tbImportConfigFile.Text = openFileDialog1.FileName;
                importConfig = CrmImportConfig.GetConfiguration(openFileDialog1.FileName);

                cbIgnoreSystemFields.Checked = importConfig.IgnoreSystemFields;
                cbIgnoreStatuses.Checked = importConfig.IgnoreStatuses;
                tbSourceDataLocation.Text = importConfig.JsonFolderPath;
                nudSavePageSize.Value = importConfig.SaveBatchSize;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var fd = openFileDialog1.ShowDialog();

            if (fd == DialogResult.OK)
            {
                tbImportSchema.Text = openFileDialog1.FileName;
            }
        }

        private void tbimportSchema_textChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                stepWizardControl1.Pages[0].AllowNext = true;
            }
        }
    }
}