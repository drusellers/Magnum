namespace Magnum.Routing.Specs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Engine;
	using Engine.Nodes;
	using NUnit.Framework;
	using TestFramework;


	[TestFixture]
	public class Join_Specs
	{
		long _id = 1;

		[Test]
		public void FirstTestName()
		{
			var route = new RouteNode<Dictionary<string, string>>(new StubRoute<Dictionary<string, string>>("a"));

            //join nodes join by being left activated by their left node
            //and they take in a right activation node that if true will callback.
			var joinNode = new JoinNode<Dictionary<string, string>>(_id++, new ConstantNode<Dictionary<string,string>>());
			joinNode.AddSuccessor(route);

			var alpha = new AlphaNode<Dictionary<string, string>>(_id++);
			alpha.AddSuccessor(joinNode);

			var equal = new EqualNode<Dictionary<string, string>>(() => _id++);
			equal.AddCheck("version", alpha);

			var segment = new SegmentNode<Dictionary<string, string>>(1);
			segment.AddSuccessor(equal);

			var engine = new MagnumRoutingEngine<Dictionary<string, string>>(x => new Uri(x["uri"]));
			engine.Root.AddSuccessor(segment);

			bool called = false;

			var uri = new Uri("http://localhost/version");
		    var dict = new Dictionary<string, string>();
            dict.Add("uri",uri.ToString());
			engine.Route(dict, x =>
				{
					called = true;
				});

			called.ShouldBeTrue();
		}
	}
}
