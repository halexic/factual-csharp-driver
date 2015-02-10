using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

/// <summary>
/// This is the response from API 
/// Json that is returned is transformed into this object
/// Fix the problem that this library returns now full C# object instead of only Json string
/// 
/// Example of use for deserialization:
/// Result result = Utility.JsonDeserialize<Result>(jsonResult);
/// </summary>
namespace FactualDriver
{
	public class Utility
	{
		public static TModel JsonDeserialize<TModel>(string json) where TModel : class
		{
			if (json != null)
			{
				//Deserialization
				var serializer = new DataContractJsonSerializer(typeof(TModel));
				using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
				{
					var result = serializer.ReadObject(stream) as TModel;
					return result;
				}
			}
			else
			{
				return null;
			}
		}
	}

	[DataContract]
	public class Result
	{
		[DataMember(Name = "version", IsRequired = false, EmitDefaultValue = true)]
		public int Version { get; set; }

		[DataMember(Name = "status", IsRequired = false, EmitDefaultValue = true)]
		public string Status { get; set; }

		[DataMember(Name = "response", IsRequired = false, EmitDefaultValue = true)]
		public Response Response { get; set; }
	}

	[DataContract]
	public class Response
	{
		[DataMember(Name = "data", IsRequired = false, EmitDefaultValue = true)]
		public List<ResponseRow> Rows { get; set; }

		[DataMember(Name = "included_rows", IsRequired = false, EmitDefaultValue = true)]
		public int IncludedRows { get; set; }
	}

	[DataContract]
	public class ResponseRow
	{
		[DataMember(Name = "address", IsRequired = false, EmitDefaultValue = true)]
		public string Address { get; set; }

		[DataMember(Name = "category_ids", IsRequired = false, EmitDefaultValue = true)]
    public List<int> CategoryIds { get; set; }

		[DataMember(Name = "category_labels", IsRequired = false, EmitDefaultValue = true)]
    public List<List<string>> CategoryLabels { get; set; }

		[DataMember(Name = "country", IsRequired = false, EmitDefaultValue = true)]
    public string Country { get; set; }

		[DataMember(Name = "email", IsRequired = false, EmitDefaultValue = true)]
    public string Email { get; set; }

		[DataMember(Name = "factual_id", IsRequired = false, EmitDefaultValue = true)]
    public string FactualId { get; set; }

		[DataMember(Name = "fax", IsRequired = false, EmitDefaultValue = true)]
    public string Fax { get; set; }

		[DataMember(Name = "hours", IsRequired = false, EmitDefaultValue = true)]
    public Hours Hours { get; set; }

		[DataMember(Name = "hours_display", IsRequired = false, EmitDefaultValue = true)]
    public string HoursDisplay { get; set; }

		[DataMember(Name = "latitude", IsRequired = false, EmitDefaultValue = true)]
		public double Latitude { get; set; }

		[DataMember(Name = "longitude", IsRequired = false, EmitDefaultValue = true)]
		public double Longitude { get; set; }

		[DataMember(Name = "locality", IsRequired = false, EmitDefaultValue = true)]
    public string Locality { get; set; }

		[DataMember(Name = "name", IsRequired = false, EmitDefaultValue = true)]
    public string Name { get; set; }

		[DataMember(Name = "neighborhood", IsRequired = false, EmitDefaultValue = true)]
    public List<string> Neighborhood { get; set; }

		[DataMember(Name = "postcode", IsRequired = false, EmitDefaultValue = true)]
    public string Postcode { get; set; }

		[DataMember(Name = "region", IsRequired = false, EmitDefaultValue = true)]
    public string Region { get; set; }

		[DataMember(Name = "tel", IsRequired = false, EmitDefaultValue = true)]
    public string Tel { get; set; }

		[DataMember(Name = "website", IsRequired = false, EmitDefaultValue = true)]
		public string Website { get; set; }

		[DataMember(Name = "distance", IsRequired = false, EmitDefaultValue = true)]
    public double Distance { get; set; }
	}

	[DataContract]
	public class Hours
	{
		[DataMember(Name = "monday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Monday { get; set; }

		[DataMember(Name = "tuesday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Tuesday { get; set; }

		[DataMember(Name = "wednesday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Wednesday { get; set; }

		[DataMember(Name = "thursday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Thursday { get; set; }

		[DataMember(Name = "friday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Friday { get; set; }

		[DataMember(Name = "saturday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Saturday { get; set; }

		[DataMember(Name = "sunday", IsRequired = false, EmitDefaultValue = true)]
		public List<List<string>> Sunday { get; set; }
	}
}
