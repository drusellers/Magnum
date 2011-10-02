namespace Magnum.Routing.Model
{
    using System.Collections.Generic;


    public class RouteSpecification
    {
        IList<RouteParameter> _parameters;

        public RouteSpecification()
        {
            _parameters = new List<RouteParameter>();
        }

        public IEnumerable<RouteParameter> Parameters
        {
            get { return _parameters; }
        }

        public void AddParameter(RouteParameter parameter)
        {
            _parameters.Add(parameter);
        }
    }
}