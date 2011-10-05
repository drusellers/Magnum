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


	public class JoinNode<TContext> :
		ActivationNodeBase<TContext>,
		RightActivation<TContext>
	{
		readonly long _id;
		readonly RightActivation<TContext> _rightActivation;

		public JoinNode(long id, RightActivation<TContext> rightActivation)
		{
			_id = id;
			_rightActivation = rightActivation;
		}

		public override void Activate(RouteContext<TContext> context, string value)
		{
			// right activate our join partner
			_rightActivation.RightActivate(context, x =>
				{
					// we were matched, so add our node to the right activation network
					// and add activation of our children to the agenda for execution
					context.AddRightActivation(_id, ()=>ActivateSuccessors(context, value));
				});
		}

		public void RightActivate(RouteContext<TContext> context, Action<RouteContext> callback)
		{
            //are there any right activations for me? if so, execute call back.
			if (context.HasRightActivation(_id))
				callback(context);
		}
	}
}