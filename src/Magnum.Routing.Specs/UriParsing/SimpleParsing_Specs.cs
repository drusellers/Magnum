namespace Magnum.Routing.Specs.UriParsing
{
	using System;
	using Configuration;
	using Model;
	using TestFramework;


	[Scenario]
	public class SimpleParsing_Specs
	{
		[Then]
		public void Should_parse_tokens()
		{
		    var x = new UrlPatternParser();
		    var o = x.Parse(new UrlPattern("/agent/version/{id}"));
            //1
            //2
            //3
		}
	}
}