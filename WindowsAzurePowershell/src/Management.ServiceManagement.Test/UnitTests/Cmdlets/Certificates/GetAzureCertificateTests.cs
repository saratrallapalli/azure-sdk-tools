﻿// ----------------------------------------------------------------------------------
//
// Copyright 2011 Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
namespace Microsoft.WindowsAzure.Management.ServiceManagement.Test.UnitTests.Cmdlets.Certificates
{
    using System.Collections;
    using System.Linq;
    using System.Management.Automation;
    using Extensions;
    using Management.Test.Stubs;
    using Samples.WindowsAzure.ServiceManagement;
    using ServiceManagement.Certificates;
    using Utilities;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetAzureCertificateTests
    {
        [TestInitialize]
        public void SetupTest()
        {
            CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();
        }

        [TestMethod]
        public void GetAzureCertificateSingleTest()
        {
            const string thumbprint = "thumb";
            const string thumbprintAlgorithm = "alg";

            // Setup
            SimpleServiceManagement channel = new SimpleServiceManagement();
            channel.GetCertificateThunk = ar => new Certificate { Thumbprint = thumbprint, ThumbprintAlgorithm = thumbprintAlgorithm };

            // Test
            GetAzureCertificate getAzureCertificate = new GetAzureCertificate(channel)
            {
                ShareChannel = true,
                CommandRuntime = new MockCommandRuntime()
            };

            getAzureCertificate.Thumbprint = thumbprint;
            getAzureCertificate.ThumbprintAlgorithm = thumbprintAlgorithm;
            getAzureCertificate.ExecuteCommand();

            Assert.AreEqual(1, ((MockCommandRuntime)getAzureCertificate.CommandRuntime).WrittenObjects.Count);

            IEnumerator enumerator = LanguagePrimitives.GetEnumerator(((MockCommandRuntime)getAzureCertificate.CommandRuntime).WrittenObjects);
            Assert.IsNotNull(enumerator);

            enumerator.MoveNext();
            Assert.IsTrue(((Certificate)enumerator.Current).Thumbprint.Equals(thumbprint) &&
                          ((Certificate)enumerator.Current).ThumbprintAlgorithm.Equals(thumbprintAlgorithm));
        }

        [TestMethod]
        public void GetAzureCertificateMultipleTest()
        {
            const string thumbprint1 = "thumb1";
            const string thumbprintAlgorithm1 = "alg1";

            const string thumbprint2 = "thumb2";
            const string thumbprintAlgorithm2 = "alg2";

            // Setup
            SimpleServiceManagement channel = new SimpleServiceManagement();
            channel.ListCertificatesThunk = ar => new CertificateList(new[]
            {
                new Certificate { Thumbprint = thumbprint1, ThumbprintAlgorithm = thumbprintAlgorithm1 },
                new Certificate { Thumbprint = thumbprint2, ThumbprintAlgorithm = thumbprintAlgorithm2 }
            });

            // Test
            GetAzureCertificate getAzureCertificate = new GetAzureCertificate(channel)
            {
                ShareChannel = true,
                CommandRuntime = new MockCommandRuntime()
            };

            getAzureCertificate.ExecuteCommand();

            Assert.AreEqual(1, ((MockCommandRuntime)getAzureCertificate.CommandRuntime).WrittenObjects.Count);

            IEnumerator enumerator = LanguagePrimitives.GetEnumerator(((MockCommandRuntime)getAzureCertificate.CommandRuntime).WrittenObjects.First());
            Assert.IsNotNull(enumerator);

            enumerator.MoveNext();
            Assert.IsTrue(((Certificate)enumerator.Current).Thumbprint.Equals(thumbprint1) &&
                          ((Certificate)enumerator.Current).ThumbprintAlgorithm.Equals(thumbprintAlgorithm1));

            enumerator.MoveNext();
            Assert.IsTrue(((Certificate)enumerator.Current).Thumbprint.Equals(thumbprint2) &&
                          ((Certificate)enumerator.Current).ThumbprintAlgorithm.Equals(thumbprintAlgorithm2));
        }
    }
}