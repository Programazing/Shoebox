using FluentAssertions;
using NUnit.Framework;
using Shoebox.Common;
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
            TestHelpers.CreateFoldersForJohn();
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(SettingsFile.SettingsPath))
            {
                File.Delete(SettingsFile.SettingsPath);
            }

            TestHelpers.DeleteFoldersForJohn();
        }

        [Test]
        public void ShoeBox_LoadsSettingsFile_WhenNotGivenAPath()
        {
            SystemUnderTest.Settings.UserSettings.Users.FirstOrDefault()
                .UserName.Should().Be("DefaultUser");
        }

        [Test]
        public void ShoeBox_LoadsSettingsFile_WhenGivenAPath()
        {
            SystemUnderTest = new Common.Shoebox(TestHelpers.DesktopLocation());

            SystemUnderTest.Settings.UserSettings.Users.FirstOrDefault()
                .UserName.Should().Be("DefaultUser");
        }

        [Test]
        public void ShoeBox_CanMoveCorrectFileTypes_FromWatchedDirectories_ToDestinationFolders()
        {
            var user = TestHelpers.GetJohnDoe();
            SystemUnderTest.AddUser(user);

            SystemUnderTest.ChangeCurrentUser(user);

            SystemUnderTest.Start();

            var fileIsThere = File.Exists(Path.Combine(TestHelpers.Downloads, "test.txt"));

            fileIsThere.Should().BeTrue();
        }
    }
}
