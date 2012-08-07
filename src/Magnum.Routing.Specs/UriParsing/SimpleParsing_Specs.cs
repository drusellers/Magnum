namespace Magnum.Routing.Specs.UriParsing
{
	using Configuration;
	using Model;
	using TestFramework;
	using System.Linq;
	using Extensions;


    [Scenario]
	public class SimpleParsing_Specs
	{
		[Then]
		public void Should_parse_tokens()
		{
		    var x = new UrlPatternParser();
		    var o = x.Parse(new UrlPattern("/agent/version/{id}"));
		    o.Parameters.First().ShouldBeAnInstanceOf<StaticRouteParameter>();
		    o.Parameters.Skip(1).First().ShouldBeAnInstanceOf<StaticRouteParameter>();
		    o.Parameters.Skip(2).First().ShouldBeAnInstanceOf<VariableRouteParameter>();

		    var p = o.Parameters.Skip(2)
		        .First()
		        .CastAs<VariableRouteParameter>();

		    p.SegmentIndex.ShouldEqual(3);
		    p.Value.ShouldEqual("id");
		}
	}
}