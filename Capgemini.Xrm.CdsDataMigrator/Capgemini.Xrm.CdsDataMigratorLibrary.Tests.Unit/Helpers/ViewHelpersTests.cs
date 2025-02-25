﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Capgemini.Xrm.DataMigration.XrmToolBox.Helpers.Tests
{
    [TestClass]
    public class ViewHelpersTests
    {

        private IViewHelpers systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            systemUnderTest = new ViewHelpers();
        }

        [TestMethod]
        public void AreAllCellsPopulated_RowWithoutEmptyCellShouldReturnTrue()
        {
            var rowWithBlankCell = GetRowWithoutBlankCell();
            var allCellsArePopulated = systemUnderTest.AreAllCellsPopulated(rowWithBlankCell);

            Assert.AreEqual(true, allCellsArePopulated);
        }


        [TestMethod]
        public void AreAllCellsPopulated_RowWithEmptyCellShouldReturnFalse()
        {
            var rowWithBlankCell = GetRowWithBlankCell();
            var allCellsArePopulated = systemUnderTest.AreAllCellsPopulated(rowWithBlankCell);

            Assert.AreEqual(false, allCellsArePopulated);
        }

        [TestMethod]
        public void GetMappingsFromViewWithEmptyRowsRemoved_EmptyRowsShouldBeCorrectlyRemoved()
        {
            var lookUpMappings = new List<DataGridViewRow>();
            var rowWithoutBlankCell = GetRowWithoutBlankCell();
            var rowWithBlankCell = GetRowWithBlankCell();
            lookUpMappings.Add(rowWithoutBlankCell);
            lookUpMappings.Add(rowWithBlankCell);
            var updatedLookupMappings = systemUnderTest.GetMappingsFromViewWithEmptyRowsRemoved(lookUpMappings);
            Assert.AreEqual(1, updatedLookupMappings.Count);
        }

        [TestMethod]
        public void GetMappingsFromViewWithEmptyRowsRemoved_NoRowsShouldBeRemoved()
        {
            var lookUpMappings = new List<DataGridViewRow>();
            var rowWithoutBlankCell = GetRowWithoutBlankCell();
            var anotherRowWithoutBlankCell = GetRowWithoutBlankCell();
            lookUpMappings.Add(rowWithoutBlankCell);
            lookUpMappings.Add(anotherRowWithoutBlankCell);
            var updatedLookupMappings = systemUnderTest.GetMappingsFromViewWithEmptyRowsRemoved(lookUpMappings);
            Assert.AreEqual(2, updatedLookupMappings.Count);
        }

        private static DataGridViewRow GetRowWithoutBlankCell()
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "accountrelated" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "accountid" });
            return dataGridViewRow;
        }

        private static DataGridViewRow GetRowWithBlankCell()
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "Account" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell { Value = "accountid" });
            return dataGridViewRow;
        }
    }
}