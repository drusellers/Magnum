namespace Magnum.Routing.Model
{
    using System.Diagnostics;

    [DebuggerDisplay("Static:{Value}")]
    class StaticRouteParameter :
        RouteParameter
    {
        public StaticRouteParameter(int segmentIndex, string value)
        {
            SegmentIndex = segmentIndex;
            Value = value;
        }

        public int SegmentIndex { get; set; }
        public string Value { get; private set; }
    }
}