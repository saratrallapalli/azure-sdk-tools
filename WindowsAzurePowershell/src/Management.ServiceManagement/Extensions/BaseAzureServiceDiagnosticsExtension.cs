﻿// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.ServiceManagement.Extensions
{
    using System;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Properties;
    using Storage;
    using Storage.Auth;
    using Utilities.CloudService;
    using Utilities.Common;
    using WindowsAzure.ServiceManagement;

    public abstract class BaseAzureServiceDiagnosticsExtensionCmdlet : BaseAzureServiceExtensionCmdlet
    {
        protected const string ConnectionQualifiersElemStr = "ConnectionQualifiers";
        protected const string DefaultEndpointsProtocolElemStr = "DefaultEndpointsProtocol";
        protected const string StorageAccountElemStr = "StorageAccount";
        protected const string StorageNameElemStr = "Name";
        protected const string StorageKeyElemStr = "StorageKey";
        protected const string WadCfgElemStr = "WadCfg";
        protected const string DiagnosticsExtensionNamespace = "Microsoft.Windows.Azure.Extensions";
        protected const string DiagnosticsExtensionType = "Diagnostics";

        protected string StorageKey { get; set; }
        protected string ConnectionQualifiers { get; set; }
        protected string DefaultEndpointsProtocol { get; set; }

        public virtual string StorageAccountName { get; set; }
        public virtual XmlDocument DiagnosticsConfiguration { get; set; }

        public BaseAzureServiceDiagnosticsExtensionCmdlet()
            : base()
        {
            Initialize();
        }

        public BaseAzureServiceDiagnosticsExtensionCmdlet(IServiceManagement channel)
            : base(channel)
        {
            Initialize();
        }

        protected void Initialize()
        {
            ExtensionNameSpace = DiagnosticsExtensionNamespace;
            ExtensionType = DiagnosticsExtensionType;

            XNamespace configNameSpace = "http://schemas.microsoft.com/ServiceHosting/2010/10/DiagnosticsConfiguration";

            PublicConfigurationXmlTemplate = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(configNameSpace + PublicConfigStr,
                    new XElement(configNameSpace + StorageAccountElemStr,
                        new XElement(configNameSpace + ConnectionQualifiersElemStr, ""),
                        new XElement(configNameSpace + DefaultEndpointsProtocolElemStr, ""),
                        new XElement(configNameSpace + StorageNameElemStr, "")
                    ),
                    new XElement(configNameSpace + WadCfgElemStr, "")
                )
            );

            PrivateConfigurationXmlTemplate = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XProcessingInstruction("xml-stylesheet", @"type=""text/xsl"" href=""style.xsl"""),
                new XElement(PrivateConfigStr,
                    new XElement(StorageKeyElemStr, "{0}")
                )
            );
        }

        protected void ValidateStorageAccount()
        {
            var cloudServiceClient = new CloudServiceClient(
                CurrentSubscription,
                SessionState.Path.CurrentLocation.Path,
                WriteDebug,
                WriteVerbose,
                WriteWarning
                );
            if (cloudServiceClient == null)
            {
                throw new Exception(string.Format(Resources.ServiceExtensionCannotFindServiceClient, CurrentSubscription.SubscriptionId));
            }

            var storageService = cloudServiceClient.GetStorageService(StorageAccountName);
            if (storageService == null)
            {
                throw new Exception(string.Format(Resources.ServiceExtensionCannotFindStorageAccountName, StorageAccountName));
            }

            if (storageService.StorageServiceKeys == null)
            {
                throw new Exception(string.Format(Resources.ServiceExtensionCannotFindStorageAccountKey, StorageAccountName));
            }
            StorageKey = storageService.StorageServiceKeys.Primary;

            StorageCredentials credentials = new StorageCredentials(
                storageService.ServiceName,
                storageService.StorageServiceKeys.Primary);

            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(
                credentials,
                General.CreateHttpsEndpoint(storageService.StorageServiceProperties.Endpoints[0]),
                General.CreateHttpsEndpoint(storageService.StorageServiceProperties.Endpoints[1]),
                General.CreateHttpsEndpoint(storageService.StorageServiceProperties.Endpoints[2])
                );
            if (cloudStorageAccount == null)
            {
                throw new Exception(string.Format(Resources.ServiceExtensionCannotFindStorageAccount, StorageAccountName));
            }

            StringBuilder endpointStr = new StringBuilder();
            endpointStr.AppendFormat("BlobEndpoint={0};", cloudStorageAccount.BlobEndpoint.ToString());
            endpointStr.AppendFormat("QueueEndpoint={0};", cloudStorageAccount.QueueEndpoint.ToString());
            endpointStr.AppendFormat("TableEndpoint={0}", cloudStorageAccount.TableEndpoint.ToString());
            ConnectionQualifiers = endpointStr.ToString();
            DefaultEndpointsProtocol = "https";
        }

        protected override void ValidateConfiguration()
        {
            PublicConfigurationXml = new XDocument(PublicConfigurationXmlTemplate);
            SetPublicConfigValue(ConnectionQualifiersElemStr, ConnectionQualifiers);
            SetPublicConfigValue(DefaultEndpointsProtocolElemStr, DefaultEndpointsProtocol);
            SetPublicConfigValue(StorageNameElemStr, StorageAccountName);
            SetPublicConfigValue(WadCfgElemStr, DiagnosticsConfiguration);
            PublicConfiguration = PublicConfigurationXml.ToString();

            PrivateConfigurationXml = new XDocument(PrivateConfigurationXmlTemplate);
            SetPrivateConfigValue(StorageKeyElemStr, StorageKey);
            PrivateConfiguration = PrivateConfigurationXml.ToString();
        }
    }
}
