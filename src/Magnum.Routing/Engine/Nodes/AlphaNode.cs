// Copyright 2007-2010 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Routing.Engine.Nodes
{
    using System;
    using System.Diagnostics;


    /// <summary>
	/// An alpha node marks the end of a branch in the left side discrimination network
	/// and starts the journey into the right side join network
	/// </summary>
	[DebuggerDisplay("Alpha:{_id}")]
    public class AlphaNode<TContext> : 
		ActivationNodeBase<TContext>,
        RightActivation<TContext>
    {
        //this is used so the node can ask for their context data.
		readonly long _id;

		public AlphaNode(long id)
		{
			_id = id;
		}

		public override void Activate(RouteContext<TContext> context, string value)
		{
            // facts / data  - normal rete network.
            // data / facts sit on the node.
            // we don't want to store state on the nodes - because we are building a routing / stateless network
            // 
			context.AddRightActivation(_id, () => ActivateSuccessors(context, value));

            //DRU: I have hit an alpha node. This is the end of the Alpha network.
		}

        public void RightActivate(RouteContext<TContext> context, Action<RouteContext> callback)
        {
            callback(context);
        }
    }
}