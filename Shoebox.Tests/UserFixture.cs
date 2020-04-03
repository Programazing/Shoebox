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
    public class UserFixture
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
            if (File.Exists(SettingsFile.SettingsPath))
            {
                File.Delete(SettingsFile.SettingsPath);
            }
        }

        [Test]
        public void Shoebox_Adds_AUser()
        {
            SystemUnderTest.AddUser(TestHelpers.GetJohnDoe());

            SystemUnderTest
                .Settings.UserSettings.Users
                .Where(x => x.UserName == "JohnDoe").FirstOrDefault()
                .UserName.Should().Be("JohnDoe");
        }

        [Test]
        public void Shoebox_WontAdd_AUser_WhenUserNameAlreadyExists()
        {
            SystemUnderTest.AddUser(TestHelpers.Users().Where(x => x.UserName == "DefaultUser").FirstOrDefault());

            SystemUnderTest.Settings.UserSettings
                .Users.Count().Should().Be(1);
        }

        [Test]
        public void Shoebox_Removes_AUser()
        {
            SystemUnderTest.AddUser(TestHelpers.GetJohnDoe());

            SystemUnderTest.RemoveUser(TestHelpers.GetJohnDoe());

            SystemUnderTest.Settings.UserSettings
                .Users.Count().Should().Be(1);
        }

       [Test]
        public void Shoebox_Updates_AUser()
        {
            SystemUnderTest.AddUser(TestHelpers.GetJohnDoe());

            var user = TestHelpers.GetJohnDoe();

            user.WatchedDirectories.FirstOrDefault().Path = "NewDirectory";

            SystemUnderTest.UpdateUser(user);

            SystemUnderTest.Settings.UserSettings
                .Users.Single(x => x.UserName == "JohnDoe")
                .WatchedDirectories.Should().Contain(x => x.Path == "NewDirectory");
        }

        [Test]
        public void ShoeBox_LoadsUsersFileAssociations_IntoAUsableObject()
        {
            SystemUnderTest.AddUser(TestHelpers.GetJohnDoe());

            var user = SystemUnderTest.Settings.UserSettings.Users.Where(x => x.UserName == "JohnDoe").FirstOrDefault();
                
            user.FileAssociations.Count().Should().Be(2);

            user.FileAssociations.FirstOrDefault().Name.Should().Be("Images");
        }

        [Test]
        public void ShoeBox_LoadsUsersWatchedDirectories_IntoAUsableObject()
        {
            var user = TestHelpers.GetJohnDoe();

            SystemUnderTest.AddUser(user);

            user.WatchedDirectories.Count().Should().Be(2);

            user.WatchedDirectories.FirstOrDefault().Path.Should().Be(TestHelpers.Downloads);
        }

        [Test]
        public void ShoeBox_Update_CurrentUser()
        {
            var user = TestHelpers.GetJohnDoe();
            SystemUnderTest.AddUser(user);

            SystemUnderTest.ChangeCurrentUser(user);

            SystemUnderTest.Settings.UserSettings.CurrentUser.Should().Be(user.UserName);
        }
    }
}
