// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.GUI.ViewModel;
using SQLDataProducer.GUI.Design;
using SQLDataProducer.GUI.Model;


namespace SQLDataProducer.Tests.ViewModels
{
    [TestFixture]
    [MSTest.TestClass]
    public class ProjectModelTest
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldInstansiateModel()
        {
            var model = new ProjectModel();

            Assert.That(model.RootNode, Is.Not.Null);
            Assert.That(model.RootNode.Children, Is.Not.Null);
            Assert.That(model.RootNode.Children.Count(), Is.EqualTo(0));

            Assert.That(model.Tables, Is.Not.Null);
            Assert.That(model.Tables.Count, Is.EqualTo(0));
        }

    }
}
