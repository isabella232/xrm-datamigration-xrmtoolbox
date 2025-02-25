﻿using Capgemini.Xrm.CdsDataMigratorLibrary.Core;
using Capgemini.Xrm.CdsDataMigratorLibrary.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capgemini.Xrm.CdsDataMigrator.Tests.Unit.Core
{
    [TestClass]
    public class SettingFileHandlerTest
    {
        [TestMethod]
        public void GetConfigData()
        {
            SettingFileHandler.GetConfigData<ServiceParameters>(out Settings actual);

            actual.Should().NotBeNull();
        }
    }
}