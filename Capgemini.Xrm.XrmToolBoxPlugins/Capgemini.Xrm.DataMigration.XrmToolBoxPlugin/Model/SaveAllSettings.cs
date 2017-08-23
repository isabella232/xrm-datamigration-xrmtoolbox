﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.Model
{
    class SaveAllSettings
    {
        #region Public Properties
        public string SchemaFilePath { get; set; }
        public string ImportFilePath { get; set; }
        public string ExportFilePath { get; set; }
        public string FailedValidationMessage { get; set; }
        public bool FailedValidation { get; set; }
        public string SuccessValidationMessage { get; set; }
        #endregion

        #region Public Methods 
        public void Validate()
        {
            var stringBuilder = new StringBuilder();
            FailedValidationMessage = "";
            FailedValidation = false;
            if (string.IsNullOrEmpty(SchemaFilePath) || SchemaFilePath == null)
            {
                FailedValidation = true;
                stringBuilder.AppendLine("Select schema file path");
            }
            if (string.IsNullOrEmpty(ImportFilePath) || ImportFilePath == null)
            {
                FailedValidation = true;
                stringBuilder.AppendLine("Select import config file path");
            }
            if (string.IsNullOrEmpty(ExportFilePath) || ExportFilePath == null)
            {
                FailedValidation = true;
                stringBuilder.AppendLine("Select export config file path");
            }
            FailedValidationMessage = stringBuilder.ToString();
        }
        #endregion
    }
}
