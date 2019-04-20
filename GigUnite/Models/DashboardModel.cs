using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GigUnite.Models
{
    public class DashboardModel
    {
        public List<Gig> RecommendedGigs;
		public List<Gig> InterestedGigs;
	}
}