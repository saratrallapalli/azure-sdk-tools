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

namespace Microsoft.WindowsAzure.Management.ServiceManagement.Extensions
{
    using DomainJoin;

    public class DomainJoinExtensionContext : ExtensionContext
    {
        public string DomainName { get; set; }
        public string Server { get; set; }
        public JoinOptions[] Options { get; set; }
        public string OUPath { get; set; }
        public string WorkGroupName { get; set; }
        public string User { get; set; }
        public string LocalUser { get; set; }
        public string UnjoinDomainUser { get; set; }
        public string NewName { get; set; }
        public bool Restart { get; set; }
        public bool Unsecure { get; set; }
    }
}
