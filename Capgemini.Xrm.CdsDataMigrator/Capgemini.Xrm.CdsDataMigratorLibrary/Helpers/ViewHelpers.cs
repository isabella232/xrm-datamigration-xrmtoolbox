﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace Capgemini.Xrm.CdsDataMigratorLibrary.Helpers
{
    public class ViewHelpers : IViewHelpers
    {
        public bool AreAllCellsPopulated(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (string.IsNullOrEmpty(cell.Value as string))
                {
                    return false;
                }
            }
            return true;
        }

        public List<DataGridViewRow> GetMappingsFromViewWithEmptyRowsRemoved(List<DataGridViewRow> viewLookupMappings)
        {
            var filteredViewLookupMappings = new List<DataGridViewRow>();
            foreach (DataGridViewRow viewLookupRow in viewLookupMappings)
            {
                if (AreAllCellsPopulated(viewLookupRow))
                {
                    filteredViewLookupMappings.Add(viewLookupRow);
                }
            }
            return filteredViewLookupMappings;
        }

        [ExcludeFromCodeCoverage]
        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }
    }
}
