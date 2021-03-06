/*
 * Copyright 2010-2014 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

/*
 * Do not modify this file. This file is generated from the codedeploy-2014-10-06.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.CodeDeploy.Model
{
    /// <summary>
    /// Container for the parameters to the CreateDeployment operation.
    /// Deploys an application revision to the specified deployment group.
    /// </summary>
    public partial class CreateDeploymentRequest : AmazonCodeDeployRequest
    {
        private string _applicationName;
        private string _deploymentConfigName;
        private string _deploymentGroupName;
        private string _description;
        private bool? _ignoreApplicationStopFailures;
        private RevisionLocation _revision;

        /// <summary>
        /// Gets and sets the property ApplicationName. 
        /// <para>
        /// The name of an existing AWS CodeDeploy application within the AWS user account.
        /// </para>
        /// </summary>
        public string ApplicationName
        {
            get { return this._applicationName; }
            set { this._applicationName = value; }
        }

        // Check to see if ApplicationName property is set
        internal bool IsSetApplicationName()
        {
            return this._applicationName != null;
        }

        /// <summary>
        /// Gets and sets the property DeploymentConfigName. 
        /// <para>
        /// The name of an existing deployment configuration within the AWS user account.
        /// </para>
        ///  
        /// <para>
        /// If not specified, the value configured in the deployment group will be used as the
        /// default. If the deployment group does not have a deployment configuration associated
        /// with it, then CodeDeployDefault.OneAtATime will be used by default.
        /// </para>
        /// </summary>
        public string DeploymentConfigName
        {
            get { return this._deploymentConfigName; }
            set { this._deploymentConfigName = value; }
        }

        // Check to see if DeploymentConfigName property is set
        internal bool IsSetDeploymentConfigName()
        {
            return this._deploymentConfigName != null;
        }

        /// <summary>
        /// Gets and sets the property DeploymentGroupName. 
        /// <para>
        /// The deployment group's name.
        /// </para>
        /// </summary>
        public string DeploymentGroupName
        {
            get { return this._deploymentGroupName; }
            set { this._deploymentGroupName = value; }
        }

        // Check to see if DeploymentGroupName property is set
        internal bool IsSetDeploymentGroupName()
        {
            return this._deploymentGroupName != null;
        }

        /// <summary>
        /// Gets and sets the property Description. 
        /// <para>
        /// A comment about the deployment.
        /// </para>
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        // Check to see if Description property is set
        internal bool IsSetDescription()
        {
            return this._description != null;
        }

        /// <summary>
        /// Gets and sets the property IgnoreApplicationStopFailures. 
        /// <para>
        /// If set to true, then if the deployment causes the ApplicationStop deployment lifecycle
        /// event to fail to a specific instance, the deployment will not be considered to have
        /// failed to that instance at that point and will continue on to the BeforeInstall deployment
        /// lifecycle event.
        /// </para>
        ///  
        /// <para>
        /// If set to false or not specified, then if the deployment causes the ApplicationStop
        /// deployment lifecycle event to fail to a specific instance, the deployment will stop
        /// to that instance, and the deployment to that instance will be considered to have failed.
        /// </para>
        /// </summary>
        public bool IgnoreApplicationStopFailures
        {
            get { return this._ignoreApplicationStopFailures.GetValueOrDefault(); }
            set { this._ignoreApplicationStopFailures = value; }
        }

        // Check to see if IgnoreApplicationStopFailures property is set
        internal bool IsSetIgnoreApplicationStopFailures()
        {
            return this._ignoreApplicationStopFailures.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property Revision. 
        /// <para>
        /// The type of revision to deploy, along with information about the revision's location.
        /// </para>
        /// </summary>
        public RevisionLocation Revision
        {
            get { return this._revision; }
            set { this._revision = value; }
        }

        // Check to see if Revision property is set
        internal bool IsSetRevision()
        {
            return this._revision != null;
        }

    }
}