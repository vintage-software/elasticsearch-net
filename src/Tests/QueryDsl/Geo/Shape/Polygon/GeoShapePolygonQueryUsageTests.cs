﻿using System.Collections.Generic;
using Nest;
using Tests.Framework.Integration;
using Tests.Framework.ManagedElasticsearch.Clusters;
using Tests.Framework.MockData;
using static Nest.Infer;

namespace Tests.QueryDsl.Geo.Shape.Polygon
{
	public class GeoShapePolygonQueryUsageTests : QueryDslUsageTestsBase
	{
		public GeoShapePolygonQueryUsageTests(ReadOnlyCluster i, EndpointUsage usage) : base(i, usage) { }

		private readonly IEnumerable<IEnumerable<GeoCoordinate>> _polygonCoordinates = new[]
		{
			new GeoCoordinate[]
			{
				new [] {-17.0, 10.0}, new [] {16.0, 15.0}, new [] {12.0, 0.0}, new [] {16.0, -15.0}, new [] {-17.0, -10.0}, new [] {-17.0, 10.0}
			},
			new GeoCoordinate[]
			{
				new [] {18.2, 8.2}, new [] {-18.8, 8.2}, new [] {-10.8, -8.8}, new [] {18.2, 8.8}
			}
		};

		protected override object QueryJson => new
		{
			geo_shape = new
			{
				_name="named_query",
				boost = 1.1,
				ignore_unmapped = true,
				location = new
				{
					relation = "intersects",
					shape = new
					{
						type = "polygon",
						coordinates = this._polygonCoordinates
					}
				}
			}
		};

		protected override QueryContainer QueryInitializer => new GeoShapePolygonQuery
		{
			Name = "named_query",
			Boost = 1.1,
			Field = Field<Project>(p => p.Location),
			Shape = new PolygonGeoShape(this._polygonCoordinates),
			Relation = GeoShapeRelation.Intersects,
			IgnoreUnmapped = true
		};

		protected override QueryContainer QueryFluent(QueryContainerDescriptor<Project> q) => q
			.GeoShapePolygon(c => c
				.Name("named_query")
				.Boost(1.1)
				.Field(p => p.Location)
				.Coordinates(this._polygonCoordinates)
				.Relation(GeoShapeRelation.Intersects)
				.IgnoreUnmapped(true)
			);

		protected override ConditionlessWhen ConditionlessWhen => new ConditionlessWhen<IGeoShapePolygonQuery>(a => a.GeoShape as IGeoShapePolygonQuery)
		{
			q =>  q.Field = null,
			q =>  q.Shape = null,
			q =>  q.Shape.Coordinates = null
		};
	}
}
