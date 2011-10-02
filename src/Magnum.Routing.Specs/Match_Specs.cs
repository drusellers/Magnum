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
	using System.Linq;
	using Engine;
	using Engine.Nodes;
	using NUnit.Framework;
	using TestFramework;


	[TestFixture]
	public class Matching_a_segment_condition
	{
		[Test]
		public void Should_find_the_existing_equal_condition()
		{
			_engine.Match<SegmentNode<Uri>>()
				.Where(x => x.Position == 1)
				.Match<SegmentNode<Uri>, EqualNode<Uri>>()
				.Any()
				.ShouldBeTrue();
		}

		[Test]
		public void Should_find_the_existing_segment_condition()
		{
			_engine.Match<SegmentNode<Uri>>()
				.Where(x => x.Position == 1)
				.Any()
				.ShouldBeTrue();
		}

		[Test]
		public void Should_route_the_url()
		{
            var shouldHaveRouted = false;

			var uri = new Uri("http://localhost/version");

			_engine.Route(uri, x =>
			    {
                    shouldHaveRouted = true;
			        Trace.WriteLine("Hello");
			    });

		    shouldHaveRouted.ShouldBeTrue();
		}

		MagnumRoutingEngine<Uri> _engine;
		long _id = 1;

		[TestFixtureSetUp]
		public void Given_an_existing_segment_condition()
		{
            //here we are just using a simple context - where its just the uri.
			_engine = new MagnumRoutingEngine<Uri>(x => x);

			var segmentNode = new SegmentNode<Uri>(1);
			var equals = new EqualNode<Uri>(() => _id++);

            //this alpha node has nothing to do. so we hit it and then 'thbbbb'
		    var alphaNode = new AlphaNode<Uri>(_id++);
		    equals.Add("version", alphaNode);
			segmentNode.AddActivation(equals);


			_engine.Add(segmentNode);
		}
	}
}