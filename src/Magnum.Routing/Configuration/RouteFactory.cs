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
namespace Magnum.Routing.Configuration
{
    using System;
    using System.Linq;
    using Exceptions;
    using Magnum.Routing.Model;


    public class RouteFactory
	{
		static RouteFactory _instance;
		readonly UrlPatternParser _parser;

		RouteFactory()
		{
			_parser = new UrlPatternParser();
		}

		public static RouteFactory Current
		{
			get { return _instance ?? (_instance = new RouteFactory()); }
			set { _instance = value; }
		}

		public virtual Route New(UrlPattern pattern)
		{
			try
			{
				var routeSpecification = _parser.Parse(pattern);

				var routeDefinition = new RouteImpl(pattern, routeSpecification.Parameters, Enumerable.Empty<RouteVariable>());

				return routeDefinition;
			}
			catch (Exception ex)
			{
				throw new RoutingException("The route could not be created", ex);
			}
		}
	}
}