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
namespace Magnum.Routing.Configuration.Builders
{
    using System;


    public class DelegateRouteBuilder<TContext> :
		RouteBuilder<TContext>
	{
		readonly Action<TContext> _callback;
		readonly Route _definition;

		public DelegateRouteBuilder(Route definition, Action<TContext> callback)
		{
			_definition = definition;
			_callback = callback;
		}

		public void Build(RoutingEngine<TContext> engine)
		{
		}
	}
}