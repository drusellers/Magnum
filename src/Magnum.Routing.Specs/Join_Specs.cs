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
			var route = new RouteNode<Dictionary<string, string>>(new StubRoute<Dictionary<string, string>>());

			var joinNode = new JoinNode<Dictionary<string, string>>(_id++, new ConstantNode<Dictionary<string,string>>());
			joinNode.AddActivation(route);

			var alpha = new AlphaNode<Dictionary<string, string>>(_id++);
			alpha.AddActivation(joinNode);

			var equal = new EqualNode<Dictionary<string, string>>(() => _id++);
			equal.Add("version", alpha);

			var segment = new SegmentNode<Dictionary<string, string>>(1);
			segment.AddActivation(equal);

			var engine = new MagnumRoutingEngine<Uri>(x => x);
			engine.Match<RootNode<Dictionary<string, string>>>().Single().AddActivation(segment);

			bool called = false;

			var uri = new Uri("http://localhost/version");
			engine.Route(uri, x =>
				{
					called = true;
				});

			called.ShouldBeTrue();
		}
	}
}
