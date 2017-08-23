﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Capgemini.Xrm.DataMigration.XrmToolBoxPlugin
{
    [Export(typeof(IXrmToolBoxPlugin)),
    ExportMetadata("BackgroundColor", "Gray"),
    ExportMetadata("PrimaryFontColor", "White"),
    ExportMetadata("SecondaryFontColor", "LightGray"),
    ExportMetadata("SmallImageBase64", "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"),
    ExportMetadata("BigImageBase64", "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"),
    ExportMetadata("Name", "Capgemini Data Migrator"),
    ExportMetadata("Description", "Capgemini plugin for XRMToolbox to export/import CRM data, it can handle mass data thanks to batch processing and pagination. You can use SDK configuration migration tool schema or foder with custom fetchXML files")]
    public class Plugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new PluginControl();
        }
    }
}
