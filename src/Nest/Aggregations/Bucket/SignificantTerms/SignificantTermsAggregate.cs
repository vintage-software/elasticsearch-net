using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest
{
	public class SignificantTermsAggregate<TKey> : MultiBucketAggregate<SignificantTermsBucket<TKey>>
	{
		public long DocCount { get; set; }
		public long? BgCount { get; set; }
	}
}
