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
namespace Magnum.Routing.Specs
{
	using System;
	using System.Diagnostics;
	using Model;


    [DebuggerDisplay("StubRoute:{_name}")]
	public class StubRoute<T> :
		Route<T>
	{
	    string _name;

	    public StubRoute()
	    {
	        _name = Guid.NewGuid().ToString();
	    }

	    public StubRoute(string name)
	    {
	        _name = name;
	    }

	    public string Url
		{
			get { throw new NotImplementedException(); }
		}

		public RouteParameters Parameters
		{
			get { throw new NotImplementedException(); }
		}

		public RouteVariables Variables
		{
			get { throw new NotImplementedException(); }
		}
	}
}