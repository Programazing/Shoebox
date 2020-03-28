﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shoebox.Tests
{
    [TestFixture]
    public class ShoeboxFixture
    {
        private Common.Shoebox SystemUnderTest;

        [SetUp]
        public void Setup()
        {
            SystemUnderTest = new Common.Shoebox();
        }

        [TearDown]
        public void Teardown()
        {
            
        }

        [Test]
        public void ShoeBox_LoadsSettingsFile_WhenNotGivenAPath()
        {
            SystemUnderTest.Settings.UserSettings.Users.FirstOrDefault().UserName.Should().Be("DefaultUser");
        }
    }
}