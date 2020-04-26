﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
using DotNetNuke.Authentication.Google.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Authentication.OAuth;

using NUnit.Framework;

namespace DotNetNuke.Tests.Authentication
{
    [TestFixture]
    public class GoogleUserDataTests
    {
        private const string SampleUserJson = @"{
 ""id"": ""108453011160062628399"",
 ""name"": ""Fred the Dinosaur"",
 ""given_name"": ""Frederick"",
 ""family_name"": ""Franklin"",
 ""link"": ""https://plus.google.com/108453011160062628399"",
 ""picture"": ""https://lh3.googleusercontent.com/-aii-uOqdr1M/AAAAAAAAAAI/AAAAAAAAADg/dNL75Dg7lbc/photo.jpg"",
 ""gender"": ""male"",
 ""locale"": ""en""
}";

        [Test]
        public void GoogleUserData_Populates_Inherited_Name_Properties_When_Deserialized()
        {
            // Act
            UserData dukesUser = Json.Deserialize<GoogleUserData>(SampleUserJson);

            // Assert
            Assert.AreEqual("Frederick", dukesUser.FirstName, "Should correctly pull first name from given_name field, not by parsing name");
            Assert.AreEqual("Franklin", dukesUser.LastName, "Should correctly pull first name from family_name field, not by parsing name");
        }

        [Test]
        public void GoogleUserData_Populates_Inherited_ProfileImage_Property_When_Deserialized()
        {
            // Act
            UserData dukesUser = Json.Deserialize<GoogleUserData>(SampleUserJson);

            // Assert
            Assert.AreEqual("https://lh3.googleusercontent.com/-aii-uOqdr1M/AAAAAAAAAAI/AAAAAAAAADg/dNL75Dg7lbc/photo.jpg", dukesUser.ProfileImage);
        }
    }
}
