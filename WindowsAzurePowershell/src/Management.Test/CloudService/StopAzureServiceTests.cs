﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
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

namespace Microsoft.WindowsAzure.Management.Test.CloudService
{
    using Microsoft.WindowsAzure.Management.CloudService;
    using Microsoft.WindowsAzure.Management.Test.Utilities.Common;
    using Microsoft.WindowsAzure.Management.Utilities.CloudService;
    using Microsoft.WindowsAzure.Management.Utilities.Common;
    using Microsoft.WindowsAzure.ServiceManagement;
    using Moq;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StopAzureServiceTests : TestBase
    {
        private const string serviceName = "AzureService";

        string slot = DeploymentSlotType.Production;

        private MockCommandRuntime mockCommandRuntime;

        private StopAzureServiceCommand stopServiceCmdlet;

        private Mock<ICloudServiceClient> cloudServiceClientMock;

        [TestInitialize]
        public void SetupTest()
        {
            GlobalPathInfo.GlobalSettingsDirectory = Data.AzureSdkAppDir;
            CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();
            mockCommandRuntime = new MockCommandRuntime();
            cloudServiceClientMock = new Mock<ICloudServiceClient>();

            stopServiceCmdlet = new StopAzureServiceCommand()
            {
                CloudServiceClient = cloudServiceClientMock.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void TestStopAzureService()
        {
            stopServiceCmdlet.ServiceName = serviceName;
            stopServiceCmdlet.Slot = slot;
            cloudServiceClientMock.Setup(f => f.StopCloudService(serviceName, slot));

            using (FileSystemHelper files = new FileSystemHelper(this))
            {
                files.CreateAzureSdkDirectoryAndImportPublishSettings();
                AzureService service = new AzureService(files.RootPath, serviceName, null);
                stopServiceCmdlet.ExecuteCmdlet();

                Assert.AreEqual<int>(0, mockCommandRuntime.OutputPipeline.Count);
                cloudServiceClientMock.Verify(f => f.StopCloudService(serviceName, slot), Times.Once());
            }
        }
    }
}